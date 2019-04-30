// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Microsoft.Extensions.Logging;

namespace NodaTime.Web.Logging
{
    /// <summary>
    /// Prototype provider for a console logger writing JSON straight to the console.
    /// </summary>
    [ProviderAlias("JsonConsole")]
    public class JsonConsoleLoggerProvider : ILoggerProvider
    {
        public JsonConsoleLoggerProvider()
        {
        }

        public ILogger CreateLogger(string categoryName) => new JsonConsoleLogger(categoryName);

        public void Dispose()
        {
            // No-op
        }
    }
}
