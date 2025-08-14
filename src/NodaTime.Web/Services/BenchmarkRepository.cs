// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using NodaTime.Benchmarks;
using NodaTime.Web.Helpers;
using NodaTime.Web.Middleware;

namespace NodaTime.Web.Services;

public class BenchmarkRepository : IRefreshableCache
{
    private const int DownloadConcurrency = 10;

    private readonly ILogger logger;
    private readonly IStorageRepository storage;
    private readonly int limit;

    private const string EnvironmentObjectName = "benchmarks/environments.pb";
    private const string ContainerObjectsPrefix = "benchmarks/benchmark-run-";

    private CacheEntry currentEntry;

    public BenchmarkRepository(
        ILogger<BenchmarkRepository> logger,
        IStorageRepository storage,
        int limit)
    {
        this.logger = logger;
        this.storage = storage;
        this.limit = limit;
        currentEntry = CacheEntry.CreateEmpty(this);
    }


    public IList<BenchmarkEnvironment> ListEnvironments() => currentEntry.Environments;
    public BenchmarkEnvironment? GetEnvironment(string environmentId) => currentEntry.EnvironmentsById.GetValueOrNull(environmentId);
    public BenchmarkType? GetType(string benchmarkTypeId) => currentEntry.TypesById.GetValueOrNull(benchmarkTypeId);
    public BenchmarkRun? GetRun(string benchmarkRunId) => currentEntry.RunsById.GetValueOrNull(benchmarkRunId);
    public Benchmark? GetBenchmark(string benchmarkId) => currentEntry.BenchmarksById.GetValueOrNull(benchmarkId);
    public IList<BenchmarkType> GetTypesByCommitAndType(string commit, string fullTypeName) =>
        currentEntry.TypesByCommitAndFullName[(commit, fullTypeName)].ToList();

    // Note: it's fine for this to fail part way through. The Refresh method deliberately
    // keeps track of "pending downloads"
    public async Task Refresh(CancellationToken cancellationToken) =>
        currentEntry = await currentEntry.Refresh(cancellationToken);

    private class CacheEntry
    {
        private readonly BenchmarkRepository repository;

        public IList<BenchmarkEnvironment> Environments { get; }
        public IDictionary<string, BenchmarkEnvironment> EnvironmentsById { get; }
        public IDictionary<string, BenchmarkRun> RunsById { get; }
        public IDictionary<string, BenchmarkType> TypesById { get; }
        public IDictionary<string, Benchmark> BenchmarksById { get; }
        public ILookup<(string, string), BenchmarkType> TypesByCommitAndFullName { get; }

        private readonly string environmentCrc32c;
        // Key is the storage object name.
        private readonly Dictionary<string, BenchmarkRun> runsByStorageName;

        // The pending state we're still loading
        private List<string>? pendingContainersToBeDownloaded;
        private string? pendingCrc;
        private List<BenchmarkEnvironment>? pendingEnvironments;
        private Dictionary<string, BenchmarkRun>? pendingDownloadedContainers;
        private List<string>? pendingContainerNames;

        public static CacheEntry CreateEmpty(BenchmarkRepository repository) => new(repository, [], "", []);

        private CacheEntry(
            BenchmarkRepository repository,
            IList<BenchmarkEnvironment> environments,
            string environmentCrc32c,
            Dictionary<string, BenchmarkRun> runsByStorageName)
        {
            this.repository = repository;
            Environments = environments;

            this.environmentCrc32c = environmentCrc32c;
            this.runsByStorageName = runsByStorageName;

            EnvironmentsById = Environments.ToDictionary(e => e.BenchmarkEnvironmentId);
            RunsById = runsByStorageName.Values.ToDictionary(r => r.BenchmarkRunId);
            TypesById = RunsById.Values.SelectMany(r => r.Types_).ToDictionary(t => t.BenchmarkTypeId);
            TypesByCommitAndFullName = RunsById.Values
                .SelectMany(r => r.Types_.Select(type => (r.Commit, type)))
                .ToLookup(pair => (pair.Commit, pair.type.FullTypeName), pair => pair.type);
            BenchmarksById = TypesById.Values.SelectMany(t => t.Benchmarks).ToDictionary(b => b.BenchmarkId);
        }

