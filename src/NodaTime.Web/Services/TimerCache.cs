// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;

namespace NodaTime.Web.Services
{
    public class TimerCache<T>
    {
        private readonly Timer timer;
        private readonly Func<T> provider;
        private readonly ILogger logger;
        private readonly object padlock = new object();
        private readonly Duration refreshPeriod;
        private readonly string cacheName;
        private T value;
        // TODO: Use Interlocked or similar to ensure proper volatility.
        // It's possible that using volatile would be enough...
        public T Value
        {
            get
            {
                lock (padlock)
                {
                    return value;
                }
            }
            set
            {
                lock (padlock)
                {
                    this.value = value;
                }
            }
        }

        public TimerCache(string cacheName, IHostApplicationLifetime lifetime, Duration refreshPeriod, Func<T> provider, ILoggerFactory loggerFactory,
            T initialValue)
        {
            this.cacheName = cacheName;
            lifetime.ApplicationStopping.Register(() => timer?.Dispose());
            logger = loggerFactory.CreateLogger(typeof(TimerCache<T>));
            this.provider = provider;
            // Don't start yet; wait for Start() to be called.
            timer = new Timer(Fetch, state: null, -1, Timeout.Infinite);
            this.refreshPeriod = refreshPeriod;
            value = initialValue;
        }

        public void Start() => timer.Change(TimeSpan.Zero, refreshPeriod.ToTimeSpan());

        private void Fetch(object? state)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                logger.LogInformation($"Refreshing cache for {cacheName}");
                Value = provider();
                logger.LogInformation("Cache refresh complete for {cacheName} in {durationMs}ms", cacheName, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error fetching {cacheName} after {durationMs}ms", cacheName, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
