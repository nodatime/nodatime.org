﻿// Copyright 2016 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.
using Google;
using NodaTime.Web.Middleware;
using NodaTime.Web.Models;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace NodaTime.Web.Services;

public class TzdbRepository : IRefreshableCache
{
    private static readonly Regex PlausibleReleaseName = new Regex(@"^[-_a-zA-Z0-9.]+\.nzd$");
    private static readonly Duration CacheRefreshTime = Duration.FromMinutes(7);

    private readonly ILogger logger;
    private readonly IStorageRepository storage;

    // Keep this as a single replaceable field for all aspects.
    // This doesn't mean it won't change during the course of a request though.
    private CacheEntry currentEntry;

    public TzdbRepository(
        ILogger<TzdbRepository> logger,
        IStorageRepository storage)
    {
        this.storage = storage;
        this.logger = logger;
        currentEntry = new([]);
    }

    public IList<TzdbDownload> GetReleases() => currentEntry.Releases;

    public TzdbDownload? GetRelease(string name)
    {
        var entry = currentEntry;

        var releasesByName = entry.ReleasesByName;
        if (releasesByName.TryGetValue(name, out var value))
        {
            return value;
        }
        // We don't know about the requested release from the last cache refresh. See whether
        // it's *possibly* a real release, and if so try to fetch the object metadata.
        // If that succeeds, serve that (and remember it) until we next refresh the cache.
        if (!PlausibleReleaseName.IsMatch(name))
        {
            return null;
        }
        var onDemandCache = entry.OnDemandReleasesByName ?? new ConcurrentDictionary<string, TzdbDownload>();
        if (onDemandCache.TryGetValue(name, out value))
        {
            return value;
        }

        string objectName = $"tzdb/{name}";
        try
        {
            storage.GetObject(objectName);
        }
        catch (GoogleApiException)
        {
            // If we can't get the metadata entry for *any* reason, just return that it's not been found.
            return null;
        }
        value = new TzdbDownload(storage, objectName);
        onDemandCache[name] = value;
        return value;
    }

    public async Task Refresh(CancellationToken cancellationToken)
    {
        var oldReleasesByName = currentEntry?.ReleasesByName ?? new Dictionary<string, TzdbDownload>();
        var releases = await storage.ListFilesAsync("tzdb/")
                            .Where(o => o.Name.EndsWith(".nzd"))
                            .Select(obj => new TzdbDownload(storage, obj.Name))
                            .Select(r => oldReleasesByName.ContainsKey(r.Name) ? oldReleasesByName[r.Name] : r)
                            .OrderByDescending(r => r.Name, StringComparer.Ordinal)
                            .ToListAsync(cancellationToken);
        currentEntry = new CacheEntry(releases);
    }

    private class CacheEntry
    {
        /// <summary>
        /// Temporary cache for releases we are asked for but didn't previously know about.
        /// This can occur if a new release has just been published, but we haven't refreshed
        /// our list yet. We still want to serve the file, and cache it, but we're happy
        /// to wait until the main refresh before listing it directly.
        /// </summary>
        public ConcurrentDictionary<string, TzdbDownload> OnDemandReleasesByName { get; }

        public Dictionary<string, TzdbDownload> ReleasesByName { get; }
        public List<TzdbDownload> Releases { get; }
        public static CacheEntry Empty { get; } = new CacheEntry(new List<TzdbDownload>());

        public CacheEntry(List<TzdbDownload> releases)
        {
            Releases = releases;
            ReleasesByName = releases.ToDictionary(r => r.Name);
            OnDemandReleasesByName = new ConcurrentDictionary<string, TzdbDownload>();
        }
    }
}
