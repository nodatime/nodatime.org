// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

namespace NodaTime.Web.Middleware;

/// <summary>
/// Extension methods to configure the middleware in this namespace.
/// </summary>
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseReferralNotFoundLogging(this IApplicationBuilder builder) =>
        builder.UseMiddleware<ReferralNotFoundLoggingMiddleware>();

    public static IApplicationBuilder UseCacheRefreshingMiddleware(this IApplicationBuilder builder, Action<CacheRefreshingMiddlewareOptions>? optionsConfiguration = null)
    {
        var options = new CacheRefreshingMiddlewareOptions();
        optionsConfiguration?.Invoke(options);
        builder.UseMiddleware<CacheRefreshingMiddleware>(options);
        return builder;
    }
}
