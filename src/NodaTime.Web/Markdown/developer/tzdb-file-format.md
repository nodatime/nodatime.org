@Title="Time zone database file format"

This document describes the Noda Time-specific binary file format that is
produced by Noda Time's `NodaTime.TzdbCompiler` tool. This tool reads the
text files from the [tz database](https://www.iana.org/time-zones) along with
some additional input from CLDR; it produces output in one of two different
formats:

   1. a .NET resource file containing a number of different resources
      (including, for example, one for each time zone), serialized as either
      an XML (`.resx`) or compiled (`.resources`) file. This is the only
      format accepted by Noda Time 1.0.x. The intention is to remove support
	  for this format in Noda Time 2.0.

   2. a binary file format (the "NodaZoneData" format) that does not depend
      upon a .NET resource file acting as a container. This is the default
      format in Noda Time 1.1 and later, and is the only format supported by
      the Portable Class Library versions of Noda Time.

The formats have some similarities, but can be treated independently. In
particular, the resource format supported by Noda Time 1.0.x is effectively
frozen, while the NodaZoneData format will evolve as more data is required.

# Version compatibility

Most users will just use the version of the database embedded in the Noda
Time assembly (which is typically produced with the same version of
`NodaTime.TzdbCompiler` as the Noda Time assembly itself), and so do not need to
worry about these details, but as we provide the ability to use a custom
compiled time zone database, we document here the compatibility rules
between the output produced by different versions of `ZoneInfoCompiler`/`NodaTime.TzdbCompiler` and
the Noda Time assembly.

In general, these are probably as you would expect: any version of Noda Time
must be able to read a compiled time zone database consumed by an earlier
version of Noda Time with the same major version number; the reverse,
however, is not true.

For example, Noda Time 1.1.x should be able to read the compiled output used
by Noda Time 1.0.x and 1.1.x, but would not necessarily be able to read the
output used by version 1.2.x. Noda Time 2.x does not necessarily need to be
able to read anything consumed by Noda Time 1.x.

(The same is true for the PCL version of Noda Time 1.x, though it is considered
a separate development line for the purposes of compatibility, and so does
not need to support the same features as the earlier non-PCL versions.)

Note that even though Noda Time must _read_ earlier versions of the compiled
database, it does not necessarily need to provide the same level of
functionality when doing so as it might when reading a current version. In
particular, only calls to methods supported by that earlier version of Noda
Time are guaranteed to work; any calls to methods introduced in a later
version may require a database compiled by at least that version.

The former is true because semantic versioning requires that we be able to
drop a later version of the Noda Time assembly into a running environment
without recompiling (or without replacing the compiled time zone database,
which might e.g. be embedded in the main executable); the latter statement
is true because it is unreasonable to expect that new functionality should
work without the new data it may require.

As a consequence, it is reasonable to act as if any functionality introduced
in a given version of Noda Time comes with an implicit caveat that it
requires a database compiled against at least that version to work.

# NodaZoneData format

Compiling the input data using the (default) `NodaZoneData` option produces
a compiled database in the "NodaZoneData" format, with an extension of ".nzd".
This is intended as the long-term format, and is designed with more extensibility
in mind than the resource file format.

## Overall structure

The first four bytes of the file contain the format version number. At the time of
this writing, the only supported version number is 0. Not all changes in the future
will require a change in version number (as even within a version the format is extensible)
but this provides flexibility in the future, where a single binary may be able to read a
range of versions. At that point, this document will be extended to give details for each version
separately. Each version may be completely independent of previous versions, so may redefine
the meaning of particular field IDs, or even contain a different field structure.

After that, the file is simply a sequence of fields. Each field consists of:

- A field ID (1 byte)
- The size of the field data, as a `count` (see details below)
- The field data

Fields are always stored in ascending order of ID (so later fields may depend on earlier ones).
The expected data within each field is governed by its ID.

## Serialization primitives

The "NodaZoneData" format makes use of the following serialization primitives:

- `byte`, a general 8-bit unsigned integer, simply serialized as a single byte.
- `fixed16`, `fixed32`, and `fixed64`, general 16-bit, 32-bit, and 64-bit signed integers,
  simply serialized as 2-, 4-, or 8-byte twos-complement values of the appropriate length,
  in MSB-first order. For example, `0x1234` would be serialized as the bytes `[0x12 0x34]` using
  the `fixed16` serialization, while -1 would be serialized as `[0xff 0xff]`
- `count`, a non-negative (typically small) `int32` value, serialized as a variably-sized integer:
  the value is written 7 bits at a time (least significant first), with the highest bit of each
  byte set if this is not the final byte of the value. For example, the value `0x16a` would be
  serialized as the bytes `[0xea 0x02]`.
- `signed count`, a potentially-negative (typically small in magnitude) `int32` value, stored
  in a "ZigZag" encoding: first mapping the signed number to an unsigned number, alternating
  between signed and unsigned values: 0 maps to 0, -1 maps to 1, 1 maps 2, -2 maps to 3, 2 maps
  to 4 etc. The result is then written in the same 7-bit encoding as `count`.
- `string`, a UTF-8 encoded text string or entry into a string pool. Each field either has a
  string pool associated with it or it doesn't. In a field with a string pool, strings are simply
  stored using the `count` encoding to specify which entry in the string pool is used. In a field
  without a string pool, the UTF-8-encoded size of the string is written using the `count` encoding,
  then the UTF-8 encoded bytes themselves are written.
- `offset`, an `int32` value representing a millisecond offset from UTC, with maximum and minimum
  values of +/-24 hours (exclusive on both sides). The offset is first made positive by simply adding
  24 hours, and then the resulting integer is represented in one of four ways, based on how neatly
  the offset divides into a whole number of larger units. In each case where the representation is
  more than one byte, the data is written with the most significant bits first. The representations are:
  - A single byte with a top bit of 0, indicating the number of half hours of the offset. (This is
    by far the most common form.)
  - Two bytes with the top three bits of the first byte set to "100" and the remaining 13 bits
    indicating the number of minutes in the offset.
  - Three bytes with the top three bits of the first byte set to "101" and the remaining 21 bits
    indicating the number of seconds in the offset.
  - Four bytes with the top three bits of the first byte set to "110" and the remaining 29 bits
    indicating the number of milliseconds in the offset.
- `zoneintervaltransition`, an encoding for an instant possibly relative to an earlier or equal one. This
  is used when writing transitions between zone intervals, where the additional context of a previous value
  helps to reduce the data required. The format is (in order of preference):
  - For `Instant.MinValue`, a single byte `0`.
  - For `Instant.MaxValue`, a single byte `1`.
  - For an instant which is a whole number of hours since the "previous" instant, and
    that number of hours is in the range \[128, 1048576), the format is that number of hours written as a `count`.
  - For an instant which is a whole number of minutes since 1800-01-01T00:00:00Z, and that number of
    minutes is in the range \[1048576, Int32.MaxValue\], the format is that number of minutes written as a `count`.
  - For any other instant, a single byte `2` followed by a `fixed64` value for the number of ticks since the Unix epoch.
- `dictionary`, a string-to-string dictionary where no keys or values can be null. This is simply a
  `count` number of entries followed by each key/value pair as a `string` key and `string` value.

## Fields

## Field 0: String pool

The string pool field is built *without* reference to a string pool (as otherwise it would be pointless).
It is simply a `count` followed by that many strings. The ordering of the strings is optimized (by running through
a lot of the serialization twice) so that the most commonly used strings come earliest. This allows those strings
to be represented very efficiently by later fields which use the string pool.

There must be exactly one string pool field.

## Field 1: Time zone

The time zone field provides full details of one of the "canonical" time zones in TZDB. (Time zone aliases always
eventually refer to a canonical time zone for their data.)

The format depends on the type of time zone. At the time of this writing, there are only two options, described below.
In both cases, the format starts with the zone ID (as a string) and then a 1-byte flag to indicate the type.

Fixed time zones (which have a single [`ZoneInterval`](noda-type://NodaTime.TimeZones.ZoneInterval) covering
  the whole of time). The flag value for a fixed zone is `1`. This is followed by the offset, and as of time zone data
  generated under Noda Time 2.x, also the name for the zone interval. Noda Time 1.x code will ignore the name part,
  and always use the time zone ID as the name for the interval. Noda Time 2.x code will read the name if it exists,
  but default to the ID if the name data isn't present.

Everything else is represented as a `PrecalculatedDateTimeZone` - a number of abutting `ZoneIntervals` from the start of
time until either the end of time, or some stable period where a pair of rules governing when daylight saving time starts
and stops continues until the end of time. This final stable period is the "tail zone" - and is optional, as a zone which
ends in a fixed offset can simply represent that as a final zone interval to the end of time.

The flag value for a precalculated zone is `2`, and the remaining data is:

- The number of precalculated zone intervals as a `count`
- The zone intervals themselves, each of which has a format of:
  - A `zoneintervaltransition` from the start of the previous interval (except for the first interval, which has no "previous")
    to the start of the current one.
  - The `string` name of the interval
  - The wall `offset` (i.e. standard + daylight savings)
  - The daylight savings `offset`
- A `zoneintervaltransition` from the start of the final zone interval above to
  the end of the final interval (which is the start of the tail zone, if there
  is one, or `Instant.MaxValue` otherwise).
- A `byte` to indicate the presence (1) or absence (0) of a tail zone
- The tail zone, if the previous flag indicated that one was present, as:
  - The standard `offset`
  - The `string` name used for intervals in "standard time"
  - The standard recurrence rule (see below)
  - The `string` name used for intervals in "daylight saving time"
  - The daylight savings recurrence rule
  - The daylight savings `offset` (to be added to the standard offset)

A recurrence rule provides data about when a transition occurs. For example, "The first Sunday in October at 1am local time." The format
is:

- A single byte representing four flags:
  - Bits 5-6: the "mode" of the rule, indicating whether transitions are specified relative to:
     - 0: UTC
     - 1: Wall time (local time)
     - 2: Standard time
  - Bits 2-4: the day of the week in which the recurrence starts,
    where 0 means "not set", and 1 to 7 mean Monday to Sunday respectively.
  - Bit 1: whether the day of month represents an upper bound (0) or lower bound (1);
    this is only relevant if a day of week is also specified, where the rule might be
	described as e.g. "first Tuesday on or after the 15th"
  - Bit 0: whether an extra day should be added to the calculation (1) or not (0), to
    cope with transitions documented as occurring at 24:00 - these may potentially
	spill into a following month
- A `count` (single byte) for the month-of-year in which the rule starts
- A `signed count` (single byte) for the day-of-month in which the rule starts, where a negative
  value indicates the number of days counted from the end of a month. (-1 is the last day of the
  month, for example.)
- An `offset` used (somewhat unconventionally) to indicate the time of day at which the recurrence starts.

This field may occur multiple times, and it always uses the string pool.

## Field 2: TZDB version

This is simply a single `string` representing the version of TZDB being serialized, such as "2013a".

This field must occur exactly once, and it does not use the string pool.

## Field 3: ID map

This is a map from alias to canonical ID. So for example, "Europe/Guernsey" maps to "Europe/London". The
representation is a single `dictionary`.

This field must occur exactly once, and it uses the string pool.

## Field 4: CLDR mapping between Windows system time zone IDs and TZDB IDs

This field contains the data from the windowsZones.xml file in [CLDR](http://cldr.unicode.org/). The format is:

- A `string` for the mapping version number (currently a string representation of the revision number)
- A `string` for the TZDB version the mapping was generated against
- A `string` for the Windows time zone version the mapping was generated against
- A `count` for the number of "map zones" to follow
- The map zone data, each of which is:
  - A `string` for the Windows system time zone ID
  - A `string` for the territory ID
  - A `count` for the number of TZDB IDs in this mapping
  - The TZDB IDs, each as a `string`

This field must occur exactly once, and it uses the string pool.

## Field 5: Additional information for Windows mapping (obsolete)

This field is only present for the sake of the PCL (1.x) versions, and is not loaded in 2.x.
It contains a mapping from Windows `TimeZoneInfo` standard name (in the en-US
locale) to TZDB ID for those time zones
where the two differ. (There are only a few.) This is required as the PCL doesn't expose the
`TimeZoneInfo.Id` property.

The field data is a single `dictionary`. It must occur exactly once, and it uses the string pool.

## Field 6: Zone location information

This field provides the information in the `iso3166.tab` and `zone.tab` files within TZDB, which
contain geographical information about time zones. Applications can use this to offer users a choice
of time zones in a friendlier way than using the TZDB ID... although the information is only provided
in English.

The field data consists of a `count` of elements, then the elements themselves. Each element consists of:

- A `signed count` for the latitude of the sample location, as an integer number of seconds.
- A `signed count` for the longitude of the sample location, as an integer number of seconds.
- A `string` for the name of the country.
- A `string` for the ISO-3166 two-letter country code.
- A (possibly empty) `string` for the comment associated with the location (usually used to disambiguate between
  locations in the same country).

## Field 7: "Zone1970" locations

This field provides the information in the `iso3166.tab` and `zone1970.tab` files within TZDB.

The field data consists of a `count` of elements, then the elements themselves. Each element consists of:

- A `signed count` for the latitude of the sample location, as an integer number of seconds.
- A `signed count` for the longitude of the sample location, as an integer number of seconds.
- A `signed count` for the number of countries.
- For each country:
  - A `string` for the name of the country.
  - A `string` for the ISO-3166 two-letter country code.
- A `string` for the zone ID.
- A (possibly empty) `string` for the comment associated with the location (usually used to disambiguate between
  locations in the same country).

# Resource file format

Compiling the input data using the `Resource` or `ResX` options produces a
compiled database in the resource file format native to Noda Time 1.0.x.
This is represented as a .NET resource file containing many resources: one
resource for each time zone, and four additional meta-information resources.

The four meta-information resources, and their resource keys, are:

   * `--meta-VersionId`, a `String` resource containing the tzdb version ID
     (e.g. "2012i")
   * `--meta-IdMap`, a `byte[]` resource containing a serialized
     `dictionary` (see below for the definition of these types), mapping all
     known time zone IDs (aliases and non-aliases) to a non-alias time zone
     ID
   * `--meta-WindowsToPosix`, a `byte[]` resource containing a serialized
     `dictionary`, mapping Windows time zone names to the corresponding tzdb
     time zone ID
   * `--meta-WindowsToPosixVersion`, a `String` resource containing the
     revision number of the Windows mapping data (e.g. "7825")

Each non-alias time zone is represented by a `byte[]` resource containing a
serialized `timezone`. The name of the resource is derived from the time zone
ID, with (theoretically) some substitutions for invalid characters.  (The
substitutions are not documented here, since they do not actually appear to be
used in practice; for example, the name of the Europe/London resource is simply
"Europe/London", and the same appears to be true for all other time zones as of
the 2012i tzdb).

All of these resources may (theoretically) contain extra appended data not
mentioned below; readers do not confirm that the end of the resource was
reached when all the data they know how to read has been consumed.

## Serialization primitives

The resource file format uses the following serialization primitives:

   * `byte`, a general 8-bit unsigned integer
   * `fixed16`, `fixed32`, and `fixed64`, general 16-bit, 32-bit, and 64-bit
     signed integers
   * `count`, a non-negative (typically small) `int32` value
   * `offset`, an `int32` value representing a millisecond offset from UTC
   * `string`, a UTF-8 encoded text string
   * `dictionary`, a dictionary with string keys and values
   * `timezone`, a serialized time zone (with several variants and subtypes,
     documented below)

Of these, only `dictionary` and `timezone` are used directly; the others are
used as part of the definitions of these types, below.

### byte

A `byte` is simply serialized as a single byte representing a value in the
range 0..255.

### fixed16, fixed32, fixed64

`fixed16`, `fixed32`, and `fixed64` are simply serialized as 2-, 4-, or
8-byte twos-complement values of the appropriate length, in MSB-first order.
For example, `0x1234` would be serialized as the bytes `[0x12 0x34]` using
the `fixed16` serialization, while -1 would be serialized as `[0xff 0xff]`.

### count

A `count` encodes a non-negative (typically small) `int32` value; it is
serialized as a variable number of bytes according to the following
scheme:

   * values `0x00`..`0x8e` are serialized as a single byte; values to `0x0e`
     are serialized as `0xf0`..`0xfe`, while `0x0f`..`0x8e` is serialized as
     `0x00`..`0x7f`
   * `0x8f`..`0x408e` is serialized as two bytes, by subtracting `0x8f` and
     then encoding the resulting (`0`&ndash;`0x3fff`) value using the byte
     `0x80`..`0xbf` for the MSB, followed by the LSB
   * `0x408f`..`0x20408e` is serialized as three bytes, by subtracting
     `0x408f` and then encoding the resulting (`0`&ndash;`0x1fffff`) value
     as the byte `0xc0`..`0xdf` for the MSB, followed by the next two bytes
     in MSB-first order (i.e. as a `fixed16`)
   * `0x20408f`..`0x1020408e` is serialized as four bytes, by subtracting
     `0x20408f` (giving `0`&ndash;`0x0fffffff`), adding `0xe0000000`,
     and then encoding the resulting (`0xe0000000`&ndash;`0xefffffff`) value
     as a `fixed32`
   * larger values are serialized as five bytes, as the leader `0xff`
     followed by the value as a `fixed32`

### offset

An `offset` encodes an `int32` value representing an offset in milliseconds
from UTC. While the encoding below can in theory represent any `int32`
value, the use in Noda Time is solely for values in the range (-24 hours,
+24 hours); commonly for 'round' numbers of minutes, etc.

An `offset` is serialized as a variable-length field as follows:

   * offsets that are a multiple of 30 minutes are serialized as the single
     byte `0x10`..`0x6e` counting the number of 30 minute periods, with a
     bias such that `0x10` represents an offset of -23:30, `0x11` represents
     -23:00, `0x3f` represents a zero offset, and `0x6e` represents the
     offset +23:30
   * offsets that are a whole number of seconds are serialized as three
     bytes, the first of which is in the range `0x82`..`0x85`, by adding
     `0x83ffff` to the number of seconds, and writing three bytes in
     MSB-first order, so that `[0x82 0xae 0x80]` represents -23:59:59,
     `[0x84 0x00 0x00]` represents +00:00:01, and `[0x85 0x51 0x7e]`
     represents +23:59:59
   * any other offset is serialized as the leader `0xfd` followed by the number
     of milliseconds as a `fixed32`

(The serialization method used additionally defines an encoding for
'out-of-range' offsets, but these cannot occur in practice, as the
underlying C# data type restricts the range of the offset to that described
above.)

### string

A `string` is serialized as a `count` of the number of UTF-8 bytes in the
string followed by those bytes.

### dictionary

A `dictionary` is serialized as a `count` number of entries in the
dictionary, followed by that number of pairs of `string` key and `string`
value.

### timezone

A `timezone` encodes a complete definition for a single time zone, excluding
the ID; it is represented in one of a few different forms depending on the
type of the time zone. The serialized format may be any of the following:

   * `0x00` followed by another `timezone`, indicating that the following
     time zone (in practice, always one of type `0x04`) should have
     information about zone transitions cached at runtime if possible
   * `0x01` followed by a `daylightsavingstimezone`, representing a time
     zone with a standard and DST offset, and simple recurrence rules for
     each transition
   * `0x02` followed by an `offset`, representing a time zone with a fixed
     offset (and no DST); this is used natively when encoding time zones
     with no DST (such as "Etc/UTC") and as part of the composite
     "pre-calculated" time zone
   * `0x03`, representing 'no time zone' (only used when encoding an absent
     'tail zone' for a pre-calculated zone)
   * `0x04` followed by a `precalculatedtimezone`, representing a zone that
     expands all known transitions explicitly, up to an optional 'tail zone'
     representing a final (possibly DST-using) stable state

#### daylightsavingstimezone

A `daylightsavingstimezone` is serialized as an `offset` containing the
standard offset followed by the DST `zonerecurrence` and the standard time
(non-DST) `zonerecurrence`.  The DST recurrence contains an offset that is
the additional delta from the standard offset for daylight savings time
(usually positive, but sometimes zero); the standard time recurrence is
guaranteed to have a zero offset.

##### zonerecurrence

A `zonerecurrence` is serialized as the `string` name of the recurrence
(e.g. "PST"), the `offset` containing the offset of this recurrence, a
description of when the recurrence occurs (the 'year offset'), and finally
two `count` values indicating the (inclusive) years the recurrence covers,
where a zero start year indicates a recurrence that starts at the beginning
of time.

(Note that 1.0 versions of NodaTime serialized the 'beginning of time' as
the year `int32.MinValue` instead of zero. The two should be treated
equally.)

The 'year offset' is:

   * a `count` indicating whether transitions are relative to UTC (0), the
     'wall' (daylight savings) offset (1), or the standard offset (2)
   * a `count` of the month-of-year in which this recurrence starts, with a
     bias such that 13 represents January and 24 represents December
   * a `count` of the day-of-month in which this recurrence starts; values
     from 0..30 indicate a number of days \[-31..-1\] counted from the end of
     a (31 day) month (in practice, only 30 is used, to represent 'last
     \[day-of-week\] of the month'), while values from 32..62 represent the
     number of days \[1..31\] counted from the start of the month
   * a `count` of the day of week in which this recurrence start, with a
     bias such that 7 represents 'no day', and 8..13 represent
     Monday..Sunday
   * a `byte` indicating whether the day of month represents an upper bound
     (0) or lower bound (1); this is only relevant if a day of week is also
     specified, where the rule might be described as e.g. "first Tuesday on
     or after the 15th"
   * an `offset` representing the (non-negative) time of day that the
     recurrence starts
   * a `byte` indicating whether an additional day should be added to the
     calculation (1) or not (0), to cope with transitions documented as
     occurring at 24:00 - these may potentially spill into a following month

#### precalculatedtimezone

A `precalculatedtimezone` is serialized as a string pool, a sequence of
`zoneinterval` periods representing an expansion of all known transitions
before the rules stabilize, and an optional 'tail zone' representing a final stable
state. There is no tail zone if the final state is fixed (if the overall zone has
stopped observing DST); otherwise, it's a `daylightsavingstimezone` with a pair of
rules which extend to the end of time.

Specifically:

   * a per-time-zone string pool containing all the names of the intervals
     (e.g. "PST") used in this zone, as a `count` of the number of entries
     in the pool, followed by that number of `string` values
   * a `count` of the number of precalculated periods, followed by that
     number of `zoneinterval` values
   * a `zoneintervaltransition` for the tail zone (as the end of time if
     there is no tail zone)
   * a `timezone` representing the tail zone, in practice always either of
     type `0x03` (no tail zone) or `0x01` (`daylightsavingstimezone`).

##### zoneinterval

A `zoneinterval` is serialized as the `zoneintervaltransition` representing
the start of the transition (the beginning of time for the first one), an
index into the zone's interval name string pool, as either `byte` or
`fixed32` depending on the total number of entries in the pool, an `offset`
representing the wall-time offset from UTC (including any daylight savings
time) and a second `offset` representing the daylight savings contribution
(or zero if the period is not a daylight savings period).

The periods serialized as part of a `precalculatedtimezone` cover the whole of
time, so each `zoneinterval` finishes as the next one starts (or as the tail
zone starts).

##### zoneintervaltransition

A `zoneintervaltransition` represents an instant at which a transition
begins. It is serialized as a variable number of bytes according to the
following scheme, based on the (signed) number of ticks from the Unix epoch:

   * The beginning of time and end of time are serialized as `0xfe` and
     `0xff` respectively.
   * Numbers of ticks that are a multiple of 30 minutes are serialized as
     the single byte `0x00`..`0x3e` counting the number of 30 minute
     periods, with a bias such that `0x00` represents -15:30, `0x01`
     represents -15:00, `0x1f` represents a zero offset, and `0x3e`
     represents the +15:30. Values outside this range are serialized
	 as one of the forms below.
   * Numbers of ticks that are a whole number of minutes are serialized as
     a `fixed32` representing a 30 bit value with the top two bits of the `fixed32` set to `01`.
	 The 30 bit value is the number of minutes, with a bias such that `0x00000000`
	 represents `-0x1fffff` minutes. (This is about 4 years. Note that this is shorter
	 than the *intended* span of about 1000 years, due to a bug; the bug cannot
	 now be fixed due to compatibility issues.) Values outside the +/-4 year range are
	 serialized as one of the forms below.
   * Numbers of ticks that are a whole number of seconds are serialized as
     a `byte` followed by a `fixed32`, combined to represent a 40 bit value with the
	 top two bits of the first byte set to `10`. The 40 bit value is the number of
	 seconds, with a bias such that `0x0000000000` represents `0x1fffffffffL` seconds.
	 This is about 4355 years. Values outside the +/-4355 year range are
	 serialized as one of the forms below.
   * Any other value is written as a single byte `0xc0` followed by a `fixed64` with
     the number of ticks.
