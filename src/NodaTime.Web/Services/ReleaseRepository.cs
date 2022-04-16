// Copyright 2016 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime.Text;
using NodaTime.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NodaTime.Web.Services
{
    public class ReleaseRepository : IReleaseRepository
    {
        private static readonly JsonSerializerSettings jsonParseSettings = new JsonSerializerSettings { DateParseHandling = DateParseHandling.None };

        private const string ObjectPrefix = "releases/";

        private static readonly Duration CacheRefreshTime = Duration.FromMinutes(6);
        private readonly TimerCache<CacheValue> cache;
        private readonly IHttpClientFactory httpClientFactory;

        public ReleaseRepository(
            IHostApplicationLifetime lifetime,
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            cache = new TimerCache<CacheValue>("releases", lifetime, CacheRefreshTime, FetchReleases, loggerFactory, FetchReleases());
            cache.Start();
        }

        public IReadOnlyList<ReleaseDownload> AllReleases => cache.Value.Releases;
        public IReadOnlyList<string> CurrentMinorVersions => cache.Value.CurrentMinorVersions;
        public IReadOnlyList<string> OldMinorVersions => cache.Value.OldMinorVersions;
        public ReleaseDownload LatestRelease => cache.Value.LatestRelease;

        private CacheValue FetchReleases()
        {
            return Task.Run(FetchReleasesAsync).Result;
        }

        private async Task<CacheValue> FetchReleasesAsync()
        {
            using (var client = httpClientFactory.CreateClient())
            {
                var json = await client.GetStringAsync("https://azuresearch-usnc.nuget.org/query?q=PackageId:NodaTime");
                var jobject = JsonConvert.DeserializeObject<JObject>(json, jsonParseSettings)!;
                var versions = jobject["data"]![0]!["versions"]!.ToList();

                var releases = new List<ReleaseDownload>();
                foreach (var versionObject in versions)
                {
                    string url = (string) versionObject["@id"]!;
                    string version = (string)versionObject["version"]!;

                    // Skip anything before 1.0
                    if (version.StartsWith("0."))
                    {
                        continue;
                    }
                    releases.Add(new ReleaseDownload(new StructuredVersion(version), url));
                }
                return new CacheValue(releases);
            }
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
                    .Where(r => r.Version.Prerelease == null)
                    .OrderByDescending(r => r.Version)
                    .First();
                var allMinorReleasesGroupedByMajor = releases
                    .Where(r => r.Version.Prerelease == null)
                    .Select(r => new { r.Version.Major, r.Version.Minor })
                    .Distinct()
                    .OrderByDescending(v => v.Major).ThenByDescending(v => v.Minor)
                    .GroupBy(v => v.Major);
                CurrentMinorVersions = allMinorReleasesGroupedByMajor.Select(g => g.First()).Select(v => $"{v.Major}.{v.Minor}.x").ToList();
                OldMinorVersions = allMinorReleasesGroupedByMajor.SelectMany(g => g.Skip(1)).Select(v => $"{v.Major}.{v.Minor}.x").ToList();
            }
        }
    }
}