        public async Task<CacheEntry> Refresh(CancellationToken cancellationToken)
        {
            var logger = repository.logger;
            var storage = repository.storage;
            var limit = repository.limit;
            CacheEntry previous = this;

            // We can just keep returning the current entry if we're asked not to do anything.
            if (limit == 0 && Environments.Count == 0 && RunsById.Count == 0)
            {
                return this;
            }

            // If we don't have previous work to finish off, check for new benchmarks.
            if (pendingContainersToBeDownloaded is null)
            {
                pendingContainerNames = await storage.ListFilesAsync(ContainerObjectsPrefix)
                    .Select(obj => obj.Name)
                    .OrderBy(name => name)
                    .Take(limit)
                    .ToListAsync(cancellationToken);
                var newContainerNames = pendingContainerNames.Except(previous.runsByStorageName.Keys).ToList();

                // If there are no new runs to load, use the previous set even if there are new environments.
                // An empty environment is fairly pointless... (and we don't mind too much if there are old ones that have been removed;
                // they'll go when there's next a new one.)
                if (!newContainerNames.Any())
                {
                    return previous;
                }
                logger.LogInformation("Loading {count} benchmarks.", newContainerNames.Count);

                // We won't reload everything from storage, but we'll come up with completely new set of objects each time, so we
                // don't need to worry about the changes involved as we reattach links.
                var environmentObject = await storage.GetObjectAsync(EnvironmentObjectName, cancellationToken);
                pendingEnvironments = environmentObject.Crc32c == previous.environmentCrc32c
                    ? previous.Environments.Select(env => { var clone = env.Clone(); clone.Runs.Clear(); return clone; }).ToList()
                    : (await LoadEnvironments(storage, cancellationToken)).OrderBy(e => e.Machine).ThenBy(e => e.TargetFramework).ThenBy(e => e.OperatingSystem).ToList();

                // We expect to always be able to get as far as this in one refresh.
                pendingContainersToBeDownloaded = newContainerNames;
                pendingCrc = environmentObject.Crc32c;
                pendingDownloadedContainers = new();
            }
            else
            {
                logger.LogInformation("Continuing to download benchmarks. {count} of {total} already downloaded.",
                    pendingDownloadedContainers!.Count(), pendingContainerNames!.Count);
            }

            // Download whatever we need, either from a previous run, or information we've just received.
            await DownloadOutstandingBenchmarks(cancellationToken);
            logger.LogInformation("Finished loading benchmarks");

            // At this point, we'll finish the work - we don't use the cancellation token after this.

            // Don't just use previous.runsByStorageName blindly - some may have been removed.
            var runsByStorageName = new Dictionary<string, BenchmarkRun>();
            foreach (var runStorageName in pendingContainerNames!)
            {
                // Everything in pendingContainerNames will either have been in the previous full state, or
                // in the set we've downloaded since the last "full" run.
                var runToAdd = previous.runsByStorageName.GetValueOrDefault(runStorageName) ?? pendingDownloadedContainers![runStorageName];
                
                var environment = pendingEnvironments!.FirstOrDefault(env => env.BenchmarkEnvironmentId == runToAdd.BenchmarkEnvironmentId);
                // If we don't have an environment, that's a bit worrying - skip and move on.
                if (environment is null)
                {
                    logger.LogInformation("Run {runName} has no environment. Skipping.", runStorageName);
                    continue;
                }
                runToAdd.Environment = environment;
                runToAdd.PopulateLinks();
                runsByStorageName[runStorageName] = runToAdd;
            }
            // Attach the runs to environments, in reverse chronological order
            foreach (var run in runsByStorageName.Values.OrderByDescending(r => r.Start.ToInstant()))
            {
                run.Environment.Runs.Add(run);
            }
            return new CacheEntry(repository, pendingEnvironments!, pendingCrc!, runsByStorageName);
        }

        private async Task DownloadOutstandingBenchmarks(CancellationToken cancellationToken)
        {
            var taskList = pendingContainersToBeDownloaded!
                .Take(DownloadConcurrency)
                .Select(DownloadRun)
                .ToList();

            while (taskList.Count > 0)
            {
                var task = await Task.WhenAny(taskList);
                taskList.Remove(task);
                var result = await task;
                pendingDownloadedContainers![result.name] = result.run;
                pendingContainersToBeDownloaded!.Remove(result.name);

                if (pendingContainersToBeDownloaded.Count > taskList.Count)
                {
                    // The first taskList.Count entries are the ones still being downloaded.
                    string next = pendingContainersToBeDownloaded[taskList.Count];
                    taskList.Add(DownloadRun(next));
                }
            }

            async Task<(string name, BenchmarkRun run)> DownloadRun(string name)
            {
                var run = await LoadContainerAsync(repository.storage, name, cancellationToken);
                return (name, run);
            }
        }

        private static async Task<BenchmarkRun> LoadContainerAsync(IStorageRepository storage, string runStorageName, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream();
            await storage.DownloadObjectAsync(runStorageName, stream, cancellationToken);
            stream.Position = 0;
            return BenchmarkRun.Parser.ParseFrom(stream);
        }

        private static async Task<IList<BenchmarkEnvironment>> LoadEnvironments(IStorageRepository storage, CancellationToken cancellationToken)
        {
            var environments = new List<BenchmarkEnvironment>();
            var stream = new MemoryStream();
            await storage.DownloadObjectAsync(EnvironmentObjectName, stream, cancellationToken);
            stream.Position = 0;
            while (stream.Position != stream.Length)
            {
                environments.Add(BenchmarkEnvironment.Parser.ParseDelimitedFrom(stream));
            }
            return environments;
        }
    }
}
