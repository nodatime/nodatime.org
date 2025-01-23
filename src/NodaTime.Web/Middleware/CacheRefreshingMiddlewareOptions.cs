// Copyright 2025 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

namespace NodaTime.Web.Middleware;

public class CacheRefreshingMiddlewareOptions
{
    /// <summary>
    /// How long each refresh attempt is allowed to take.
    /// </summary>
    public Duration RefreshTimeout { get; set; } = Duration.FromSeconds(4);

    /// <summary>
    /// How long to use as the refresh interval when the previous attempt timed out.
    /// </summary>
    public Duration RefreshWhenIncompleteInterval { get; set; } = Duration.FromSeconds(30);

    /// <summary>
    /// How long to use as the refresh interval when the previous attempt failed with an exception.
    /// </summary>
    public Duration RefreshWhenFailedInterval { get; set; } = Duration.FromMinutes(5);

    /// <summary>
    /// The path to use for a refresh - should be something non-latency-sensitive.
    /// </summary>
    public string RequestPath { get; set; } = "/health";

    private readonly List<(Type, Duration)> cacheTypes = new();

    public CacheRefreshingMiddlewareOptions Add<T>(Duration refreshInterval) where T : IRefreshableCache
    {
        cacheTypes.Add((typeof(T), refreshInterval));
        return this;
    }

    public IEnumerable<ConfiguredRefreshableCache> GetConfiguredCaches(IServiceProvider provider) =>
        cacheTypes.Select(ct => new ConfiguredRefreshableCache((IRefreshableCache) provider.GetRequiredService(ct.Item1), ct.Item2));    
}
