// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using NodaTime.Web.Services;
using System;
using System.IO;
using System.Threading;

namespace NodaTime.Web.Models
{
    /// <summary>
    /// A download of TZDB data in the form of an NZD file.
    /// </summary>
    public class TzdbDownload
    {
        public string Name { get; }
        public string NodaTimeOrgUrl { get; }

        private readonly Lazy<byte[]> data;

        public TzdbDownload(IStorageRepository storage, string name)
        {
            Name = Path.GetFileName(name);
            NodaTimeOrgUrl = $"https://nodatime.org/tzdb/{Name}";
            data = new Lazy<byte[]>(LoadContent, LazyThreadSafetyMode.ExecutionAndPublication);

            byte[] LoadContent()
            {
                var stream = new MemoryStream();
                storage.DownloadObject(name, stream);
                return stream.ToArray();
            }
        }

        public Stream GetContent() => new MemoryStream(data.Value);
    }
}
