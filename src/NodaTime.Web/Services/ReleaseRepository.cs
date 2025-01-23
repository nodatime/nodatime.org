// Copyright 2016 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using NodaTime.Web.Middleware;
using NodaTime.Web.Models;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;

namespace NodaTime.Web.Services;

public class ReleaseRepository : IRefreshableCache
{
    private static NullLogger nullLogger = new NullLogger();

    public ReleaseRepository()
    {
        AllReleases = [];
        CurrentMinorVersions = [];
        OldMinorVersions = [];
        LatestRelease = new("0.0.0");
    }

    public IReadOnlyList<StructuredVersion> AllReleases { get; private set; }
    public IReadOnlyList<string> CurrentMinorVersions { get; private set; }
    public IReadOnlyList<string> OldMinorVersions { get; private set; }
    public StructuredVersion LatestRelease { get; private set; }

    public async Task Refresh(CancellationToken cancellationToken)
    {
        var repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
        var packageFinder = await repository.GetResourceAsync<FindPackageByIdResource>();
        var cache = new SourceCacheContext { NoCache = true, RefreshMemoryCache = true };

        var allVersions = await packageFinder.GetAllVersionsAsync("NodaTime", cache, nullLogger, cancellationToken);
        var releases = new List<StructuredVersion>();
        foreach (var version in allVersions.Select(v => v.ToNormalizedString()))
        {
            if (version.StartsWith("0."))
            {
                continue;
            }
            releases.Add(new StructuredVersion(version));
        }

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
