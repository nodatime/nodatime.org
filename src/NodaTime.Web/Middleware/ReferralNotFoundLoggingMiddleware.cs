// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NodaTime.Web.Middleware
{
    public sealed class ReferralNotFoundLoggingMiddleware
    {
        // Avoid a memory leak through poisoned referrals
        private const int MaxEntries = 500;

        private readonly ConcurrentDictionary<(string, string), int> invalidReferrals;
        private readonly RequestDelegate next;
        private readonly ILogger logger;
        private int logEntries;

        public ReferralNotFoundLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            logger = loggerFactory.CreateLogger<ReferralNotFoundLoggingMiddleware>();
            invalidReferrals = new ConcurrentDictionary<(string, string), int>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await next(context);
            if (Interlocked.CompareExchange(ref logEntries, 0, 0) >= MaxEntries)
            {
                return;
            }
            if (!logger.IsEnabled(LogLevel.Information))
            {
                return;
            }
            if ((HttpStatusCode) context.Response.StatusCode != HttpStatusCode.NotFound)
            {
                return;
            }
            var path = context.Request.Path.ToString();
            // We see a lot of malware probing like this
            if (path.StartsWith("//"))
            {
                return;
            }
            var referer = context.Request.GetTypedHeaders().Referer?.ToString();
            if (referer is null)
            {
                return;
            }
            if (invalidReferrals.TryAdd((path, referer), 0))
            {
                Interlocked.Increment(ref logEntries);
                logger.LogInformation("{Path} referred to by {Referer} not found or redirected",
                    path, referer);
            }
        }
    }
}
