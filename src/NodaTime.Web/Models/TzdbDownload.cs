// Copyright 2017 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using NodaTime.Web.Services;
using NodaTime.TimeZones;

namespace NodaTime.Web.Models
{
    /// <summary>
    /// A download of TZDB data in the form of an NZD file.
    /// </summary>
    public class TzdbDownload
    {
        public string Name { get; }

        private readonly Lazy<(byte[] Data, string VersionId)> content;

        public TzdbDownload(IStorageRepository storage, string name)
        {
            Name = Path.GetFileName(name);
            content = new Lazy<(byte[], string)>(LoadContent, LazyThreadSafetyMode.ExecutionAndPublication);

            (byte[] Data, string VersionId) LoadContent()
            {
                var stream = new MemoryStream();
                storage.DownloadObject(name, stream);
                stream.Position = 0;
                var source = TzdbDateTimeZoneSource.FromStream(stream);
                return (stream.ToArray(), source.VersionId);
            }
        }

        public Stream GetContent() => new MemoryStream(content.Value.Data);

        public string VersionId => content.Value.VersionId;
    }
}
