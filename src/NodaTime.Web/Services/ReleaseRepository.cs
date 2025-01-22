// Copyright 2016 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using NodaTime.Web.Models;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NodaTime.Web.Services
{
    public class ReleaseRepository : IReleaseRepository
    {
        private static NullLogger nullLogger = new NullLogger();

        private static readonly Duration CacheRefreshTime = Duration.FromMinutes(6);
        private readonly TimerCache<CacheValue> cache;

        public ReleaseRepository(
            IHostApplicationLifetime lifetime,
            ILoggerFactory loggerFactory)
        {
            cache = new TimerCache<CacheValue>("releases", lifetime, CacheRefreshTime, FetchReleases, loggerFactory, FetchReleases());
            cache.Start();
        }

        public IReadOnlyList<StructuredVersion> AllReleases => cache.Value.Releases;
        public IReadOnlyList<string> CurrentMinorVersions => cache.Value.CurrentMinorVersions;
        public IReadOnlyList<string> OldMinorVersions => cache.Value.OldMinorVersions;
        public StructuredVersion LatestRelease => cache.Value.LatestRelease;

        private CacheValue FetchReleases()
        {
            return Task.Run(FetchReleasesAsync).Result;
        }

        private async Task<CacheValue> FetchReleasesAsync()
        {


            var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
            var packageFinder = await repository.GetResourceAsync<FindPackageByIdResource>();
            var cache = new SourceCacheContext { NoCache = true, RefreshMemoryCache = true };

            var allVersions = await packageFinder.GetAllVersionsAsync("NodaTime", cache, nullLogger, default);
            var releases = new List<StructuredVersion>();
            foreach (var version in allVersions.Select(v => v.ToNormalizedString()))
            {
                if (version.StartsWith("0."))
                {
                    continue;
                }
                releases.Add(new StructuredVersion(version));
            }
            
            return new CacheValue(releases);
        }

        private class CacheValue
        {
            public List<StructuredVersion> Releases { get; }
            public StructuredVersion LatestRelease { get; }
            public List<string> CurrentMinorVersions { get; }
            public List<string> OldMinorVersions { get; }

            public CacheValue(List<StructuredVersion> releases)
            {
                Releases = releases;
                // "Latest" is in terms of version, not release date. (So if
                // 1.4 comes out after 2.0, 2.0 is still latest.)
                // Pre-release versions are excluded.
                LatestRelease = releases
                    .Where(r => r.Prerelease == null)
                    .OrderByDescending(r => r)
                    .First();
                var allMinorReleasesGroupedByMajor = releases
                    .Where(r => r.Prerelease == null)
                    .Select(r => new { r.Major, r.Minor })
                    .Distinct()
                    .OrderByDescending(v => v.Major).ThenByDescending(v => v.Minor)
                    .GroupBy(v => v.Major);
                CurrentMinorVersions = allMinorReleasesGroupedByMajor.Select(g => g.First()).Select(v => $"{v.Major}.{v.Minor}.x").ToList();
                OldMinorVersions = allMinorReleasesGroupedByMajor.SelectMany(g => g.Skip(1)).Select(v => $"{v.Major}.{v.Minor}.x").ToList();
            }
        }
    }
}
