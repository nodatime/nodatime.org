@Title="Core concepts"

This is a companion page to the
["core types quick reference"](core-types), and ["choosing between types"](type-choices)
pages, describing the fundamental concepts in Noda Time.

One of the benefits of Noda Time over the Base Class Library (BCL)
time support is the representation of different concepts with
different types. The downside of this is that there are more types
to learn about, and you're forced to make decisions between subtly
different concepts where in the BCL you could just use `DateTime`
and *hope* you were doing the right thing.

By forcing you to think a little bit more upfront, we're hoping
you'll gain a greater understanding not only of how your code works
but sometimes what data you're trying to represent anyway - you
should *expect* the use of Noda Time to clarify your thinking around
date and time data for your whole project.

This document introduces the core concepts, but in order to avoid it
being too overwhelming, we won't go into the fine details. See
individual pages (particularly the ["choosing between
types"](type-choices) page) and the [API documentation][api]
for more information.

"Local" and "global" (or "absolute") types
------------------------------------------

Many of the concepts can be considered as either *local* or
*global*. These terms are fairly common within other APIs and
documentation, but they can be confusing to start with. (Global
values are also called *absolute* values in [some writing][2], although
we don't use this name in Noda Time.)

The key difference is that people all around the world will agree on
a *global* value simultaneously, whereas they may all see different
*local* values for the same global value, due to time zones. A local
value has no associated time zone in Noda Time; in particular it is
*not* just "a date and time fixed to the local time of the computer
running the code" as a `DateTime` with a `Kind` of `Local` is
interpreted in .NET.

Internally, Noda Time has a concept of a `LocalInstant` which is a
local value without reference to any particular calendar system, but
this is currently not exposed. Similarly, [`LocalTime`][LocalTime] does
not refer to a calendar system - we assume that all calendars are based
around solar days 24 hours in length (give or take daylight saving
changes). However, the [`LocalDate`][LocalDate] and
[`LocalDateTime`][LocalDateTime] types *do* both refer to calendar
systems... see the later section on calendars for more information.

The global time line, and `Instant`
-----------------------------------

Noda Time assumes a single non-relativistic time line: global time
progresses linearly, and everyone in the universe would agree on the
global concept of "now". Time is measured with a granularity of
*nanoseconds*. This is 100 times finer than the granularity used in `DateTime` and
`TimeSpan` in .NET, which is the *tick*. A tick is 100 nanoseconds
in these types, but please be aware that a `Stopwatch` tick in .NET can vary based on the
timer used.

The "zero point" used everywhere in Noda Time is the Unix epoch:
midnight at the start of January 1st 1970, UTC. (UTC itself did not
exist in 1970, but that's another matter.) The Unix epoch happens
to be a common zero point used in many other systems - but if we had
used some other zero point (such a January 1st 2000 UTC) it would
simply have offset the values, and changed the maximum and minimum
representable values. It's just an origin.

The Noda Time [`Instant`][Instant] type represents a point on this
global timeline: the number of nanoseconds which have elapsed since the Unix
epoch. The value can be negative for dates and times before 1970 of
course - the range of supported dates is from around 27000 BCE to
around 31000 CE in the Gregorian calendar.

An `Instant` has no concept of a particular time zone - it is
*just* the number of nanoseconds which have occurred since
the Unix epoch. The fact that the Unix epoch is defined in terms of
UTC is irrelevant - it could have been defined in terms of a
different time zone just as easily, such as "1am on January 1st 1970
in the Europe/London time zone" (as the UK time zone was
experimentally an hour ahead of UTC at the time). The tick values
would remain the same.

Similarly, an `Instant` has no concept of a particular calendar
system - it is meaningless to ask which month an instant occurs in,
as the concept of a month (or year, etc) is only relevant within a
particular calendar system.

`Instant` is a good type to use for "when something happened" - a
timestamp in a log file, for example. The instant can then be
*interpreted* in a particular time zone and calendar system, in a
way which is useful to the person looking at the log.

Calendar systems
----------------

Humans break up time into units such as years, months, days, hours,
minutes and so on. While time itself has no such concept, it makes
life more convenient for people. Unfortunately - and this is a
recurrent theme in software - humanity hasn't done very well in
agreeing a single system to use. So there are multiple different
calendars, including Gregorian, Julian, Coptic and Buddhist. These
allow different people to talk about the same local time in
different ways - the Unix epoch occurred on December 19th 1969 in
the Julian calendar system, for example.

The calendar system you use is one of the few things Noda Time is
willing to use a default for: unless you specify otherwise, the
ISO-8601 calendar is used. If you don't know which calendar you ought
to use, this is almost certainly the one for you.

In Noda Time, the [`CalendarSystem`][CalendarSystem] type handles the
details of different calendar systems. Most of the methods are internal,
although a few useful methods are exposed. Most of the time even if you
*do* need to use a `CalendarSystem`, you can just fetch a reference to
an appropriate object, and then pass it to other constructors etc as a
little bundle of magic which simply does the right thing for you.

See the [calendars documentation](calendars) for more details about
which calendar systems are supported.

<a name="time-zones"></a>Time zones
-----------------------------------

In the most basic sense, a time zone is a mapping between [UTC][]
date/times and local date/times - or equivalently, a mapping from
UTC instants to *offsets*, where an offset is simply the difference
between UTC and local time. In Noda Time, time zones are represented
by the [`DateTimeZone`][DateTimeZone] class, and offsets are represented
by the [`Offset`][Offset] struct.

An offset is positive if local time is later than (ahead of) UTC,
and negative if local time is earlier than (behind) UTC. For
example, the offset in France is +1 hour during winter in the
Northern Hemisphere and +2 hours in
the summer; the offset in California is -8 hours in the winter and
-7 hours in the summer. So at noon UTC in winter, it's 4am in
California and 1pm in France.

As well as mapping any particular instant to an offset,
`DateTimeZone` allows you to find out the name of the part of the
time zone for that instant, as well as when the next or previous
change occurs - usually for daylight saving changes.

Most of the time when you use a `DateTimeZone` you won't need
worry about that - the main purpose is usually to convert between a
[`ZonedDateTime`][ZonedDateTime] and a [`LocalDateTime`][LocalDateTime], 
where the names mean exactly what you expect them to. There's a slight
twist to this: converting from an `Instant` or a `ZonedDateTime` to a
`LocalDateTime` is unambiguous; at any point in time, all the (accurate)
clocks in a particular time zone will show the same time... but the
reverse isn't true. Any one local time can map to:

- A single instant in time: this is the case for almost all the time.
- Two instants in time: this occurs around a time zone transition
which goes from one offset to an earlier one, e.g. turning clocks
back in the fall. If the clocks go back at 2am local time to 1am
local time, then 1.30am occurs twice... so you need to tell Noda
Time which of the possibilities you want to account for.
- Zero instants in time: this occurs around a time zone transition
which goes from one offset to a later one, e.g. turning clocks
forward in the spring. If the clocks go forward at 1am local time to
2am local time, then 1.30am doesn't occur at all.

Noda Time makes it *reasonably* easy to handle these situations, but
you need to work out what you want to happen. See the [`DateTimeZone`
documentation][DateTimeZone] for more details and options.

There are various different sources of time zone information available, and
Noda Time handles two of them: it is able to map BCL `TimeZoneInfo` objects
using `BclDateTimeZone`, or it can use the [tz database][TZDB] (also known as
the IANA Time Zone database, or zoneinfo or Olson database). A version of TZDB
is embedded within the Noda Time distribution, and if you need a more recent
one, there are [instructions on how to download and use new data](tzdb).
We generally recommend that you isolate yourself from the provider you're using
by only depending on [`IDateTimeZoneProvider`][IDateTimeZoneProvider], and
injecting the appropriate provider in the normal way. "Stock" providers are
available via the [`DateTimeZoneProviders`][DateTimeZoneProviders] class.

Also note that in some cases, you may not have full time zone information,
but have just a local time and an offset. For example, if you're parsing the string
"2012-06-26T20:41:00+01:00" that gives the information that the local time is one hour
ahead of UTC - but it doesn't give any indication of what the offset would be at any other
time. In this situation, you should use [`OffsetDateTime`][OffsetDateTime].

Periods and durations
---------------------

There are two similar types in Noda Time used to represent "lengths of time". The
simplest is [`Duration`][Duration] which is equivalent to [`TimeSpan`][TimeSpan] in the BCL,
other than in terms of granularity. This is a fixed number of nanoseconds - it's
the same length of time wherever it's applied. `Duration` is used for arithmetic on `Instant`
and `ZonedDateTime` values.

A more complex type is [`Period`][Period], which is a set of values associated with different
calendar-based periods: years, months, weeks, days, hours, minutes and so on. Some of
these periods represent different lengths of time depending on what they're applied
to - if you add "one month" to January 1st, that's going to be 31 days long. Adding the
same period to February 1st will give a shorter length of time - which then depends
on whether the year is a leap year or not. Periods based on smaller units (hours, minutes
and so on) will always represent the same length of time, but they're still
available within periods. `Period` is used for arithmetic on locally-based values
(`LocalDateTime`, `LocalDate`, `LocalTime`).

See the [arithmetic](arithmetic) page for more information.

[api]: ../api/
[2]: https://blogs.msdn.microsoft.com/bclteam/2007/06/18/a-brief-history-of-datetime-anthony-moore/
[LocalTime]: noda-type://NodaTime.LocalTime
[LocalDate]: noda-type://NodaTime.LocalDate
[LocalDateTime]: noda-type://NodaTime.LocalDateTime
[Instant]: noda-type://NodaTime.Instant
[CalendarSystem]: noda-type://NodaTime.CalendarSystem
[UTC]: https://en.wikipedia.org/wiki/Coordinated_Universal_Time
[DateTimeZone]: noda-type://NodaTime.DateTimeZone
[Offset]: noda-type://NodaTime.Offset
[Period]: noda-type://NodaTime.Period
[Duration]: noda-type://NodaTime.Duration
[OffsetDateTime]: noda-type://NodaTime.OffsetDateTime
[ZonedDateTime]: noda-type://NodaTime.ZonedDateTime
[TZDB]: https://www.iana.org/time-zones
[IDateTimeZoneProvider]: noda-type://NodaTime.IDateTimeZoneProvider
[DateTimeZoneProviders]: noda-type://NodaTime.DateTimeZoneProviders
[TimeSpan]: https://msdn.microsoft.com/en-us/library/system.timespan.aspx
