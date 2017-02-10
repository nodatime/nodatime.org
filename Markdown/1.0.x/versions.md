@Title="Version history"

User-visible changes from 1.0.0-beta1 onwards. See the
[project repository](https://github.com/nodatime/nodatime) for more
details.

## 1.0.2, not yet released ##

Bug fixes:

- Fixed [issue 174] which made the "Asia/Amman" time zone (Jordan)
  give incorrect values or a `NullReferenceException` when asked for
  a value in 2040 (and possibly other times). Other time zones should
  not be affected.

## 1.0.1, released 2012-11-13 with tzdb 2012i ##

No API changes; this release was purely to fix a problem
with the NodaTime.Testing package.

## 1.0.0, released 2012-11-07 with tzdb 2012i ##

API changes:

- Added the `CalendarSystem.Id` property that returns a unique ID for a
  calendar system, along with the `ForId()` factory method and `Ids` static
  property; `ToString()` now returns the calendar ID rather than the name
- Added support for the `c` custom format specifier for local date values,
  which includes the calendar ID, representing the calendar system
- Added support for the `;` custom format specifier to parse both comma
  and period for fractions of a second; `InstantPattern.ExtendedIsoPattern`,
  `LocalTimePattern.ExtendedIsoPattern`, and
  `LocalDateTimePattern.ExtendedIsoPattern` now accept both as separators
  (issues [140][issue 140] and [141][issue 141])
- Added support for the `r` standard pattern for `LocalDateTime` that includes
  the calendar system
- Renamed the `CreateWithInvariantInfo()` method on the pattern types to
  `CreateWithInvariantCulture()` ([issue 137])

Other:

- Set specific files to be ignored in ZoneInfoCompiler, so we don't
  need to remove them manually each time

## 1.0.0-rc1, released 2012-11-01 with tzdb 2012h

API changes:

- Renamed `DateTimeZone.GetOffsetFromUtc()` to `DateTimeZone.GetUtcOffset()` to
  match the BCL ([issue 121])
- Added `LocalDate.FromWeekYearWeekAndDay()` ([issue 120])
- Text formats can now parse negative values for absolute years ([issue 118])
- `Tick`, `SecondOfDay` and `MillisecondOfDay` properties removed from
  time-based types ([issue 103])
- Removed the `NodaCultureInfo` and `NodaFormatInfo` types ([issue 131] and
  related issues), removed the `FormatInfo` property and `WithFormatInfo()`
  method on pattern types, and changed the culture-specific pattern factory
  methods to take a `CultureInfo` rather than a `NodaFormatInfo`
- Removed `EmptyDateTimeZoneSource`
- Many classes are now `sealed`: `PeriodPattern`, `BclDateTimeZoneProvider`,
  `BclDateTimeZoneSource`, `DateTimeZoneCache`, and `ZoneInterval`, along
  with various exception types and
  `NodaTime.Testing.SingleTransitionDateTimeZone`
- The names of the special resources used by `TzdbDateTimeZoneSource`
  (`VersionKey`, etc) are now internal
- Format of the special resource keys has changed; clients using
  custom-built TZDB resources will need to rebuild

Bug fixes:

- Various fixes to `BclDateTimeZone` not working the same way as a BCL
  `TimeZoneInfo` (issues [114][issue 114], [115][issue 115], and
  [122][issue 122])

Other:

- Noda Time assemblies are now signed ([issue 35])
- Removed support for building under Visual Studio 2008 ([issue 107])

## 1.0.0-beta2, released 2012-08-04 with tzdb 2012e

API changes:

- Overhaul of how to get a `DateTimeZone` from an ID:
   - `IDateTimeZoneProvider` (SPI for time zones) renamed to
     `IDateTimeZoneSource`, along with similar renaming for the built-in
     sources
   - New interface `IDateTimeZoneProvider` aimed at *callers*, with caching
     assumed
   - New class `DateTimeZoneProviders` with static properties to access the
     built-in providers: TZDB, BCL and default (currently TZDB)
   - Removed various `DateTimeZone` static methods in favour of always going
     via an `IDateTimeZoneProvider` implementation
   - `DateTimeZoneCache` now public and implements `IDateTimeZoneProvider`
- `DateTimeZone` no longer has internal abstract methods, making third-party
  implementations possible ([issue 77])
- `DateTimeZone` now implements `IEquatable<DateTimeZone>`, and documents what
  it means for time zones to be equal ([issue 81])
- New core type: `OffsetDateTime` representing a local date/time and an offset
  from UTC, but not full time zone information
- Added a new standard offset pattern of `G`, which is like `g` but using
  "Z" for zero; also available as `OffsetPattern.GeneralInvariantPatternWithZ`
- `Period` and `PeriodBuilder` no longer differentiate between absent and zero
  components (to the extent that they did at all): `Units` has been removed
  from `Period`, period formatting now omits all zero values unconditionally,
  and the `Year` (etc) properties on `PeriodBuilder` are no longer nullable
  ([issue 90])
- Removed the BCL parsing methods and some of the BCL formatting methods from
  `Instant`, `LocalDate`, `LocalDateTime`, and `Offset` in favour of the
  pattern-based API ([issue 87])
- `Duration.ToString()` and `Interval.ToString()` now return more descriptive
  text

- Removed `DateTimeZone.GetSystemDefaultOrNull()`; callers should use the
  provider's `GetSystemDefault()` method and (if necessary) catch the
  `TimeZoneNotFoundException` that it can throw ([issue 61])
- Removed `DateTimeZone.UtcId` and `DateTimeZone.IsFixed` (issues [64][issue 64]
  and [62][issue 62])
- Removed most of the convenience static properties on `Duration` (e.g.
  `Duration.OneStandardDay`) in favour of the existing static methods; removed
  `MinValue` and `MaxValue`, and added `Epsilon` ([issue 70])
- Removed `Instant.BeginningOfTimeLabel` and `Instant.EndOfTimeLabel`
- `Instant.InIsoUtc` renamed to `InUtc`
- `Instant.UnixEpoch` moved to `NodaConstants.UnixEpoch`;
  `NodaConstants.DateTimeEpochTicks` replaced by `BclEpoch`
- Added `Instant.PlusTicks()`
- `LocalDate.LocalDateTime` property changed to `LocalDate.AtMidnight()` method
  ([issue 56])
- `LocalTime` now implements `IComparable<LocalTime>` ([issue 51])
- Added a `LocalTime` constructor taking hours and minutes ([issue 53])
- Removed "component" properties from `Offset`, and renamed the "total"
  properties to just `Ticks` and `Milliseconds`
- Removed `Offset.Create()` methods (and moved them in slightly different form
  in a new internal `TestObjects` class in `NodaTime.Test`)
- Added `Period.ToDuration()` ([issue 55]) and `Period.CreateComparer()`
  ([issue 69])
- `Period.Empty` renamed to `Period.Zero` (as part of [issue 90])
- `PeriodBuilder` no longer implements `IEquatable<PeriodBuilder>`
  ([issue 91])
- Removed `SystemClock.SystemNow` in favour of using `SystemClock.Instance.Now`
  if you really have to
- Added `ZonedDateTime.ToOffsetDateTime()`, which returns the `OffsetDateTime`
  equivalent to a `ZonedDateTime`
- Removed the Buddhist `Era` (as there is no Buddhist calendar implementation)

- `NodaTime.Testing.StubClock` renamed to `FakeClock`, and gains an
  auto-advance option (issues [72][issue 72] and [73][issue 73])
- `NodaTime.Testing.TimeZones.SingleTransitionZone` renamed to
  `SingleTransitionDateTimeZone`

Bug fixes:

- Many

## 1.0.0-beta1, released 2012-04-12 with tzdb 2012c

- Initial beta release
