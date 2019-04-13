// Copyright 2016 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NodaTime.Text;
using NodaTime.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NodaTime.Web.Services
{
    public class ReleaseRepository : IReleaseRepository
    {
        private const string ObjectPrefix = "releases/";
        private static readonly Regex ReleasePattern = new Regex(ObjectPrefix + @"NodaTime-(\d+\.\d+\.\d+(?:-[a-z]+\d+)?)(?:-src)?.zip");
        private const string Sha256Key = "SHA-256";
        private const string ReleaseDateKey = "ReleaseDate";

        private static readonly Duration CacheRefreshTime = Duration.FromMinutes(6);
        private readonly IStorageRepository storage;
        private readonly TimerCache<CacheValue> cache;

        public ReleaseRepository(
            IApplicationLifetime lifetime,
            ILoggerFactory loggerFactory,
            IStorageRepository storage)
        {
            this.storage = storage;
            cache = new TimerCache<CacheValue>("releases", lifetime, CacheRefreshTime, FetchReleases, loggerFactory, FetchReleases());
            cache.Start();
        }

        public IReadOnlyList<ReleaseDownload> AllReleases => cache.Value.Releases;
        public IReadOnlyList<string> CurrentMinorVersions => cache.Value.CurrentMinorVersions;
        public IReadOnlyList<string> OldMinorVersions => cache.Value.OldMinorVersions;
        public ReleaseDownload LatestRelease => cache.Value.LatestRelease;

        private CacheValue FetchReleases()
        {
            var releases = storage
                .ListFiles(ObjectPrefix)
                .Where(obj => !obj.Name.EndsWith("/"))
                .Select(ConvertObject)
                .OrderByDescending(r => r.Version)
                .ToList();
            return new CacheValue(releases);
        }

        private ReleaseDownload ConvertObject(StorageFile file)
        {
            string? sha256Hash;
            file.Metadata.TryGetValue(Sha256Key, out sha256Hash);
            string? releaseDateMetadata;
            file.Metadata.TryGetValue(ReleaseDateKey, out releaseDateMetadata);
            var match = ReleasePattern.Match(file.Name);
            StructuredVersion? version = match.Success ? new StructuredVersion(match.Groups[1].Value) : null;
            LocalDate releaseDate = releaseDateMetadata == null
                ? LocalDate.FromDateTime(file.LastUpdated)
                : LocalDatePattern.Iso.Parse(releaseDateMetadata).Value;
            return new ReleaseDownload(version, file.Name.Substring(ObjectPrefix.Length),
                storage.GetDownloadUrl(file.Name), sha256Hash, releaseDate);
        }

        private class CacheValue
        {
            public List<ReleaseDownload> Releases { get; }
            public ReleaseDownload LatestRelease { get; }
            public List<string> CurrentMinorVersions { get; }
            public List<string> OldMinorVersions { get; }

            public CacheValue(List<ReleaseDownload> releases)
            {
                Releases = releases;
                // "Latest" is in terms of version, not release date. (So if
                // 1.4 comes out after 2.0, 2.0 is still latest.)
                // Pre-release versions are excluded.
                LatestRelease = releases
                    .Where(r => !r.File.Contains("-src") && r.Version?.Prerelease == null)
                    .OrderByDescending(r => r.Version)
                    .First();
                var allMinorReleasesGroupedByMajor = releases
                    .Where(r => !r.File.Contains("-src") && r.Version != null && r.Version.Prerelease == null)
                    .Select(r => new { r.Version!.Major, r.Version!.Minor })
                    .Distinct()
                    .OrderByDescending(v => v.Major).ThenByDescending(v => v.Minor)
                    .GroupBy(v => v.Major);
                CurrentMinorVersions = allMinorReleasesGroupedByMajor.Select(g => g.First()).Select(v => $"{v.Major}.{v.Minor}.x").ToList();
                OldMinorVersions = allMinorReleasesGroupedByMajor.SelectMany(g => g.Skip(1)).Select(v => $"{v.Major}.{v.Minor}.x").ToList();
            }
        }
    }
}
