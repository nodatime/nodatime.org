// Copyright 2025 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using System.Collections.Immutable;

namespace NodaTime.Web.Middleware;

public class CacheRefreshingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger logger;
    private readonly ImmutableList<ConfiguredRefreshableCache> caches;
    // Parallel collection (urgh) with caches.
    private readonly List<Instant> nextRefreshTimes;
    private readonly CacheRefreshingMiddlewareOptions options;
    private readonly IClock clock;

    private int active;

    public CacheRefreshingMiddleware(ILogger<CacheRefreshingMiddleware> logger, IClock clock, IServiceProvider provider, CacheRefreshingMiddlewareOptions options, RequestDelegate next)
    {
        this.next = next;
        this.logger = logger;
        caches = [.. options.GetConfiguredCaches(provider)];
        this.options = options;
        this.clock = clock;
        nextRefreshTimes = Enumerable.Repeat(Instant.MinValue, caches.Count).ToList();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Make sure we only only bother checking etc on the right path.
        if (context.Request.Path == options.RequestPath)
        {
            await MaybeRefresh();
        }
        await next(context);
    }

    private async Task MaybeRefresh()
    {
        if (Interlocked.CompareExchange(ref active, 1, 0) != 0)
        {
            return;
        }
        try
        {
            // Do we have *anything* to refresh?
            var now = clock.GetCurrentInstant();
            var nextRefresh = nextRefreshTimes.Min();
            if (now < nextRefresh)
            {
                return;
            }

            // What should we refresh?
            var index = nextRefreshTimes.IndexOf(nextRefresh);
            var cacheAndRefreshInterval = caches[index];
            var cache = cacheAndRefreshInterval.Cache;

            // Update state, and try to do the refresh.
            logger.LogDebug($"Refreshing {cache.GetType().Name}");
            using var cts = new CancellationTokenSource(options.RefreshTimeout.ToTimeSpan());
            try
            {
                await cache.Refresh(cts.Token);
                nextRefreshTimes[index] = now + cacheAndRefreshInterval.RefreshInterval;
                logger.LogDebug($"Refresh of {cache.GetType().Name} completed");
            }
            catch (OperationCanceledException ex)
            {
                nextRefreshTimes[index] = now + options.RefreshWhenIncompleteInterval;
                logger.LogDebug(ex, $"Refresh of {cache.GetType().Name} timed out (will continue next time)");
            }
            catch (Exception ex)
            {
                nextRefreshTimes[index] = now + options.RefreshWhenFailedInterval;
                logger.LogError(ex, $"Refresh of {cache.GetType().Name} failed");
            }
        }
        finally
        {
            Interlocked.Exchange(ref active, 0);
        }
    }
}
