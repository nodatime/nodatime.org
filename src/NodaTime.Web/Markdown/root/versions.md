@Title="Version history"

User-visible changes from 1.0.0-beta1 onwards. See the
[project repository](https://github.com/nodatime/nodatime) for more
details.

See the [end of this page](#tzdb-updates) for the policy on which
versions receive patch updates for TZDB releases.

## 3.2.2 and 3.1.14, released 2025-03-23 with tzdb 2025b

This patch release simply updates the built-in TZDB time
zone data to 2025b, using CLDR version 45 for Windows mappings.

## 3.2.1 and 3.1.13, released 2025-01-16 with tzdb 2025a

This patch release simply updates the built-in TZDB time
zone data to 2025a, using CLDR version 45 for Windows mappings.

## 3.2.0, released 2024-10-13 with tzdb 2024b

Changes since 3.1.x:

- Various minor docs/examples fixes
- More LocalDateTime ISO patterns with reduced precision (
- More LocalTime ISO formats
- Support for configurable "2 digit year max value" in patterns
- Fixed arguments passed to base constructors in AmbiguousTimeException and SkippedTimeException
- Add the unparsed value (and index) to UnparsableValueException
- Made previous UnparsableValueException constructors obsolete
- Optimizations for text handling
- Added .NET 8 target
- Added Instant.ToUnixTimeSecondsAndNanoseconds
- Added Duration.ToInt128Nanoseconds and Duration.FromNanoseconds(Int128) (.NET 8 only)
- Conditionalized the dependency on System.Runtime.CompilerServices.Unsafe
- Added Period.MinValue and Period.MaxValue
- Added generic math support (.NET 8 only)
- Added TimeProvider extension methods (.NET 8 only)

## 3.1.12, released 2024-09-05 with tzdb 2024b

This patch release simply updates the built-in TZDB time
zone data to 2024b, using CLDR version 44 for Windows mappings.
(It also makes some trivial changes to documentation, but no
actual code changes.)

## 3.1.11, released 2024-02-01 with tzdb 2024a

This patch release simply updates the built-in TZDB time
zone data to 2024a, using CLDR version 44 for Windows mappings.

## 3.1.10, released 2023-12-23 with tzdb 2023d

This patch release simply updates the built-in TZDB time
zone data to 2023d, using CLDR version 42 for Windows mappings.

## 3.1.9, released 2023-03-24 with tzdb 2023c

This patch release simply updates the built-in TZDB time
zone data to 2023c, using CLDR version 42 for Windows mappings.

## 3.1.8, released 2023-03-24 with tzdb 2023b

This patch release simply updates the built-in TZDB time
zone data to 2023b, using CLDR version 42 for Windows mappings.

## 3.1.7, released 2023-03-23 with tzdb 2023a

This patch release simply updates the built-in TZDB time
zone data to 2023a, using CLDR version 42 for Windows mappings.

## 3.1.6, released 2022-10-29 with tzdb 2022g

This patch release simply updates the built-in TZDB time
zone data to 2022g, using CLDR version 41 for Windows mappings.

## 3.1.5, released 2022-10-29 with tzdb 2022f

This patch release simply updates the built-in TZDB time
zone data to 2022f, using CLDR version 41 for Windows mappings.

## 3.1.4, released 2022-10-11 with tzdb 2022e

This patch release simply updates the built-in TZDB time
zone data to 2022e, using CLDR version 41 for Windows mappings.

## 3.1.3, released 2022-09-24 with tzdb 2022d

This patch release simply updates the built-in TZDB time
zone data to 2022d, using CLDR version 41 for Windows mappings.

## 3.1.2, released 2022-08-16 with tzdb 2022c

This patch release simply updates the built-in TZDB time
zone data to 2022c, using CLDR version 41 for Windows mappings.

There are no data differences between tzdb 2022b and 2022c; this release
is primarily to avoid confusion as to why 2022c might be missing.

## 3.1.1, released 2022-08-12 with tzdb 2022b

This patch release simply updates the built-in TZDB time
zone data to 2022b, using CLDR version 40 for Windows mappings.
(Note that CLDR 41 has been released, but has the same mappings as CLDR 40.)

## 3.1.0, release 2022-04-18 with tzdb 2022a

Changes since 3.0.0:

- Added .NET 6.0 target
- Implemented `DateOnly` and `TimeOnly` conversions to/from
  `LocalDate` and `LocalTime`
- Fixed `BclDateTimeZone` support in .NET 6.0 on Unix
- Implemented `ToString` method for `YearMonth`
- Improved error messages for bad format specifiers
- Added `LocalDateTime.MinIsoValue` and `LocalDate.MaxIsoValue`
- Added `Period.Between` overload accepting `YearMonth` values
- Added `YearMonth.PlusMonths` method
- Added `Period.DaysBetween` method (previously internal)
- Various documentation improvements

## 2.4.18, 3.0.10, released 2022-03-17 with tzdb 2022a

This set of patch releases simply updates the built-in TZDB time
zone data to 2022a, using CLDR version 40 for Windows mappings.

## 2.4.17, 3.0.9, released 2021-10-22 with tzdb 2021e

This set of patch releases simply updates the built-in TZDB time
zone data to 2021e, using CLDR version 39 for Windows mappings.

## 2.4.16, 3.0.8, released 2021-10-16 with tzdb 2021d

This set of patch releases simply updates the built-in TZDB time
zone data to 2021d, using CLDR version 39 for Windows mappings.

## 2.4.15, 3.0.7, released 2021-10-02 with tzdb 2021c

This set of patch releases simply updates the built-in TZDB time
zone data to 2021c, using CLDR version 38.1 for Windows mappings.

## 2.4.14, 3.0.6, released 2021-09-25 with tzdb 2021b

This set of patch releases simply updates the built-in TZDB time
zone data to 2021b, using CLDR version 38.1 for Windows mappings.

## 2.4.13, 3.0.5, released 2021-01-24 with tzdb 2021a

This set of patch releases simply updates the built-in TZDB time
zone data to 2021a, using CLDR version 38.1 for Windows mappings.

## 2.4.12, 3.0.4, released 2020-12-23 with tzdb 2020e

This set of patch releases simply updates the built-in TZDB time
zone data to 2020e.

## 2.4.11, 3.0.3, released 2020-10-22 with tzdb 2020d

This set of patch releases simply updates the built-in TZDB time
zone data to 2020d.

## 2.4.10, 3.0.2, released 2020-10-17 with tzdb 2020c

This set of patch releases simply updates the built-in TZDB time
zone data to 2020c.

## 2.4.9, 3.0.1, released 2020-10-08 with tzdb 2020b

This set of patch releases simply updates the built-in TZDB time
zone data to 2020b.

## 3.0.0, released 2020-05-22 with tzdb 2020a

Changes since 2.x:

- Removal of support for binary serialization
- Type converter support
- C# 8 nullable reference type support
- A single target of .NET Standard 2.0
- Provide dictionaries from TZDB ID to Windows ID and vice versa [issue 274]
- Provide Zone/Clock/Calendar properties on ZonedClock [issue 1362]
- Added LocalTime.FromHoursSinceMidnight and FromMinutesSinceMidnight [issue 1366]
- Introduce a date adjuster to add a period when invoked
- Added o and O invariant round-trip standard patterns to LocalTime [issue 1361]
- Added standard LocalDate patterns for ISO and "full round trip with calendar" [issue 1363]
- Use 'S' as the extended ISO standard pattern specifier [issue 1219]
- Added a template value to InstantPattern (like other patterns)
- Added a new roundtrip pattern for durations [issue 1290]
- Support for XML schemas [issue 1496]
- Improved date parsing performance for ISO patterns
- `AnnualDate` now implements the non-generic `IComparable` interface
- Added `LocalTimePattern.GeneralIso` property ("HH:mm:ss")
- Added `LocalDate YearMonth.OnDayOfMonth(int)`
- Added the 'M' standard pattern for `LocalDate`, meaning "month/day pattern"
- Added a type converter for `ZonedDateTime`. This uses the `DateTimeZoneProvider`
  in `NodaTime.Text.TypeConverterSettings` for parsing.
- Added XML serialization support for `AnnualDate`
- Fixes to NuGet packages around SourceLink and deterministic source paths
- `BclDateTimeZone` now interprets a `TimeZoneInfo` rule of a transition at 23:59:59.999 as "at midnight" [issue 1524]
- Added equality operators to `Period` [issue 1529]
- Added comparison operators to `YearMonth`
- Added a type converter for `YearMonth` [issue 1539]
- Improved performance of equality comparisons for `LocalDate` and `YearMonth`
- Updated the System.Runtime.CompilerServices.Unsafe dependency

Known issues:

- The NodaTime.Testing 3.0.0 package does not include XML documentation

New features since 3.0.0-beta02:

- Fixes to NuGet packages around SourceLink and deterministic source paths
- `BclDateTimeZone` now interprets a `TimeZoneInfo` rule of a transition at 23:59:59.999 as "at midnight" [issue 1524]
- Added equality operators to `Period` [issue 1529]
- Added comparison operators to `YearMonth`
- Added a type converter for `YearMonth` [issue 1539]
- Improved performance of equality comparisons for `LocalDate` and `YearMonth`
- Updated the System.Runtime.CompilerServices.Unsafe dependency

## 3.0.0-beta02, released 2020-04-25 with tzdb 2020a

Hopefully the last release before 3.0.0.

New features since 3.0.0-beta01:

- Support for XML schemas [issue 1496]
- Improved date parsing performance for ISO patterns
- `AnnualDate` now implements the non-generic `IComparable` interface
- Added `LocalTimePattern.GeneralIso` property ("HH:mm:ss")
- Added `LocalDate YearMonth.OnDayOfMonth(int)`
- Added the 'M' standard pattern for `LocalDate`, meaning "month/day pattern"
- Added a type converter for `ZonedDateTime`. This uses the `DateTimeZoneProvider`
  in `NodaTime.Text.TypeConverterSettings` for parsing.
- Added XML serialization support for `AnnualDate`

Semi-breaking change:

- `DateTimeZoneProviders.Serialization` is now deprecated. Use `NodaTime.Xml.XmlSerializationSettings.DateTimeZoneProvider`
instead. We'll keep `DateTimeZoneProviders.Serialization` in 3.0.0 for binary compatibility, but probably remove it for 4.0.0 (if
that ever happens). The deprecated property just delegates to the new property.

## 2.4.8, released 2020-04-25 with tzdb 2020a.

This patch release simply updates the built-in TZDB time
zone data to 2020a.

## 2.4.7, released 2019-09-11 with tzdb 2019c.

This patch release simply updates the built-in TZDB time
zone data to 2019c.

## 3.0.0-beta01, released 2019-09-03 with tzdb 2019b.

First beta release of 3.0.0.

New features since 3.0.0-alpha01:

- Provide dictionaries from TZDB ID to Windows ID and vice versa [issue 274]
- Provide Zone/Clock/Calendar properties on ZonedClock [issue 1362]
- Add LocalTime.FromHoursSinceMidnight and FromMinutesSinceMidnight [issue 1366]
- Introduce a date adjuster to add a period when invoked
- Add o and O invariant round-trip standard patterns to LocalTime [issue 1361]
- Add standard LocalDate patterns for ISO and "full round trip with calendar" [issue 1363]
- Use 'S' as the extended ISO standard pattern specifier [issue 1219]
- Add a template value to InstantPattern (like other patterns)
- Add a new roundtrip pattern for durations [issue 1290]

## 2.4.6, released 2019-07-02 with tzdb 2019b.

This patch release simply updates the built-in TZDB time
zone data to 2019b.

## 2.4.5, released 2019-03-26 with tzdb 2019a.

This patch release simply updates the built-in TZDB time
zone data to 2019a.

## 3.0.0-alpha01, released 2019-02-05 with tzdb2018i

Initial alpha release to allow users to experiment with:

- Type converter support
- C# 8 nullable reference type support
- A single target of .NET Standard 2.0

## 2.4.4, released 2018-12-31 with tzdb 2018i

This patch release simply updates the built-in TZDB time
zone data to 2018i.

## 2.4.3, released 2018-12-30 with tzdb 2018h

This patch release mostly updates the built-in TZDB time
zone data to 2018h. However, it also fixes [issue 1227].

Note that the 2.3.x series no longer receives TZDB updates. The
1.4.x series has not been included in this update, but may be
updated later. (See [issue 1243] for discussion.)

## 1.4.7, 2.3.2, 2.4.2, released 2018-10-28 with tzdb 2018g

This set of patch releases simply updates the built-in TZDB time
zone data to 2018g.

## 1.4.6, 2.3.1, 2.4.1, released 2018-10-18 with tzdb 2018f

This set of patch releases simply updates the built-in TZDB time
zone data to 2018f.

## 2.4.0, released 2018-08-19 with tzdb 2018e

- The NuGet package explicitly supports .NET Standard 2.0 now. This
  uses the same build as for .NET Standard 1.3 (no support for BclDateTimeZone)
  but gives a better installation experience when using a platform supporting it.
  When targeting the full .NET Framework, the .NET 4.5 build will still be picked up.
- Fixed [issue 1193], a rare but nasty deadlock due to type initialization

## 2.3.0, released 2018-05-19 with tzdb 2018e

New features:

- `Deconstruct` methods to support C# 7 deconstruction in many types
- `Min`/`Max` methods added to `LocalDate`, `LocalTime` and `LocalDateTime`
- `OffsetDate` and `OffsetTime` structs added to represent dates or times-of-day with offsets,
  with conversions from other types as appropriate.
- `OffsetDateTime.InZone` method added for easier conversion to `ZonedDateTime`
- New operations on `DateInterval`: `Contains(DateInterval)`, `Union`,
  `Intersection` and iteration (it implements `IEnumerable<LocalDate>`)
- New calendar system: the Badíʿ calendar
- Text handling for `AnnualDate`

2.3.0-beta01 was released on 2017-12-13, and 2.3.0-beta02 was
released on 2018-03-25.

## 1.3.8, 1.4.5, 2.0.8, 2.1.5, 2.2.6, released 2018-05-04 with tzdb 2018e

This set of patch releases simply updates the built-in TZDB time
zone data to 2018e. Note that this is the first release using
negative DST. For example, Ireland observes standard time in the
summer, and DST of -1 hour in the winter. This does not change any
local times, just the standard/savings components.

## 1.3.7, 1.4.4, 2.0.7, 2.1.4, 2.2.5, released 2018-03-24 with tzdb 2018d

This set of patch releases simply updates the built-in TZDB time
zone data to 2018d.

## 1.3.6, 1.4.3, 2.0.6, 2.1.3, 2.2.4, released 2018-01-24 with tzdb 2018c

This set of patch releases simply updates the built-in TZDB time
zone data to 2018c. Note that 2018a and 2018b were skipped. 2018a and 2018b used negative
daylight savings for Ireland in winter - a change that is under significant discussion, and
is reverted in 2018c.

## 2.0.5, 2.1.2, 2.2.3, released 2017-10-23/2017-11-16 with tzdb 2017c

This set of patch releases simply rebuilds the previous set of
releases, but in release mode instead of debug.

Fixes [issue 1027].

## 1.3.5, 1.4.2, 2.0.4, 2.1.1, 2.2.2, released 2017-10-23/2017-10-24 with tzdb 2017c

This set of patch releases simply updates the built-in TZDB time
zone data to 2017c.

## 2.2.1, released 2017-10-14 with tzdb 2017b

Bug-fix release:

- Fixes [issue 957]: Incorrect time zone calculations near the start/end of time
- Fixes [issue 971]: Clarify exception for unknown standard patterns to point to %
- Fixes [issue 979]: `Instant` bounds checking bypassed when subtracting a `Duration`
- Fixes [issue 981]: Failure to parse time zones with IDs like
  "Etc/GMT-12". (Zones where another zone has an ID with common prefix *and*
  there's at least one zone between the two, lexically.)

## 1.4.1, released 2017-10-14 with tzdb 2017b

Bug-fix release:

- Fixes [issue 981]: Failure to parse time zones with IDs like
  "Etc/GMT-12" (Zones where another zone has an ID with common prefix *and*
  there's at least one zone between the two, lexically.)

## 2.2.0, released 2017-07-09 and 2017-07-14 with tzdb 2017b

This was an accidental release, immediately delisted on nuget.org but later relisted due to issues with
it actually being visible anyway. (The NodaTime.Testing package was only released on July 14th.)

It's exactly the same as 2.1.0: upgrading from 2.1.0 to 2.2.0 should be a no-op.

## 2.1.0, released 2017-07-09 with tzdb 2017b

- Some optimizations to `Period` which didn't get into 2.0.x (most `Duration` ones did)
- Obsoleted the misnamed `ToDayOfWeek` extension method ([issue 776])
- Added factory methods and some extra functionality to `ParseResult` to make it easier
  to work with it in a functional context ([issue 780])
- Added [SourceLink](https://github.com/ctaggart/SourceLink) support ([issue 870])
- Added `LocalDate.MinIsoValue`/`MaxIsoValue` ([issue 898])

(Beta 1 was released on 2017-07-05. Only change since beta 1 was the final addition listed above.)

## 1.4.0, released 2017-07-09 with tzdb 2017b

Release to enable migration to 2.0.

- `ZonedClock` and `WeekYearRules` backported from 2.0
- Added extension methods for `IClock` and `IDateTimeZoneSource` to allow smoother migration
- Members removed in 2.0 are obsolete where possible:
   - In most cases, the message indicates how to migrate code ready for 2.0
   - In some cases, members have been removed with no equivalent (e.g. `Century` properties)
   - The `IsoDayOfWeek` properties which have been renamed to `DayOfWeek` in 2.0 have *not* been
     made obsolete as there'd be no good way of dealing with this. (Just rename uses *after* migrating to 2.0.)

(Beta 1 was released on 2017-07-04. No changes from beta to GA.)

## 2.0.3, released 2017-06-13 with tzdb 2017b

Patch release: optimization only:

- Fixes [issue 837]: `Instant.FromUnixTimeSeconds` slow (fix in `Duration.FromXyz`)
- Fixes [issue 838]: Improve `Duration` performance further

## 2.0.2, released 2017-05-22 with tzdb 2017b

Patch release: bugfix only:

- Fixes [issue 824]: incorrect Period computations.

## 2.0.1, released 2017-05-05 with tzdb 2017b

Patch release: bugfixes only:

- Fixes [issue 743]: handling of time zones on Mono on Linux
- Fixes [issue 801]: `LocalDate` constructor validation
- Fixes [issue 807]: `LocalTime` validation

## 2.0, released 2017-03-31 with tzdb 2017b

Major release: do *not* expect to be able to upgrade from 1.x without making adjustments to your code; please
read the list of breaking changes and see the [Noda Time 1.x to 2.0 migration guide](/unstable/userguide/migration-to-2)
for full details.

New features include:

- Nanosecond precision
- .NET Core support: 2.0 targets `netstandard1.3` and `net45` TFMs
- Improved performance on many `LocalDate`-related operations due to new internal representation
- Time zone reimplementation
- Many extension methods, including on BCL types, in the `NodaTime.Extensions` namespace
- Date and time adjusters, e.g. `var monthEnd = date.With(DateAdjusters.EndOfMonth)`
- Extension methods for testing (to allow things like `19.June(1976)`) in the `NodaTime.Testing.Extensions` namespace
- `AnnualDate` to represent events like birthdays and anniversaries
- `DateInterval` - a `LocalDate`-based interval type
- `ZonedClock` - a wrapper around `IClock` with a time zone, making it easier to get the current day/time in a time zone repeatedly
- `WeekYearRules` - a calendar-neutral way of extracting week-year information
- Simpler calendar access using properties in `CalendarSystem`
- Embedded date/time patterns in LocalDateTime, `ZonedDateTime` and `OffsetDateTime` patterns

Breaking changes:

- When passing an `IFormatProvider`, only `CultureInfo` and `DateTimeFormatInfo` values can be used;
  any other non-null reference will now throw an exception. When a `DateTimeFormatInfo` is provided,
  the invariant culture is used for resource lookups and text comparisons.
- Removed `LocalDateTime` constructors accepting tick values
- Changed `LocalTime` constructors accepting tick values (tick-of-second and tick-of-millisecond)
  to be static factory methods.
- Renamed all static pattern properties to remove the `Pattern` suffix
- Renamed the `IsoDayOfWeek` properties in `LocalDate`, `LocalDateTime`, `OffsetDateTime`
  and `ZonedDateTime` to just `DayOfWeek`, and removed the previous numeric `DayOfWeek` properties.
- Removed members which had already been made obsolete in the 1.x release line, including
  support for the legacy resource-based time zone data format.
- Removed `Instant(long)` constructor from the public API.
- Removed `LocalTime.LocalDateTime` property.
- Changed the behaviour of the `LenientResolver` to more closely match real-world usage.
  This also affects `DateTimeZone.AtLeniently` and `LocalDateTime.InZoneLeniently`.
   - For ambiguous values, the lenient resolver used to return the later of the two possible instants.
     It now returns the *earlier* of the two possible instants.  For example, if 01:00 is ambiguous, it used to return
     1:00 standard time and it now will return 01:00 *daylight* time.
   - For skipped values, the lenient resolver used to return the instant corresponding to the first possible local time
     following the "gap".  It now returns the instant that would have occurred if the gap had not existed.  This
     corresponds to a local time that is shifted forward by the duration of the gap.  For example, if values from
     02:00 to 02:59 were skipped, a value of 02:30 used to return 03:00 and it will now return 03:30.
- `Period` and `PeriodBuilder` properties for date-based values (years, months, weeks, days) are now of type `int` rather than `long`.
- Default values for local date-based structs (`LocalDate` etc) now return 0001-01-01
  instead of the Unix epoch.
- `CalendarSystem.GetMaxMonth` has been renamed to `GetMonthsInYear`.
- `Offset` can no longer represent sub-second offsets (which are not used in
  practice); formatting and parsing for these has been removed.
- `Offset` now has a range of +/- 18 hours instead of just less than a complete day.
- The "numeric" standard patterns for `Instant` and `Offset` have been removed.
- Renamed `IClock.Now` to `IClock.GetCurrentInstant()`.
- Prohibited unescaped ASCII letters from acting as literals in patterns (except 'T' in date/time patterns)
- `CenturyOfEra` and `YearOfCentury` removed from `LocalDate` and related types.
- `Duration.FromStandardDays` has been renamed to `Duration.FromDays`
- `Duration.FromStandardWeeks` has been removed.
- Constants within `NodaConstants` which had `Standard` in their name have had that part removed.
- Julian and Coptic calendars with a minimum number of days in the first week other than 4 are not supported.
- Factory methods for the Julian, Coptic and Persian calendars have been converted into properties.
- `y` and `yyy` are no longer supported in date format specifiers; use `yy` or `yyyy` instead.
- The date format specifiers `yy` and `yyyy` now refer to the year of era instead of the absolute year; `u` is used for absolute year.
- Some parameters have been renamed for consistency, affecting code which uses named arguments.
- Factory methods for converting from units since the Unix epoch to `Instant` have been renamed to `FromUnixTimeXyz()`
- The `Instant.Ticks` property has been converted to a method, `Instant.ToUnixTimeTicks()`. (This reflects
  the fact that it would no longer reflect the complete state of the object, and aims to obscure the fact
  that the Unix epoch is the internal epoch in Noda Time.)
- Text formatting no longer uses the `NumberFormatInfo` from a culture for positive or negative signs.
- Binary data serialized with 1.x is not compatible with 2.0.
- Properties related to week-years (e.g. `WeekOfWeekYear`) have been removed, in favour of a more
  flexible system. See the [week-years guide](/unstable/userguide/weekyears) for more information.
- Removal of `IDateTimeZoneSource.MapTimeZoneId` and the introduction of `IDateTimeZoneSource.GetSystemDefaultId` instead.

Bug fixes:

- `BclDateTimeZone` has been reimplemented from scratch. This may result in a very few differences
  in the interpretation of when an adjustment rule starts and ends. It is now known to be incompatible
  with the BCL in well-understood ways which we don't intend to change.

Other:

- Added a `ReturnForwardShifted` resolver, which shifts values in the daylight saving time "gap" forward by the duration of the gap,
  effectively returning the instant that would have occurred had the gap not existed.  This was added to support the new behaviour of
  the "lenient" resolver (see above), but can also be used separately.
- When an `IDateTimeZoneSource` advertises a zone with an ID corresponding to a fixed-offset
  zone, `DateTimeZoneCache` now consults the source first. This fixes [issue 332].

## 1.3.4, released 2017-03-21 with tzdb 2017b

(No code changes.)

## 1.3.3, released 2017-03-07 with tzdb 2017a

(No code changes.)

## 1.3.2, released 2016-04-14 with tzdb 2016c

Only one code change, primarily an update to TZDB 2016c.

Bug fixes:

- When parsing a date, correctly return a failed parse result when
  provided a month number of 0, instead of throwing an exception
  ([issue 414]).

## 1.3.1, released 2015-03-06 with tzdb 2015a

Bug fixes:

- Worked around a limitation of the .NET `TimeZoneInfo` API that caused
  `BclDateTimeZone` to incorrectly calculate time zone conversions for
  Russian time zones before 2014-10-26 ([issue 342]). (This is essentially
  the same problem documented in [Microsoft KB
  3012229](https://support.microsoft.com/kb/3012229).)
- TZDB zone transitions that occur at 24:00 at the end of the last year in a
  zone rule are now handled correctly ([issue 335])
- Instances of `BclDateTimeZone` are now considered equal if they wrap the
  same underlying `TimeZoneInfo`, rather than always throwing
  `NotImplementedException` ([issue 334])
- The `NodaTime` assembly now correctly declares a dependency on
  `System.Xml`, required due to XML serialization support ([issue 339])

Other:

- Fixed a case issue in the NuGet package definition that broke ASP.NET's
  `kpm restore` ([issue 345])
- Added XamariniOS1 as a NuGet target ([issue 340])
- Various reported documentation issues resolved ([issue 326]
  and [issue 346], among others)
- Updated `TzdbCompiler` to handle newer versions of the TZDB source
  distribution

## 1.3.0, released 2014-06-27 with tzdb 2014e

Major features:

- Support for the Persian/Solar Hijri calendar
- Experimental support for the Hebrew calendar ([issue 238])

API changes:

- Added `CalendarSystem.GetPersianCalendar()` and `Era.AnnoPersico` for the
  Persian calendar
- Added `CalendarSystem.GetHebrewCalendar()`, `Era.AnnoMundi`, and the
  `HebrewMonthNumbering` enum for the Hebrew calendar
- Added `LocalDate.At(LocalTime)` and `LocalTime.On(LocalDate)`, more
  discoverable versions of `LocalDate + LocalTime` ([issue 192])
- Added `OffsetDateTime.WithOffset()`, which returns an `OffsetDateTime`
  representing the same point in time, but using a given offset ([issue 246])
- Added `ZonedDateTime.IsDaylightSavingTime()`, mirroring the method of the
  same name in `System.DateTime` ([issue 264])
- Added missing `OffsetDateTimePattern.CreateWithInvariantCulture()`,
  `OffsetDateTimePattern.CreateWithCurrentCulture()`, and
  `ZonedDateTimePattern.CreateWithCurrentCulture()` ([issue 267])
- Added `OffsetDateTimePattern.Rfc3339Pattern`, an RFC 3339-compliant pattern
  formatter ([issue 284])
- `ZonedDateTimePattern` patterns containing the `G` and `F` standard patterns
  can now be used for parsing when used with a zone provider ([issue 277])
- Marked the desktop assembly with the `AllowPartiallyTrustedCallers`
  attribute (and relevant types with the `SecurityCritical` attribute),
  allowing it to be used in partially-trusted contexts (issues [issue 268]
  and [issue 272])
- Changed the previously-undocumented format for `Interval.ToString()` to
  ISO-8601 interval format ([issue 270])

API changes for NodaTime.Serialization.JsonNet:

- Added `JsonSerializerSettings.WithIsoIntervalConverter()` and
  `JsonSerializer.WithIsoIntervalConverter()` extension methods, which change
  the representation of a serialized `Interval` from the object-based format
  (with 'Start' and 'End' properties) to the string representation using the
  ISO-8601 interval format ([issue 270])
- Added `NodaConverters.IsoIntervalConverter`, which provides access to the
  `JsonConverter` used for the ISO-8601 interval format

Bug fixes:

- Fixed recognition of time zone transitions that only adjusted the way a given
  offset is divided between standard and daylight saving time (e.g. the UK,
  1968-1971)
- Fixed a bug where formatting a time with a whole number of seconds using a
  fractional-second format in a locale that doesn't use '.' as a decimal
  separator could erroneously cause the result to have a trailing '.'
  ([issue 273])
- Fixed the JSON and XML serialization formats for `OffsetDateTime` to emit
  offsets compatible with RFC 3339, and by extension, with JavaScript's
  `Date.parse()` and .NET's XML conversions ([issue 284])

Other:

- Significantly improved the performance of various parsing/formatting and
  point-in-time calculation methods
- Failures during text parsing now indicate which part of the input failed to
  match the given part of the pattern ([issue 288])
- API documentation now indicates which versions of Noda Time support the given
  member ([issue 261])
- Annotations added to support [ReSharper](https://www.jetbrains.com/resharper/) users,
  by indicating pure members, parameters which must be non-null etc
  ([issue 207])

## 1.3.0-beta1, released 2014-06-19 with tzdb 2014e

Essentially identical to 1.3.0.  The only change was the removal of some older
TZDB versions from the source distribution.

## 1.2.0, released 2013-11-16 with tzdb 2013h

Major features:

- Noda Time core types now support XML and (on the desktop build) binary
  serialization (issues [issue 24] and [issue 226])
- New optional `NodaTime.Serialization.JsonNet` NuGet package enabling JSON
  serialization for Noda Time types using [Json.NET](http://json.net/)
- New support for parsing and formatting `Duration`, `OffsetDateTime`, and
  `ZonedDateTime`, including new custom format patterns representing an
  embedded offset (`o`), time zone identifier (`z`), (format-only) time zone
  abbreviation (`x`), and various patterns for `Duration` (issues
  [issue 139], [issue 171], and [issue 216])
- New web site: [http://nodatime.org/](http://nodatime.org/)

API changes:

- Added `DateTimeZoneProviders.Serialization`, which is a static _mutable_
  property used to control the time zone provider used by XML and binary
  serialization
- New classes `DurationPattern`, `OffsetDateTimePattern`, and
  `ZonedDateTimePattern` that represent patterns for parsing and formatting
  `Duration`, `OffsetDateTime`, and `ZonedDateTime` respectively
- Added `LocalDateTimePattern` properties `GeneralIsoPattern`,
  `BclRoundtripPattern`, and `FullRoundtripPattern`, which provide
  programmatic access to the `o`/`O`, `r`, and `s` patterns, respectively
- Default formatting (i.e. `ToString()`) for `Duration`, `OffsetDateTime`,
  and `ZonedDateTime` has changed
- Local date patterns that include `yyyy` (for example, the standard ones)
  can now parse and format five-digit years in cases where the result would
  not be ambiguous
- Added `InstantPattern.WithMinMaxLabels()`, which allows replacement of the
  text used to format the minimum and maximum instants (the defaults for
  which have also changed)
- Added `Era.AnnoMartyrum`, obsoleting the misnamed `AnnoMartyrm`
- Added `Instant.WithOffset()`, which parallels the equivalent method in
  `LocalDateTime`
- Added `OffsetDateTime.WithCalendar()`, which returns an `OffsetDateTime`
  representing the same point in time, but using a given calendar system
- Added `Interval.Contains()`, which returns whether an `Interval` contains
  the given `Instant`
- Added `ZonedDateTime.Calendar`, which returns the calendar system used by
  a `ZonedDateTime`
- Added `ZonedDateTime.GetZoneInterval()`, a convenience method that returns
  the `ZoneInterval` of the time zone used by a `ZonedDateTime`
  ([issue 211])
- Added `ParseResult.Exception`, which provides direct access to the
  exception that would be thrown by `GetValueOrThrow()`
- `DateTimeZoneNotFoundException` and `InvalidNodaDataException` are now
  sealed (as they should have been from the start)
- `CalendarSystem` is now also `sealed` (though it was previously an
  `abstract` class with an internal constructor, so this should have no
  practical effect)

Newly-obsolete members:

- `Era.AnnoMartyrm`, which was a typo for the newly-introduced `AnnoMartyrum`

Bug fixes:

- Built-in time zone providers are now initialised lazily ([issue 209])
- Fixed a bug where `Period.Between()` could return a mixture of positive
  and negative values when called with end-of-month and near-leap-year
  values ([issue 223] and [issue 224])
- Fixed another bug where `Period.Between()` would incorrectly overflow when
  creating a `Period` that exceeded `long.MaxValue` ticks ([issue 229])
- Fixed two serious bugs in the Islamic calendar system ([issue 225])
- Custom formats for `Instant` no longer trim whitespace from the result
  ([issue 227])
- Removed support for the (undocumented) upper-case aliases for the
  existing `Instant` patterns `n`, `g`, and `d` ([issue 228])

Other:

- Completely changed the way documentation is generated; note in
  particular that the developer guide is no longer shipped with releases,
  but is available at
  [http://nodatime.org/developer](http://nodatime.org/developer) instead
- Visual Studio solution files have been split out into
  `NodaTime-{All,Core,Documentation,Tools}.sln` ([issue 214])
- The `ZoneInfoCompiler` tool has been renamed to `NodaTime.TzdbCompiler`

## 1.2.0-rc2, released 2013-11-12 with tzdb 2013h

First release of the NodaTime.Serialization.JsonNet assembly.

Essentially identical to 1.2.0.  The only differences between 1.2.0-rc2 and
1.2.0 were documentation and release process improvements.

## 1.2.0-rc1, released 2013-11-01 with tzdb 2013h

Essentially identical to 1.2.0.  The only differences between 1.2.0-rc1 and
1.2.0-rc2 were within the NodaTime.Serialization.JsonNet assembly (not included
in 1.2.0-rc1, and so not documented here), and to some benchmarks and
documentation.

## 1.1.1, released 2013-08-30 with tzdb 2013d

Bug fixes:

- Workaround for a Mono bug where `BclDateTimeZoneSource.GetIds()` would not
  return the local time zone ID, which caused
  `DateTimeZoneProviders.Bcl.GetSystemDefault()` to fail ([issue 235])
- Fixed a shortcoming in the PCL implementation of
  `TzdbDateTimeZoneSource.MapTimeZoneId()` that caused
  `DateTimeZoneProviders.Tzdb.GetSystemDefault()` to fail when the system
  culture was set to something other than English ([issue 221]).  Under the
  PCL, we can't get the ID of a `TimeZoneInfo`, so we were relying on the
  `StandardName` property - but that (unexpectedly) varies by system locale.
  The fix adds a fallback that attempts to determine which TZDB time zone best
  fits the given `TimeZoneInfo`, by looking at transitions of all zones in the
  current year.

## 1.1.0, released 2013-04-06 with tzdb 2013b

API changes:

- Added `OffsetDateTime.Comparer` and `ZonedDateTime.Comparer`,
  `IComparer<T>` implementations that compare values by either the local
  date/time or the underlying instant
- `TzdbDateTimeZoneSource.WindowsZones` renamed to `WindowsMapping`
- `ZoneEqualityComparer` converted to use factory methods rather than
  multiple constructors; some options' names changed

## 1.1.0-rc1, released 2013-03-28 with tzdb 2013b

Major features:

- Noda Time assemblies are now also available in Portable Class Library
  versions (see the [installation instructions](/userguide/installation) for details)
- Noda Time now uses a self-contained (and more compact) format for storing
  TZDB data that is not directly tied to .NET resources; the old format was
  not supportable in the PCL build

API changes:

- The new `DateTimeZone.GetZoneIntervals` methods return a sequence of zone
  intervals which cover a particular interval ([issue 172])
- A new `ZoneEqualityComparer` class allows time zones to be compared for
  equality with varying degrees of strictness, over a given interval
- `LocalDate`, `LocalTime`, `LocalDateTime`, and `ZonedDateTime` now
  implement the non-generic `IComparable` interface (explicitly, to
  avoid accidental use)
- Much more information is now exposed via `TzdbDateTimeZoneSource`:
   - Added `Aliases` and `CanonicalIdMap` properties, which together provide
     bidirectional mappings between a canonical TZDB ID and its aliases
     ([issue 32])
   - Added a `WindowsMapping` property (was `WindowsZones` in -rc1), which
     exposes details of the CLDR mapping between TZDB and Windows time zone
     IDs ([issue 82])
   - Added a `ZoneLocations` property, which exposes the location data from
     `zone.tab` and `iso3166.tab` ([issue 194])
   - Added a `TzdbVersion` property, which returns the TZDB version string
     (e.g. "2013a")
- Changed the means of constructing a `TzdbDateTimeZoneSource`:
   - Added a `Default` property, which provides access to the underlying source
     of TZDB data distributed with Noda Time ([issue 144])
   - Added `FromStream()`, which can be used to create a source using data in
     the new format produced by `ZoneInfoCompiler` (later renamed to `NodaTime.TzdbCompiler`)
   - Added a `Validate()` method, which allows for optional validation of
     source data (previously, this was performed on every load)
- `LocalDateTime` patterns can now parse times of the form 24:00:00,
  indicating midnight of the following day ([issue 153])
- Added convenience factory methods to `Instant`:
  `From(Ticks,Milliseconds,Seconds)SinceUnixEpoch` ([issue 142])
- Added convenience factory methods to `LocalTime`:
  `From(Ticks,Milliseconds,Seconds)SinceMidnight` ([issue 148])
- Added `LocalDateTime.InUtc()` and `WithOffset()`, convenience conversions
  from `LocalDateTime` to a (UTC) `ZonedDateTime` or `OffsetDateTime`
  ([issue 142])
- Added `Date` and `TimeOfDay` properties to `ZonedDateTime` and
  `OffsetDateTime`, which provide a `LocalDate` and `LocalTime` directly,
  rather than needing to go via the `LocalDateTime` property ([issue 186])
- Added `Period.FromMilliseconds()`, obsoleting the misnamed
  `FromMillseconds()` ([issue 149])
- Added `DateTimeZoneNotFoundException`, which is used in place of the
  .NET `TimeZoneNotFoundException` (which does not exist in the PCL);
  on desktop builds, the new type extends the latter for compatibility
- Added `InvalidNodaDataException`, which is now used to consistently
  report problems reading time zone data; this replaces (inconsistent) use
  of the .NET `InvalidDataException` type, which is also not available in
  the PCL

Newly-obsolete members:

- `DateTimeZoneProviders.Default`, which use should be replaced by
  existing equivalent property `DateTimeZoneProviders.Tzdb`
- `Period.FromMillseconds()`, which was a typo for the newly-introduced
  `Period.FromMilliseconds()`
- All `TzdbDateTimeZoneSource` constructors, which construct a source using
  TZDB data in the older resource-based format; the newer format should be
  provided to the new `TzdbDateTimeZoneSource.FromStream()` method instead
  (or, for the embedded resources, the new `TzdbDateTimeZoneSource.Default`
  property should be used directly)

API changes for NodaTime.Testing:

- Added `FakeDateTimeZoneSource`, which is exactly what it sounds like
  ([issue 83])
- Added `MultiTransitionDateTimeZone`, a time zone with multiple
  transitions, complementing the existing `SingleTransitionDateTimeZone`
- Added the `SingleTransitionDateTimeZone.Transition` property that returns
  the transition point of the fake zone
- Added an additional constructor to `SingleTransitionDateTimeZone`
  that allows the time zone ID to be specified

Bug fixes:

- Workaround for a Mono bug where abbreviated genitive month names are provided
  as "1", "2" etc ([issue 202])
- Fixed parsing issue for cultures where the AM designator is a leading
  substring of the PM designator ([issue 201])
- Not part of the main release, but `NodaIntervalConverter` now handles values
  embedded in a larger object being deserialized ([issue 191])
- Behaviour at extremes of time is now improved ([issue 197]); there's more
  work to come here in 1.2, but this is at least an improvement
- Day-of-week and month names are now parsed by choosing the longest possible
  match, allowing dates to be parsed in cultures where one string is a prefix
  of another ([issue 159])
- Fixed a minor bug that made the "Asia/Amman" time zone (Jordan) give
  incorrect values or a `NullReferenceException` when asked for a value in 2040
  (and possibly other times). Other time zones should not be affected
  ([issue 174])
- `LocalDateTime` and `ZonedDateTime` now have non-crashing behaviour when
  initialised via default (parameterless) constructors ([issue 116])

Other:

- Symbols for the release are now published to SymbolSource. See the
  [installation guide](/userguide/installation) for details of how to debug into Noda
  Time using this
- The version of SHFB used to generate the documentation has changed, along
  with some settings around which members to generate documentation for.
  (Members simply inherited from the BCL aren't documented.)

## 1.0.1, released 2012-11-13 with tzdb 2012i

No API changes; this release was purely to fix a problem
with the NodaTime.Testing package.

## 1.0.0, released 2012-11-07 with tzdb 2012i

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
  ([issue 140] and [issue 141])
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
  `TimeZoneInfo` ([issue 114], [issue 115], and [issue 122])

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
- Removed `DateTimeZone.UtcId` and `DateTimeZone.IsFixed` ([issue 64]
  and [issue 62])
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
  auto-advance option ([issue 72] and [issue 73])
- `NodaTime.Testing.TimeZones.SingleTransitionZone` renamed to
  `SingleTransitionDateTimeZone`

Bug fixes:

- Many

## 1.0.0-beta1, released 2012-04-12 with tzdb 2012c

- Initial beta release

# <a id="tzdb-updates"></a>Update policy for time zone data

New time zone data is released by IANA as required. The latest data
is always available via the Noda Time web site as described in the
[user guide](/userguide/tzdb). Additionally, NuGet packages are
updated with a patch release to include the latest data.

The latest minor release for each supported major version is always updated.
Additionally, if the latest minor release is less than 6 months old, the
previous minor release is also updated. In other words, if you check
once every 6 months to make sure you're on the latest minor release,
you should always get patch releases containing TZDB updates.

The 1.x and 2.x series of Noda Time are no longer being updated to include new versions
of TZDB. However, they are still compatible with the new NZD files: if you fetch
the NZD file from the web site (as [described in the user guide](/unstable/userguide/tzdb))
you can use the latest time zone data with old versions of Noda Time.
