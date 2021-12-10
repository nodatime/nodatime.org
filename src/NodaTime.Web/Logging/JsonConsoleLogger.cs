// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NodaTime.Web.Logging
{
    // Other potential features:
    // - Configurable labels
    // - Event ID
    // - Scopes

    /// <summary>
    /// Prototype logger that writes JSON straight to the console in a way that Stackdriver is able to interpret pleasantly.
    /// </summary>
    public class JsonConsoleLogger : ILogger
    {
        private readonly string categoryName;

        public JsonConsoleLogger(string categoryName)
        {
            this.categoryName = categoryName;
        }

        // We don't really support scopes
        public IDisposable BeginScope<TState>(TState state) => SingletonDisposable.Instance;

        // Note: log level filtering is handled by other logging infrastructure, so we don't do any of it here.
        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string severity = logLevel switch
            {
                LogLevel.Trace => "DEBUG",
                LogLevel.Debug => "DEBUG",
                LogLevel.Information => "INFO",
                LogLevel.Warning => "WARNING",
                LogLevel.Error => "ERROR",
                LogLevel.Critical => "CRITICAL",
                _ => "UNKNOWN"
            };

            string message = formatter(state, exception);
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            StringBuilder builder = new StringBuilder();
            JsonWriter writer = new JsonTextWriter(new StringWriter(builder));
            writer.WriteStartObject();
            writer.WritePropertyName("message");
            writer.WriteValue(message);
            writer.WritePropertyName("log_name");
            writer.WriteValue(categoryName);
            if (exception != null)
            {
                writer.WritePropertyName("exception");
                writer.WriteValue(exception.ToString());
            }
            writer.WritePropertyName("severity");
            writer.WriteValue(severity);

            // If we have format params and its more than just the original message add them.
            if (state is IEnumerable<KeyValuePair<string, object>> formatParams &&
                ContainsFormatParameters(formatParams))
            {
                writer.WritePropertyName("format_parameters");
                writer.WriteStartObject();
                foreach (var pair in formatParams)
                {
                    string key = pair.Key;
                    if (string.IsNullOrEmpty(key))
                    {
                        continue;
                    }
                    if (char.IsDigit(key[0]))
                    {
                        key = "_" + key;
                    }
                    writer.WritePropertyName(key);
                    writer.WriteValue(pair.Value?.ToString() ?? "");
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
            Console.WriteLine(builder);

            // Checks that fields is:
            // - Non-empty
            // - Not just a single entry with a key of "{OriginalFormat}"
            // so we can decide whether or not to populate a struct with it.
            bool ContainsFormatParameters(IEnumerable<KeyValuePair<string, object>> fields)
            {
                using (var iterator = fields.GetEnumerator())
                {
                    // No fields? Nothing to format.
                    if (!iterator.MoveNext())
                    {
                        return false;
                    }
                    // If the first entry isn't the original format, we definitely want to create a struct
                    if (iterator.Current.Key != "{OriginalFormat}")
                    {
                        return true;
                    }
                    // If the first entry *is* the original format, we want to create a struct
                    // if and only if there's at least one more entry.
                    return iterator.MoveNext();
                }
            }
        }

        // Used for scope handling.
        private class SingletonDisposable : IDisposable
        {
            internal static readonly SingletonDisposable Instance = new SingletonDisposable();
            private SingletonDisposable() { }
            public void Dispose() { }
        }
    }
}
