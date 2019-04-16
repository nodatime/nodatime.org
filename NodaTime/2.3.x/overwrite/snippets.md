---
uid: NodaTime.DateInterval.#ctor(NodaTime.LocalDate,NodaTime.LocalDate)
snippet: *content
---

```csharp
using NodaTime;
using System;

var calendar = CalendarSystem.Gregorian;
LocalDate start = new LocalDate(2017, 1, 1, calendar);
LocalDate end = new LocalDate(2017, 12, 31, calendar);

DateInterval interval = new DateInterval(start, end);

Console.WriteLine(interval.Length);
Console.WriteLine(interval.ToString());
Console.WriteLine(interval.Start);
Console.WriteLine(interval.End);
Console.WriteLine(interval.Calendar);
```

Output:

```text
365
[2017-01-01, 2017-12-31]
Sunday, 01 January 2017
Sunday, 31 December 2017
Gregorian

```

---
uid: NodaTime.DateInterval.Intersection(NodaTime.DateInterval)
snippet: *content
---

```csharp
using NodaTime;
using System;

DateInterval januaryToAugust = new DateInterval(
    new LocalDate(2017, 1, 1),
    new LocalDate(2017, 8, 31));

DateInterval juneToNovember = new DateInterval(
    new LocalDate(2017, 6, 1),
    new LocalDate(2017, 11, 30));

DateInterval juneToAugust = new DateInterval(
    new LocalDate(2017, 6, 1),
    new LocalDate(2017, 8, 31));

var result = januaryToAugust.Intersection(juneToNovember);
Console.WriteLine(result);
```

Output:

```text
[2017-06-01, 2017-08-31]

```

---
uid: NodaTime.DateInterval.Contains(NodaTime.LocalDate)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate start = new LocalDate(2017, 1, 1);
LocalDate end = new LocalDate(2017, 12, 31);

DateInterval interval = new DateInterval(start, end);

var result = interval.Contains(new LocalDate(2017, 12, 5));
Console.WriteLine(result);
```

Output:

```text
True

```

---
uid: NodaTime.DateInterval.Contains(NodaTime.DateInterval)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate start = new LocalDate(2017, 1, 1);
LocalDate end = new LocalDate(2017, 12, 31);

DateInterval interval = new DateInterval(start, end);
DateInterval june = new DateInterval(
    new LocalDate(2017, 6, 1),
    new LocalDate(2017, 6, 30));

var result = interval.Contains(june);
Console.WriteLine(result);
```

Output:

```text
True

```

