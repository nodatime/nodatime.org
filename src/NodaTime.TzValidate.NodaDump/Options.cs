// Copyright 2015 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

namespace NodaTime.TzValidate.NodaDump
{
    /// <summary>
    /// Options for TzValidate dumpers.
    /// Note: this is a copy of the code in the NodaTime repo, as it's not published as a NuGet package but we use it in the web site.
    /// </summary>
    public sealed class Options
    {
        public int? FromYear { get; set; }
        public int ToYear { get; set; }
        public string? Source { get; set; }
        public string? ZoneId { get; set; }
        public string? OutputFile { get; set; }
        public bool HashOnly { get; set; }
        public bool DisableAbbreviations { get; set; }
        public bool WallChangeOnly { get; set; }
        internal Instant Start => FromYear is null ? Instant.MinValue : Instant.FromUtc(FromYear.Value, 1, 1, 0, 0);
        internal Instant End => Instant.FromUtc(ToYear, 1, 1, 0, 0);
    }
}
