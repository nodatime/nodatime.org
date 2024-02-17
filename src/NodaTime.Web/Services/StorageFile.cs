// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using System;
using System.Collections.Generic;

namespace NodaTime.Web.Services
{
    /// <summary>
    /// Abstraction of a file fetched from an <see cref="IStorageRepository"/>.
    /// </summary>
    public sealed class StorageFile
    {
        public string Name { get; }
        public IDictionary<string, string> Metadata { get; }
        public DateTimeOffset LastUpdated { get; }
        public string Crc32c { get; }

        public StorageFile(string name, IDictionary<string, string>? metadata, DateTimeOffset lastUpdated, string crc32c)
        {
            Name = name;
            Metadata = metadata ?? new Dictionary<string, string>();
            LastUpdated = lastUpdated;
            Crc32c = crc32c;
        }
    }
}