---
uid: NodaTime.Duration.FromDays(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

Duration duration = Duration.FromDays(5);
Console.WriteLine(duration.Days);
Console.WriteLine(duration.ToString());
```

Output:

```text
5
5:00:00:00

```

---
uid: NodaTime.Duration.FromHours(System.Double)
snippet: *content
---

```csharp
using NodaTime;
using System;

Duration duration = Duration.FromHours(1.5);
Console.WriteLine(duration.Hours);
Console.WriteLine(duration.TotalHours);
Console.WriteLine(duration.TotalMinutes);
Console.WriteLine(duration.ToString());
```

Output:

```text
1
1.5
90
0:01:30:00

```

---
uid: NodaTime.Duration.FromMinutes(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Duration duration = Duration.FromMinutes(50);
Console.WriteLine(duration.Minutes);
Console.WriteLine(duration.ToString());
```

Output:

```text
50
0:00:50:00

```

---
uid: NodaTime.Duration.FromSeconds(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Duration duration = Duration.FromSeconds(42);
Console.WriteLine(duration.Seconds);
Console.WriteLine(duration.ToString());
```

Output:

```text
42
0:00:00:42

```

---
uid: NodaTime.Duration.FromMilliseconds(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Duration duration = Duration.FromMilliseconds(600);
Console.WriteLine(duration.Milliseconds);
Console.WriteLine(duration.TotalSeconds);
Console.WriteLine(duration.ToString());
```

Output:

```text
600
0.6
0:00:00:00.6

```

---
uid: NodaTime.Duration.FromTicks(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Duration duration = Duration.FromTicks(10_000_000);
Console.WriteLine(duration.TotalTicks);
Console.WriteLine(duration.TotalSeconds);
Console.WriteLine(duration.ToString());
```

Output:

```text
10000000
1
0:00:00:01

```

---
uid: NodaTime.Duration.FromNanoseconds(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Duration duration = Duration.FromNanoseconds(1_000_000_000);
Console.WriteLine(duration.TotalNanoseconds);
Console.WriteLine(duration.TotalSeconds);
Console.WriteLine(duration.ToString());
```

Output:

```text
1000000000
1
0:00:00:01

```

---
uid: NodaTime.Duration.FromTimeSpan(System.TimeSpan)
snippet: *content
---

```csharp
using NodaTime;
using System;

Duration duration = Duration.FromTimeSpan(TimeSpan.FromHours(3));
Console.WriteLine(duration.Hours);
Console.WriteLine(duration.ToString());
```

Output:

```text
3
0:03:00:00

```

---
uid: NodaTime.LocalDate.#ctor(System.Int32,System.Int32,System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;
using System.Globalization;

LocalDate date = new LocalDate(2010, 6, 16);
Console.WriteLine(date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
Console.WriteLine(date.Year);
Console.WriteLine(date.Month);
Console.WriteLine(date.Day);
```

Output:

```text
2010-06-16
2010
6
16

```

---
uid: NodaTime.LocalDate.#ctor(System.Int32,System.Int32,System.Int32,NodaTime.CalendarSystem)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16, CalendarSystem.Iso);
Console.WriteLine(date);
```

Output:

```text
Wednesday, 16 June 2010

```

---
uid: NodaTime.LocalDate.op_Addition(NodaTime.LocalDate,NodaTime.LocalTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16);
LocalTime time = new LocalTime(16, 20);
LocalDateTime dateTime = date + time;
Console.WriteLine(dateTime);
```

Output:

```text
06/16/2010 16:20:00

```

---
uid: NodaTime.LocalDate.At(NodaTime.LocalTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16);
LocalTime time = new LocalTime(16, 20);
LocalDateTime dateTime = date.At(time);
Console.WriteLine(dateTime);
```

Output:

```text
06/16/2010 16:20:00

```

---
uid: NodaTime.LocalDateTime.#ctor(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,NodaTime.CalendarSystem)
snippet: *content
---

```csharp
using NodaTime;
using System;

CalendarSystem calendar = CalendarSystem.Iso;
LocalDateTime dt = new LocalDateTime(2010, 6, 16, 16, 20, calendar);
Console.WriteLine(dt.Minute);
```

Output:

```text
20

```

---
uid: NodaTime.LocalDateTime.#ctor(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.Text;
using System;

LocalDateTime dt = new LocalDateTime(2010, 6, 16, 16, 20);
Console.WriteLine(LocalDateTimePattern.GeneralIso.Format(dt));
Console.WriteLine(dt.Calendar);
```

Output:

```text
2010-06-16T16:20:00
ISO

```

---
uid: NodaTime.LocalDateTime.ToString(System.String,System.IFormatProvider)
snippet: *content
---

```csharp
using NodaTime;
using System;
using System.Globalization;

LocalDateTime dt = new LocalDateTime(2010, 6, 16, 16, 20);
Console.WriteLine(dt.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture));
```

Output:

```text
2010-06-16T16:20:00

```

---
uid: NodaTime.LocalTime.#ctor(System.Int32,System.Int32,System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;
using System.Globalization;

LocalTime time = new LocalTime(16, 20, 0);
Console.WriteLine(time.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
```

Output:

```text
16:20:00

```

---
uid: NodaTime.Offset.FromHours(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

Offset offset = Offset.FromHours(1);
Console.WriteLine(offset.Seconds);
```

Output:

```text
3600

```

---
uid: NodaTime.Offset.FromHoursAndMinutes(System.Int32,System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

Offset offset = Offset.FromHoursAndMinutes(1, 1);
Console.WriteLine(offset.Seconds);
```

Output:

```text
3660

```

---
uid: NodaTime.Offset.FromSeconds(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

Offset offset = Offset.FromSeconds(450);
Console.WriteLine(offset.Seconds);
```

Output:

```text
450

```

---
uid: NodaTime.Offset.FromMilliseconds(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

Offset offset = Offset.FromMilliseconds(1200);
Console.WriteLine(offset.Seconds);
Console.WriteLine(offset.Milliseconds);
```

Output:

```text
1
1000

```

---
uid: NodaTime.Offset.FromTicks(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Offset offset = Offset.FromTicks(15_000_000);
Console.WriteLine(offset.Ticks);
Console.WriteLine(offset.Seconds);
```

Output:

```text
10000000
1

```

---
uid: NodaTime.Offset.FromNanoseconds(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Offset offset = Offset.FromNanoseconds(1_200_000_000);
Console.WriteLine(offset.Seconds);
Console.WriteLine(offset.Nanoseconds);
```

Output:

```text
1
1000000000

```

---
uid: NodaTime.Offset.FromTimeSpan(System.TimeSpan)
snippet: *content
---

```csharp
using NodaTime;
using System;

var timespan = TimeSpan.FromHours(1.5);
Offset offset = Offset.FromTimeSpan(timespan);
Console.WriteLine(offset.Seconds);
```

Output:

```text
5400

```

---
uid: NodaTime.Offset.Plus(NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

var offset = Offset.FromSeconds(100);
var offset2 = Offset.FromSeconds(150);
var expected = Offset.FromSeconds(250);

var actual = offset.Plus(offset2);

Console.WriteLine(actual);
```

Output:

```text
+00:04:10

```

---
uid: NodaTime.Offset.Minus(NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

var offset = Offset.FromSeconds(100);
var offset2 = Offset.FromSeconds(120);
var expected = Offset.FromSeconds(-20);

var actual = offset.Minus(offset2);

Console.WriteLine(actual);
```

Output:

```text
-00:00:20

```

---
uid: NodaTime.Offset.ToTimeSpan
snippet: *content
---

```csharp
using NodaTime;
using System;

var offset = Offset.FromSeconds(120);
var actual = offset.ToTimeSpan();
var expected = TimeSpan.FromSeconds(120);

Console.WriteLine(actual);
```

Output:

```text
00:02:00

```

---
uid: NodaTime.Period.FromYears(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.FromYears(27);
Console.WriteLine(period.Years);
Console.WriteLine(period.ToString());
```

Output:

```text
27
P27Y

```

---
uid: NodaTime.Period.FromMonths(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.FromMonths(10);
Console.WriteLine(period.Months);
Console.WriteLine(period.ToString());
```

Output:

```text
10
P10M

```

---
uid: NodaTime.Period.FromWeeks(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.FromWeeks(1);
Console.WriteLine(period.Weeks);
Console.WriteLine(period.ToString());
```

Output:

```text
1
P1W

```

---
uid: NodaTime.Period.FromDays(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.FromDays(3);
Console.WriteLine(period.Days);
Console.WriteLine(period.ToString());
```

Output:

```text
3
P3D

```

---
uid: NodaTime.Period.FromHours(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.FromHours(5);
Console.WriteLine(period.Hours);
Console.WriteLine(period.ToString());
```

Output:

```text
5
PT5H

```

---
uid: NodaTime.Period.FromMinutes(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.FromMinutes(15);
Console.WriteLine(period.Minutes);
Console.WriteLine(period.ToString());
```

Output:

```text
15
PT15M

```

---
uid: NodaTime.Period.FromSeconds(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.FromSeconds(70);
Console.WriteLine(period.Seconds);
Console.WriteLine(period.ToString());
```

Output:

```text
70
PT70S

```

---
uid: NodaTime.Period.FromMilliseconds(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.FromMilliseconds(1500);
Console.WriteLine(period.Milliseconds);
Console.WriteLine(period.ToString());
```

Output:

```text
1500
PT1500s

```

---
uid: NodaTime.Period.FromTicks(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.FromTicks(42);
Console.WriteLine(period.Ticks);
Console.WriteLine(period.ToString());
```

Output:

```text
42
PT42t

```

---
uid: NodaTime.Period.FromNanoseconds(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.FromNanoseconds(42);
Console.WriteLine(period.Nanoseconds);
Console.WriteLine(period.ToString());
```

Output:

```text
42
PT42n

```

---
uid: NodaTime.Period.Between(NodaTime.LocalDate,NodaTime.LocalDate)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.Between(
    new LocalDate(2017, 11, 10),
    new LocalDate(2017, 11, 15));

Console.WriteLine(period.Days);
Console.WriteLine(period.ToString());
```

Output:

```text
5
P5D

```

---
uid: NodaTime.Period.Between(NodaTime.LocalDate,NodaTime.LocalDate)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.Between(
    new LocalDate(1990, 6, 26),
    new LocalDate(2017, 11, 15));

Console.WriteLine(period.Years);
Console.WriteLine(period.Months);
Console.WriteLine(period.Days);
Console.WriteLine(period.ToString());
```

Output:

```text
27
4
20
P27Y4M20D

```

---
uid: NodaTime.Period.Between(NodaTime.LocalDate,NodaTime.LocalDate,NodaTime.PeriodUnits)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.Between(
    new LocalDate(2016, 11, 14),
    new LocalDate(2017, 11, 21),
    PeriodUnits.Years | PeriodUnits.Days);

Console.WriteLine(period.Years);
Console.WriteLine(period.Days);
Console.WriteLine(period.ToString());
```

Output:

```text
1
7
P1Y7D

```

---
uid: NodaTime.Period.Between(NodaTime.LocalTime,NodaTime.LocalTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.Between(new LocalTime(10, 10), new LocalTime(13, 15));

Console.WriteLine(period.Hours);
Console.WriteLine(period.Minutes);
Console.WriteLine(period.ToString());
```

Output:

```text
3
5
PT3H5M

```

---
uid: NodaTime.Period.Between(NodaTime.LocalTime,NodaTime.LocalTime,NodaTime.PeriodUnits)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.Between(
    new LocalTime(10, 10, 2),
    new LocalTime(13, 15, 49),
    PeriodUnits.Hours | PeriodUnits.Seconds);

Console.WriteLine(period.Hours);
Console.WriteLine(period.Seconds);
Console.WriteLine(period.ToString());
```

Output:

```text
3
347
PT3H347S

```

---
uid: NodaTime.Period.Between(NodaTime.LocalDateTime,NodaTime.LocalDateTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.Between(
    new LocalDateTime(2015, 1, 23, 21, 30, 15),
    new LocalDateTime(2017, 10, 15, 21, 02, 17));

Console.WriteLine(period.Years);
Console.WriteLine(period.Months);
Console.WriteLine(period.Days);
Console.WriteLine(period.Hours);
Console.WriteLine(period.Minutes);
Console.WriteLine(period.Seconds);
Console.WriteLine(period.ToString());
```

Output:

```text
2
8
21
23
32
2
P2Y8M21DT23H32M2S

```

---
uid: NodaTime.Period.Between(NodaTime.LocalDateTime,NodaTime.LocalDateTime,NodaTime.PeriodUnits)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period period = Period.Between(
    new LocalDateTime(2015, 1, 23, 21, 30, 15),
    new LocalDateTime(2017, 10, 15, 21, 02, 17),
    PeriodUnits.Years | PeriodUnits.Days | PeriodUnits.Hours);

Console.WriteLine(period.Years);
Console.WriteLine(period.Days);
Console.WriteLine(period.Hours);
Console.WriteLine(period.ToString());
```

Output:

```text
2
264
23
P2Y264DT23H

```

---
uid: NodaTime.DateTimeZone.GetUtcOffset(NodaTime.Instant)
snippet: *content
---

```csharp
using NodaTime;
using System;

// Yes, in 1900 Paris did (according to TZDB) have a UTC offset of 9 minutes, 21 seconds.
DateTimeZone paris = DateTimeZoneProviders.Tzdb["Europe/Paris"];
Offset offset = paris.GetUtcOffset(Instant.FromUtc(1900, 1, 1, 0, 0));
Console.WriteLine(offset.ToString());
```

Output:

```text
+00:09:21

```

---
uid: NodaTime.DateTimeZone.GetZoneInterval(NodaTime.Instant)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.TimeZones;
using System;

DateTimeZone london = DateTimeZoneProviders.Tzdb["Europe/London"];
ZoneInterval interval = london.GetZoneInterval(Instant.FromUtc(2010, 6, 19, 0, 0));
Console.WriteLine(interval.Name);
Console.WriteLine(interval.Start);
Console.WriteLine(interval.End);
Console.WriteLine(interval.WallOffset);
Console.WriteLine(interval.Savings);
```

Output:

```text
BST
2010-03-28T01:00:00Z
2010-10-31T01:00:00Z
+01
+01

```

---
uid: NodaTime.DateTimeZone.AtStrictly(NodaTime.LocalDateTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];
ZonedDateTime dt = dublin.AtStrictly(new LocalDateTime(2010, 6, 9, 15, 15, 0));

Console.WriteLine(dt.Hour);
Console.WriteLine(dt.Year);

Instant instant = Instant.FromUtc(2010, 6, 9, 14, 15, 0);
Console.WriteLine(dt.ToInstant());
```

Output:

```text
15
2010
2010-06-09T14:15:00Z

```

---
uid: NodaTime.ZonedDateTime.TickOfDay
snippet: *content
---

```csharp
using NodaTime;
using System;

// This is a 25-hour day at the end of daylight saving time
var dt = new LocalDate(2017, 10, 29);
var time = new LocalTime(23, 59, 59);
var dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];

var startOfDay = dublin.AtStartOfDay(dt);
ZonedDateTime nearEndOfDay = dublin.AtStrictly(dt + time);
Console.WriteLine(nearEndOfDay.TickOfDay);

Duration duration = nearEndOfDay - startOfDay;
Console.WriteLine(duration);
```

Output:

```text
863990000000
1:00:59:59

```

---
uid: NodaTime.ZonedDateTime.IsDaylightSavingTime
snippet: *content
---

```csharp
using NodaTime;
using System;

// Europe/Dublin transitions from UTC+1 to UTC+0 at 2am (local) on 2017-10-29
var dt = new LocalDateTime(2017, 10, 29, 1, 45, 0);
DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];

ZonedDateTime beforeTransition = new ZonedDateTime(dt, dublin, Offset.FromHours(1));
Console.WriteLine(beforeTransition.IsDaylightSavingTime());

// Same local time, different offset - so a different instant, after the transition.
ZonedDateTime afterTransition = new ZonedDateTime(dt, dublin, Offset.FromHours(0));
Console.WriteLine(afterTransition.IsDaylightSavingTime());
```

Output:

```text
False
True

```

---
uid: NodaTime.ZonedDateTime.IsDaylightSavingTime
snippet: *content
---

```csharp
using NodaTime;
using System;

// Europe/Dublin transitions from UTC+1 to UTC+0 at 2am (local) on 2017-10-29
var dt = new LocalDateTime(2017, 10, 29, 1, 45, 0);
DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];

ZonedDateTime beforeTransition = new ZonedDateTime(dt, dublin, Offset.FromHours(1));
Console.WriteLine(beforeTransition.IsDaylightSavingTime());

// Same local time, different offset - so a different instant, after the transition.
ZonedDateTime afterTransition = new ZonedDateTime(dt, dublin, Offset.FromHours(0));
Console.WriteLine(afterTransition.IsDaylightSavingTime());
```

Output:

```text
False
True

```

---
uid: NodaTime.ZonedDateTime.Add(NodaTime.ZonedDateTime,NodaTime.Duration)
snippet: *content
---

```csharp
using NodaTime;
using System;

// Europe/Dublin transitions from UTC+1 to UTC+0 at 2am (local) on 2017-10-29
var dt = new LocalDateTime(2017, 10, 29, 1, 45, 0);
DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];

ZonedDateTime beforeTransition = new ZonedDateTime(dt, dublin, Offset.FromHours(1));

var result = ZonedDateTime.Add(beforeTransition, Duration.FromHours(1));
Console.WriteLine(result.Date);
// Adding an hour of elapsed time takes us across the DST transition, so we have
// the same local time (shown on a clock) but a different offset.
Console.WriteLine(result);

// The + operator and Plus instance method are equivalent to the Add static method.
var result2 = beforeTransition + Duration.FromHours(1);
var result3 = beforeTransition.Plus(Duration.FromHours(1));
Console.WriteLine(result2);
Console.WriteLine(result3);
```

Output:

```text
Sunday, 29 October 2017
2017-10-29T01:45:00 Europe/Dublin (+00)
2017-10-29T01:45:00 Europe/Dublin (+00)
2017-10-29T01:45:00 Europe/Dublin (+00)

```

---
uid: NodaTime.ZonedDateTime.op_Addition(NodaTime.ZonedDateTime,NodaTime.Duration)
snippet: *content
---

```csharp
using NodaTime;
using System;

// Europe/Dublin transitions from UTC+1 to UTC+0 at 2am (local) on 2017-10-29
var dt = new LocalDateTime(2017, 10, 29, 1, 45, 0);
DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];

ZonedDateTime beforeTransition = new ZonedDateTime(dt, dublin, Offset.FromHours(1));

var result = ZonedDateTime.Add(beforeTransition, Duration.FromHours(1));
Console.WriteLine(result.Date);
// Adding an hour of elapsed time takes us across the DST transition, so we have
// the same local time (shown on a clock) but a different offset.
Console.WriteLine(result);

// The + operator and Plus instance method are equivalent to the Add static method.
var result2 = beforeTransition + Duration.FromHours(1);
var result3 = beforeTransition.Plus(Duration.FromHours(1));
Console.WriteLine(result2);
Console.WriteLine(result3);
```

Output:

```text
Sunday, 29 October 2017
2017-10-29T01:45:00 Europe/Dublin (+00)
2017-10-29T01:45:00 Europe/Dublin (+00)
2017-10-29T01:45:00 Europe/Dublin (+00)

```

---
uid: NodaTime.ZonedDateTime.Plus(NodaTime.Duration)
snippet: *content
---

```csharp
using NodaTime;
using System;

// Europe/Dublin transitions from UTC+1 to UTC+0 at 2am (local) on 2017-10-29
var dt = new LocalDateTime(2017, 10, 29, 1, 45, 0);
DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];

ZonedDateTime beforeTransition = new ZonedDateTime(dt, dublin, Offset.FromHours(1));

var result = ZonedDateTime.Add(beforeTransition, Duration.FromHours(1));
Console.WriteLine(result.Date);
// Adding an hour of elapsed time takes us across the DST transition, so we have
// the same local time (shown on a clock) but a different offset.
Console.WriteLine(result);

// The + operator and Plus instance method are equivalent to the Add static method.
var result2 = beforeTransition + Duration.FromHours(1);
var result3 = beforeTransition.Plus(Duration.FromHours(1));
Console.WriteLine(result2);
Console.WriteLine(result3);
```

Output:

```text
Sunday, 29 October 2017
2017-10-29T01:45:00 Europe/Dublin (+00)
2017-10-29T01:45:00 Europe/Dublin (+00)
2017-10-29T01:45:00 Europe/Dublin (+00)

```

---
uid: NodaTime.ZonedDateTime.PlusHours(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.Text;
using System;

DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];
var start = Instant.FromUtc(2017, 7, 20, 5, 30);
// Dublin is at UTC+1 in July 2017, so this is 6:30am.
ZonedDateTime zoned = new ZonedDateTime(start, dublin);
var pattern = ZonedDateTimePattern.ExtendedFormatOnlyIso;
Console.WriteLine(    pattern.Format(zoned.PlusHours(1)));
```

Output:

```text
2017-07-20T07:30:00 Europe/Dublin (+01)

```

---
uid: NodaTime.ZonedDateTime.PlusMinutes(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.Text;
using System;

DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];
var start = Instant.FromUtc(2017, 7, 20, 5, 30);
// Dublin is at UTC+1 in July 2017, so this is 6:30am.
ZonedDateTime zoned = new ZonedDateTime(start, dublin);
var pattern = ZonedDateTimePattern.ExtendedFormatOnlyIso;
Console.WriteLine(    pattern.Format(zoned.PlusMinutes(1)));
```

Output:

```text
2017-07-20T06:31:00 Europe/Dublin (+01)

```

---
uid: NodaTime.ZonedDateTime.PlusSeconds(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.Text;
using System;

DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];
var start = Instant.FromUtc(2017, 7, 20, 5, 30);
// Dublin is at UTC+1 in July 2017, so this is 6:30am.
ZonedDateTime zoned = new ZonedDateTime(start, dublin);
var pattern = ZonedDateTimePattern.ExtendedFormatOnlyIso;
Console.WriteLine(    pattern.Format(zoned.PlusSeconds(1)));
```

Output:

```text
2017-07-20T06:30:01 Europe/Dublin (+01)

```

---
uid: NodaTime.ZonedDateTime.PlusMilliseconds(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.Text;
using System;

DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];
var start = Instant.FromUtc(2017, 7, 20, 5, 30);
// Dublin is at UTC+1 in July 2017, so this is 6:30am.
ZonedDateTime zoned = new ZonedDateTime(start, dublin);
var pattern = ZonedDateTimePattern.ExtendedFormatOnlyIso;
Console.WriteLine(    pattern.Format(zoned.PlusMilliseconds(1)));
```

Output:

```text
2017-07-20T06:30:00.001 Europe/Dublin (+01)

```

---
uid: NodaTime.ZonedDateTime.PlusTicks(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.Text;
using System;

DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];
var start = Instant.FromUtc(2017, 7, 20, 5, 30);
// Dublin is at UTC+1 in July 2017, so this is 6:30am.
ZonedDateTime zoned = new ZonedDateTime(start, dublin);
var pattern = ZonedDateTimePattern.ExtendedFormatOnlyIso;
Console.WriteLine(    pattern.Format(zoned.PlusTicks(1)));
```

Output:

```text
2017-07-20T06:30:00.0000001 Europe/Dublin (+01)

```

---
uid: NodaTime.ZonedDateTime.PlusNanoseconds(System.Int64)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.Text;
using System;

DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];
var start = Instant.FromUtc(2017, 7, 20, 5, 30);
// Dublin is at UTC+1 in July 2017, so this is 6:30am.
ZonedDateTime zoned = new ZonedDateTime(start, dublin);
var pattern = ZonedDateTimePattern.ExtendedFormatOnlyIso;
Console.WriteLine(    pattern.Format(zoned.PlusNanoseconds(1)));
```

Output:

```text
2017-07-20T06:30:00.000000001 Europe/Dublin (+01)

```

---
uid: NodaTime.ZonedDateTime.Subtract(NodaTime.ZonedDateTime,NodaTime.Duration)
snippet: *content
---

```csharp
using NodaTime;
using System;

// Europe/Dublin transitions from UTC+1 to UTC+0 at 2am (local) on 2017-10-29
var dt = new LocalDateTime(2017, 10, 29, 1, 45, 0);
DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];

ZonedDateTime afterTransition = new ZonedDateTime(dt, dublin, Offset.FromHours(0));

var result = ZonedDateTime.Subtract(afterTransition, Duration.FromHours(1));
Console.WriteLine(result.Date);
// Adding an hour of elapsed time takes us across the DST transition, so we have
// the same local time (shown on a clock) but a different offset.
Console.WriteLine(result);

// The + operator and Plus instance method are equivalent to the Add static method.
var result2 = afterTransition - Duration.FromHours(1);
var result3 = afterTransition.Minus(Duration.FromHours(1));
Console.WriteLine(result2);
Console.WriteLine(result3);            
```

Output:

```text
Sunday, 29 October 2017
2017-10-29T01:45:00 Europe/Dublin (+01)
2017-10-29T01:45:00 Europe/Dublin (+01)
2017-10-29T01:45:00 Europe/Dublin (+01)

```

---
uid: NodaTime.ZonedDateTime.op_Subtraction(NodaTime.ZonedDateTime,NodaTime.Duration)
snippet: *content
---

```csharp
using NodaTime;
using System;

// Europe/Dublin transitions from UTC+1 to UTC+0 at 2am (local) on 2017-10-29
var dt = new LocalDateTime(2017, 10, 29, 1, 45, 0);
DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];

ZonedDateTime afterTransition = new ZonedDateTime(dt, dublin, Offset.FromHours(0));

var result = ZonedDateTime.Subtract(afterTransition, Duration.FromHours(1));
Console.WriteLine(result.Date);
// Adding an hour of elapsed time takes us across the DST transition, so we have
// the same local time (shown on a clock) but a different offset.
Console.WriteLine(result);

// The + operator and Plus instance method are equivalent to the Add static method.
var result2 = afterTransition - Duration.FromHours(1);
var result3 = afterTransition.Minus(Duration.FromHours(1));
Console.WriteLine(result2);
Console.WriteLine(result3);            
```

Output:

```text
Sunday, 29 October 2017
2017-10-29T01:45:00 Europe/Dublin (+01)
2017-10-29T01:45:00 Europe/Dublin (+01)
2017-10-29T01:45:00 Europe/Dublin (+01)

```

---
uid: NodaTime.ZonedDateTime.Minus(NodaTime.Duration)
snippet: *content
---

```csharp
using NodaTime;
using System;

// Europe/Dublin transitions from UTC+1 to UTC+0 at 2am (local) on 2017-10-29
var dt = new LocalDateTime(2017, 10, 29, 1, 45, 0);
DateTimeZone dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];

ZonedDateTime afterTransition = new ZonedDateTime(dt, dublin, Offset.FromHours(0));

var result = ZonedDateTime.Subtract(afterTransition, Duration.FromHours(1));
Console.WriteLine(result.Date);
// Adding an hour of elapsed time takes us across the DST transition, so we have
// the same local time (shown on a clock) but a different offset.
Console.WriteLine(result);

// The + operator and Plus instance method are equivalent to the Add static method.
var result2 = afterTransition - Duration.FromHours(1);
var result3 = afterTransition.Minus(Duration.FromHours(1));
Console.WriteLine(result2);
Console.WriteLine(result3);            
```

Output:

```text
Sunday, 29 October 2017
2017-10-29T01:45:00 Europe/Dublin (+01)
2017-10-29T01:45:00 Europe/Dublin (+01)
2017-10-29T01:45:00 Europe/Dublin (+01)

```

---
uid: NodaTime.ZonedDateTime.Subtract(NodaTime.ZonedDateTime,NodaTime.ZonedDateTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

var zone = DateTimeZone.ForOffset(Offset.FromHours(-5));
ZonedDateTime subject = new ZonedDateTime(Instant.FromUtc(2017, 7, 17, 7, 17), zone);
ZonedDateTime other = new ZonedDateTime(Instant.FromUtc(2017, 7, 17, 9, 17), zone);

var difference = ZonedDateTime.Subtract(other, subject);
Console.WriteLine(difference);
```

Output:

```text
0:02:00:00

```

---
uid: NodaTime.ZonedDateTime.Minus(NodaTime.ZonedDateTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

var zone = DateTimeZone.ForOffset(Offset.FromHours(-5));
ZonedDateTime subject = new ZonedDateTime(Instant.FromUtc(2017, 7, 17, 7, 17), zone);
ZonedDateTime other = new ZonedDateTime(Instant.FromUtc(2017, 7, 17, 9, 17), zone);

var difference = other.Minus(subject);
Console.WriteLine(difference);
```

Output:

```text
0:02:00:00

```

---
uid: NodaTime.ZonedDateTime.NanosecondOfDay
snippet: *content
---

```csharp
using NodaTime;
using System;

// This is a 25-hour day at the end of daylight saving time
var dt = new LocalDate(2017, 10, 29);
var time = new LocalTime(23, 59, 59);
var dublin = DateTimeZoneProviders.Tzdb["Europe/Dublin"];

var startOfDay = dublin.AtStartOfDay(dt);
ZonedDateTime nearEndOfDay = dublin.AtStrictly(dt + time);

Console.WriteLine(nearEndOfDay.NanosecondOfDay);

Duration duration = nearEndOfDay - startOfDay;
Console.WriteLine(duration);
```

Output:

```text
86399000000000
1:00:59:59

```

