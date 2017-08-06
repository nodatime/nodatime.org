@Title="BCL conversions"

Various Noda Time types have "broadly similar" types in the .NET
framework's Base Class Library (BCL). Where appropriate,
conversions are provided - we have no illusions that you'll be able
to use Noda Time for *everything*. Noda Time attempts to shield you
from using "the wrong kind of `DateTime`"

All BCL type conversions to Noda Time types which have implicit calendar systems (`LocalDateTime` etc) use
the ISO-8601 calendar.

DateTime
========

`DateTime` can represent many things (which is [one of the reasons](http://blog.nodatime.org/2011/08/what-wrong-with-datetime-anyway.html) Noda Time exists).

However, the following mappings are reasonable:

<table>
  <thead>
    <tr>
      <td>Noda Time type</td>
      <td>DateTime kind</td>
      <td>Conversions</td>
      <td>Notes</td>
    </tr>
  </thead>
  <tbody>
    <tr>
	  <td>Instant</td>
	  <td>Utc</td>
	  <td>Instant.ToDateTimeUtc<br />
	    Instant.FromDateTimeUtc</td>
	  <td></td>
    </tr>
    <tr>
	  <td>ZonedDateTime</td>
	  <td>Universal</td>
	  <td>ZonedDateTime.ToDateTimeUtc</td>
	  <td>This preserves the instant, but loses the time zone information</td>
    </tr>
    <tr>
	  <td>ZonedDateTime</td>
	  <td>Unspecified</td>
	  <td>ZonedDateTime.ToDateTimeUnspecified</td>
	  <td>This preserves the local time, but loses the time zone information</td>
    </tr>
    <tr>
	  <td>LocalDateTime</td>
	  <td>Unspecified</td>
	  <td>LocalDateTime.ToDateTimeUnspecified<br />
	      LocalDateTime.FromDateTime</td>
	  <td>FromDateTime uses the "local" value of the DateTime regardless of kind</td>
    </tr>
    <tr>
      <td>OffsetDateTime</td>
      <td>Unspecified</td>
      <td>OffsetDateTime.ToDateTimeOffset<br />
      OffsetDateTime.FromDateTimeOffset</td>
      <td>FromDateTimeOffset uses the "local" value of the DateTime regardless of kind</td>
    </tr>
  </tbody>
</table>

Note that there are no conversions to a `DateTime` with a kind of `Local` - this would effectively
be for the system default time zone, which you should generally be explicit about to start with.

DateTimeOffset
==============

[`OffsetDateTime`](noda-type://NodaTime.OffsetDateTime) corresponds most closely to `DateTimeOffset`, although you can also use a [`ZonedDateTime`](noda-type://NodaTime.ZonedDateTime) with a fixed time zone. That's exactly what `ZonedDateTime.FromDateTimeOffset` does,
but you must be aware that "real" time zone information is lost as soon as you've got a `DateTimeOffset` -
it represents an exact instant in time, with a local offset from UTC, but that doesn't tell you what the
local offset would be a minute later or earlier. The reverse conversion (`ZonedDateTime.ToDateTimeOffset`)
loses the time zone information in a similar way.

`Instant` also provides conversions to and from `DateTimeOffset`; `ToDateTimeOffset` will always return a
`DateTimeOffset` with an offset of zero, and `FromDateTimeOffset` will "subtract" the offset from local time,
to represent the appropriate instant in time - but without any further trace of the offset, which isn't stored in an `Instant`.

TimeSpan
========

Both [`Offset`](noda-type://NodaTime.Offset) and [`Duration`](noda-type://NodaTime.Duration) are similar to `TimeSpan`,
but they're used in different senses; `Offset` is used to indicate the difference between UTC and local time, whereas
a `Duration` is simply an arbitrary number of ticks.

Both types have `ToTimeSpan` and `FromTimeSpan` methods, although `Offset.FromTimeSpan` will throw an `ArgumentOutOfRangeException`
if the `TimeSpan` has a magnitude of 24 hours or more.

TimeZoneInfo
============

The main time zone type in Noda Time is [`DateTimeZone`](noda-type://NodaTime.DateTimeZone), which the default provider
creates from the zoneinfo time zone database. However, if you want to create a
`DateTimeZone` which corresponds exactly to a particular `TimeZoneInfo`,
there are some options using [`BclDateTimeZone`](noda-type://NodaTime.TimeZones.BclDateTimeZone):

- You can use `DateTimeZoneProviders.Bcl` everywhere you create time zones. (You may well want to inject this as an [`IDateTimeZoneProvider`](noda-type://NodaTime.IDateTimeZoneProvider)
  if you're using dependency injection). This is appropriate if you're going to work with various time zones,
  and you only ever care about the BCL versions.
- To convert a single time zone, you can use `BclDateTimeZone.FromTimeZoneInfo`.
- If you just need the system default time zone, you can call
  `BclDateTimeZone.ForSystemDefault`. There are some (rare) circumstances where
  using `DateTimeZoneProviders.Tzdb.GetSystemDefault` may throw an exception,
  indicating that there's no known mapping from the local BCL time zone ID to
  TZDB. Using `BclDateTimeZone.ForSystemDefault()` *always* returns a converted
  version of the BCL local time zone.

There are various pros and cons involved in using the zoneinfo time
zones vs the BCL ones. In particular:

- If you need to interoperate with non-Windows systems, they're
  likely to use the zoneinfo IDs
- If you need to interoperate with Windows systems, they're likely
  to use the Windows IDs
- zoneinfo provides more historical information
- If you're running Noda Time under Windows, changes to BCL time zone
  information will become available automatically
- Using the zoneinfo database allows you to decide exactly when you
  update your time zone information (e.g. if you need to check that
  all the zones still have the same IDs, or even to find zones which
  have changed in a meaningful way for your data)

DayOfWeek
=========

For every day other than Sunday, `DayOfWeek` and
[`IsoDayOfWeek`](noda-type://NodaTime.IsoDayOfWeek) have the same
value. However, `DayOfWeek` uses 0 for Sunday, and `IsoDayOfWeek`
uses 7 (as per ISO-8601). Converting between the two isn't
difficult, but there are utility methods in
[`BclConversions`](noda-type://NodaTime.Utility.BclConversions) to
make things slightly smoother:

```csharp
DayOfWeek bcl = BclConversions.ToDayOfWeek(IsoDayOfWeek.Wednesday);
IsoDayOfWeek iso = BclConversions.ToIsoDayOfWeek(DayOfWeek.Wednesday);
```

Any others?
===========

If you have other requirements around BCL conversions, please ask on
the [mailing list](https://groups.google.com/group/noda-time).
