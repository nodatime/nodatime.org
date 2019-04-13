// Copyright 2019 The Noda Time Authors. All rights reserved.
// Use of this source code is governed by the Apache License 2.0,
// as found in the LICENSE.txt file.

using Newtonsoft.Json;
using NodaTime.Text;
using NodaTime.TimeZones;
using System.Collections.Generic;
using System.Linq;

namespace NodaTime.Web.ViewModels
{
    /// <summary>
    /// A release of IANA time zone information, in an easy to display (and consume) model.
    /// </summary>
    public class IanaRelease
    {
        private readonly TzdbDateTimeZoneSource source;

        [JsonProperty("IanaVersion")]
        public string IanaVersion => source.TzdbVersion;

        [JsonProperty("FullVersionId")]
        public string FullVersionId => source.VersionId;

        [JsonProperty("Zones")]
        public IEnumerable<Zone> Zones { get; }

        private IanaRelease(TzdbDateTimeZoneSource source)
        {
            this.source = source;
            var locations = source.ZoneLocations?.ToDictionary(location => location.ZoneId) ?? new Dictionary<string, TzdbZoneLocation>();
            Zones = source
                .GetIds()
                .Where(x => source.CanonicalIdMap[x] == x)
                .OrderBy(x => x)
                .Select(id => new Zone(source, locations, id))
                .ToList();
        }

        public static IanaRelease FromTzdbDateTimeZoneSource(TzdbDateTimeZoneSource source) =>
            new IanaRelease(source);

        public class Zone
        {
            private static readonly Instant StartOfModernEra = Instant.FromUtc(2000, 1, 1, 0, 0);
            private static readonly Instant EndOfModernEra = Instant.FromUtc(2040, 1, 1, 0, 0);

            [JsonProperty("Id")]
            public string Id { get; }

            [JsonProperty("Aliases")]
            public IEnumerable<string> Aliases { get; }

            [JsonProperty("Location")]
            public Location? Location { get; }

            [JsonProperty("Offsets")]
            public IEnumerable<string> Offsets { get; }

            internal Zone(TzdbDateTimeZoneSource source, Dictionary<string, TzdbZoneLocation> locations, string id)
            {
                Id = id;
                Aliases = source.Aliases[id];
                locations.TryGetValue(id, out var location);
                Location = location == null ? null : new Location(location);
                var zone = source.ForId(id);
                Offsets = zone.GetZoneIntervals(StartOfModernEra, EndOfModernEra)
                    .Select(zi => zi.WallOffset)
                    .Distinct()
                    .OrderBy(x => x)
                    .Select(o => OffsetPattern.GeneralInvariant.Format(o));
            }
        }

        public class Location
        {
            [JsonProperty("CountryCode")]
            public string CountryCode { get; }

            [JsonProperty("CountryName")]
            public string CountryName { get; }

            [JsonProperty("Comment")]
            public string Comment { get; }

            [JsonProperty("Latitude")]
            public double Latitude { get; }

            [JsonProperty("Longitude")]
            public double Longitude { get; }

            internal Location(TzdbZoneLocation location)
            {
                CountryCode = location.CountryCode;
                CountryName = location.CountryName;
                Comment = location.Comment;
                Latitude = location.Latitude;
                Longitude = location.Longitude;
            }
        }
    }
}
