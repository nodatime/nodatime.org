@Title="Serialization"

Currently serialization is experimental. We will have one serialization assembly for each type of
serialization we support which requires separate dependencies; if and when "stock" binary and XML
serialization are supported, they will be included within the main Noda Time assembly.

Json.NET: NodaTime.Serialization.JsonNet
----------------------------------------

[Json.NET](http://json.net/) is supported within the `NodaTime.Serialization.JsonNet` assembly and the namespace
of the same name.

An extension method of `ConfigureForNodaTime` is provided on both `JsonSerializer` and
`JsonSerializerSettings`. Alternatively, the `NodaConverters` type provides public static read-only fields
for individual converters. (All converters are immutable.)

Custom converters can be created easily from patterns using `NodaPatternConverter`.

Supported types and default representations
===========================================

All default patterns use the invariant culture.

- `Offset`: general pattern, e.g. `+05` or `-03:30`
- `LocalDate`: ISO-8601 date pattern: `yyyy'-'MM'-'dd`
- `LocalTime`: ISO-8601 time pattern, extended to handle fractional seconds: `HH':'mm':'ss.FFFFFFF`
- `LocalDateTime`: ISO-8601 date/time pattern with no time zone specifier, extended to handle fractional seconds: `yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFF`
- `Instant`: an ISO-8601 pattern extended to handle fractional seconds: `yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFF'Z'`
- `Interval`: A compound object of the form `{ Start: xxx, End: yyy }` where `xxx` and `yyy` are represented however the serializer sees fit. (Typically using the default representation above.)
- `Period`: The round-trip period pattern; `NodaConverters.NormalizingIsoPeriodConverter` provides a converter using the normalizing ISO-like pattern
- `Duration`: TBD
- `ZonedDateTime`: TBD
- `DateTimeZone`: The ID is written as a string.

Limitations
===========

- Currently only ISO calendars are supported, and handling for negative and non-four-digit years will depend on the appropriate underlying pattern implementation.
- There's no indication of the time zone provider or its version in the `DateTimeZone` representation.
