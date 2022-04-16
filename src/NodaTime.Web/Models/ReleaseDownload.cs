// Copyright 2016 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

namespace NodaTime.Web.Models
{
    public class ReleaseDownload
    {
        public StructuredVersion Version { get; }
        /// <summary>
        /// URL we could use to fetch the publication date if we really want that.
        /// </summary>
        public string NuGetMetadataUrl { get; set; }

        public ReleaseDownload(StructuredVersion version, string nuGetMetadataUrl)
        {
            Version = version;
            NuGetMetadataUrl = nuGetMetadataUrl;
        }
    }
}
