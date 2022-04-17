---
uid: NodaTime.AnnualDate.CompareTo(NodaTime.AnnualDate)
snippet: *content
---

```csharp
using NodaTime;
using System;

var february23rd = new AnnualDate(2, 23);
var august17th = new AnnualDate(8, 17);

var lessThan = february23rd.CompareTo(august17th);
var equal = february23rd.CompareTo(february23rd);
var greaterThan = august17th.CompareTo(february23rd);

Console.WriteLine(lessThan);
Console.WriteLine(equal);
Console.WriteLine(greaterThan);
```

Output:

```text
-1
0
1

```

---
uid: NodaTime.AnnualDate.Equals(NodaTime.AnnualDate)
snippet: *content
---

```csharp
using NodaTime;
using System;

var february23rd = new AnnualDate(2, 23);
var august17th = new AnnualDate(8, 17);

var unequal = february23rd.Equals(august17th);
var equal = february23rd.Equals(february23rd);

Console.WriteLine(unequal);
Console.WriteLine(equal);
```

Output:

```text
False
True

```

---
uid: NodaTime.AnnualDate.InYear(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

var annualDate = new AnnualDate(3, 12);
var localDate = annualDate.InYear(2013);

Console.WriteLine(localDate);
```

Output:

```text
Tuesday, 12 March 2013

```

---
uid: NodaTime.AnnualDate.IsValidYear(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

var leapDay = new AnnualDate(2, 29);

var leapYear = leapDay.IsValidYear(2020);
var nonLeapYear = leapDay.IsValidYear(2018);

Console.WriteLine(leapYear);
Console.WriteLine(nonLeapYear);
```

Output:

```text
True
False

```

---
uid: NodaTime.DateAdjusters.AddPeriod(NodaTime.Period)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDateTime localDateTime = new LocalDateTime(1985, 10, 26, 1, 18);
Offset offset = Offset.FromHours(-5);
OffsetDateTime original = new OffsetDateTime(localDateTime, offset);

var dateAdjuster = DateAdjusters.AddPeriod(Period.FromYears(30));
OffsetDateTime updated = original.With(dateAdjuster);

Console.WriteLine(updated.LocalDateTime);
Console.WriteLine(updated.Offset);
```

Output:

```text
10/26/2015 01:18:00
-05

```

---
uid: NodaTime.DateAdjusters.DayOfMonth(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

var start = new LocalDate(2014, 6, 27);

var adjuster = DateAdjusters.DayOfMonth(19);

Console.WriteLine(adjuster(start));
```

Output:

```text
Thursday, 19 June 2014

```

---
uid: NodaTime.DateAdjusters.Month(System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

var start = new LocalDate(2014, 6, 27);

var adjuster = DateAdjusters.Month(2);

Console.WriteLine(adjuster(start));
```

Output:

```text
Thursday, 27 February 2014

```

---
uid: NodaTime.DateAdjusters.Next(NodaTime.IsoDayOfWeek)
snippet: *content
---

```csharp
using NodaTime;
using System;

var start = new LocalDate(2014, 6, 27);

var adjuster = DateAdjusters.Next(IsoDayOfWeek.Thursday);

Console.WriteLine(adjuster(start));
```

Output:

```text
Thursday, 03 July 2014

```

---
uid: NodaTime.DateAdjusters.NextOrSame(NodaTime.IsoDayOfWeek)
snippet: *content
---

```csharp
using NodaTime;
using System;

var start = new LocalDate(2014, 6, 27);

var adjuster = DateAdjusters.NextOrSame(IsoDayOfWeek.Friday);

Console.WriteLine(adjuster(start));
```

Output:

```text
Friday, 27 June 2014

```

---
uid: NodaTime.DateAdjusters.Previous(NodaTime.IsoDayOfWeek)
snippet: *content
---

```csharp
using NodaTime;
using System;

var start = new LocalDate(2014, 6, 27);

var adjuster = DateAdjusters.Previous(IsoDayOfWeek.Thursday);

Console.WriteLine(adjuster(start));
```

Output:

```text
Thursday, 26 June 2014

```

---
uid: NodaTime.DateAdjusters.PreviousOrSame(NodaTime.IsoDayOfWeek)
snippet: *content
---

```csharp
using NodaTime;
using System;

var start = new LocalDate(2014, 6, 27);

var adjuster = DateAdjusters.PreviousOrSame(IsoDayOfWeek.Friday);

Console.WriteLine(adjuster(start));
```

Output:

```text
Friday, 27 June 2014

```

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
uid: NodaTime.DateInterval.Deconstruct(NodaTime.LocalDate@,NodaTime.LocalDate@)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate start = new LocalDate(2017, 1, 1);
LocalDate end = new LocalDate(2017, 12, 31);

DateInterval value = new DateInterval(start, end);
value.Deconstruct(out LocalDate actualStart, out LocalDate actualEnd);

Console.WriteLine(actualStart);
Console.WriteLine(actualEnd);
```

Output:

```text
Sunday, 01 January 2017
Sunday, 31 December 2017

```

---
uid: NodaTime.DateInterval.Union(NodaTime.DateInterval)
snippet: *content
---

```csharp
using NodaTime;
using System;

DateInterval firstInterval = new DateInterval(
    new LocalDate(2014, 3, 7),
    new LocalDate(2014, 3, 20));

DateInterval secondInterval = new DateInterval(
    new LocalDate(2014, 3, 15),
    new LocalDate(2014, 3, 23));

DateInterval? overlappingInterval = firstInterval.Union(secondInterval);

Console.WriteLine(overlappingInterval);
```

Output:

```text
[2014-03-07, 2014-03-23]

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
uid: NodaTime.Interval.#ctor(NodaTime.Instant,NodaTime.Instant)
snippet: *content
---

```csharp
using NodaTime;
using System;

Instant start = Instant.FromUtc(2019, 1, 1, 15, 25, 48);
Instant end = Instant.FromUtc(2019, 1, 1, 16, 25, 48);
Interval interval = new Interval(start, end);
Console.WriteLine(interval.Start);
Console.WriteLine(interval.End);
```

Output:

```text
2019-01-01T15:25:48Z
2019-01-01T16:25:48Z

```

---
uid: NodaTime.Interval.#ctor(System.Nullable{NodaTime.Instant},System.Nullable{NodaTime.Instant})
snippet: *content
---

```csharp
using NodaTime;
using System;

Instant end = Instant.FromUtc(2019, 1, 1, 16, 25, 48);
Interval interval = new Interval(null, end);
Console.WriteLine(interval.HasStart);
Console.WriteLine(interval.HasEnd);
Console.WriteLine(interval.End);
```

Output:

```text
False
True
2019-01-01T16:25:48Z

```

---
uid: NodaTime.Interval.Contains(NodaTime.Instant)
snippet: *content
---

```csharp
using NodaTime;
using System;

Instant start = Instant.FromUtc(2019, 1, 1, 15, 25, 48);
Instant end = Instant.FromUtc(2019, 1, 1, 16, 25, 48);
Interval interval = new Interval(start, end);
Console.WriteLine(interval.Contains(Instant.FromUtc(2019, 1, 1, 15, 50, 50)));
```

Output:

```text
True

```

---
uid: NodaTime.Interval.Deconstruct(System.Nullable{NodaTime.Instant}@,System.Nullable{NodaTime.Instant}@)
snippet: *content
---

```csharp
using NodaTime;
using System;

Instant? start = Instant.FromUtc(2019, 1, 2, 3, 10, 11);
Instant? end = Instant.FromUtc(2020, 4, 5, 6, 12, 13);
Interval interval = new Interval(start, end);
interval.Deconstruct(out start, out end);
Console.WriteLine(start);
Console.WriteLine(end);
```

Output:

```text
2019-01-02T03:10:11Z
2020-04-05T06:12:13Z

```

---
uid: NodaTime.Interval.Duration
snippet: *content
---

```csharp
using NodaTime;
using System;

Instant start = Instant.FromUtc(2019, 1, 1, 3, 10, 20);
Instant end = Instant.FromUtc(2019, 1, 1, 9, 10, 20);
Interval interval = new Interval(start, end);
var duration = interval.Duration;
Console.WriteLine(duration);
```

Output:

```text
0:06:00:00

```

---
uid: NodaTime.Interval.ToString
snippet: *content
---

```csharp
using NodaTime;
using System;

Instant start = Instant.FromUtc(2019, 1, 2, 10, 11, 12);
Instant end = Instant.FromUtc(2020, 2, 3, 12, 13, 14);
Interval interval = new Interval(start, end);
var stringRepresentation = interval.ToString();
Console.WriteLine(stringRepresentation);
```

Output:

```text
2019-01-02T10:11:12Z/2020-02-03T12:13:14Z

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
Console.WriteLine(date.ToString("uuuu-MM-dd", CultureInfo.InvariantCulture));
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
uid: NodaTime.LocalDate.Day
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16);
int result = date.Day;
Console.WriteLine(result);
```

Output:

```text
16

```

---
uid: NodaTime.LocalDate.DayOfWeek
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16);
IsoDayOfWeek result = date.DayOfWeek;
Console.WriteLine(result);
```

Output:

```text
Wednesday

```

---
uid: NodaTime.LocalDate.DayOfYear
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16);
int result = date.DayOfYear;
Console.WriteLine(result);
```

Output:

```text
167

```

---
uid: NodaTime.LocalDate.Add(NodaTime.LocalDate,NodaTime.Period)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16);
LocalDate result = LocalDate.Add(date, Period.FromDays(3));
Console.WriteLine(result);
```

Output:

```text
Saturday, 19 June 2010

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
uid: NodaTime.LocalDate.AtMidnight
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16);
LocalDateTime dateTime = date.AtMidnight();
Console.WriteLine(dateTime);
```

Output:

```text
06/16/2010 00:00:00

```

---
uid: NodaTime.LocalDate.CompareTo(NodaTime.LocalDate)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date1 = new LocalDate(2010, 6, 16);
LocalDate date2 = new LocalDate(2010, 6, 16);
int result = date1.CompareTo(date2);
Console.WriteLine(result);
```

Output:

```text
0

```

---
uid: NodaTime.LocalDate.Equals(NodaTime.LocalDate)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date1 = new LocalDate(2010, 6, 16);
LocalDate date2 = new LocalDate(2010, 6, 16);
bool result = date1.Equals(date2);
Console.WriteLine(result);
```

Output:

```text
True

```

---
uid: NodaTime.LocalDate.Equals(System.Object)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16);
object dateAsObject = new LocalDate(2010, 6, 16);
bool result = date.Equals(dateAsObject);
Console.WriteLine(result);
```

Output:

```text
True

```

---
uid: NodaTime.LocalDate.Max(NodaTime.LocalDate,NodaTime.LocalDate)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate earlyJune = new LocalDate(2010, 6, 5);
LocalDate lateJune = new LocalDate(2010, 6, 25);
LocalDate max = LocalDate.Max(earlyJune, lateJune);
Console.WriteLine(max);
```

Output:

```text
Friday, 25 June 2010

```

---
uid: NodaTime.LocalDate.Min(NodaTime.LocalDate,NodaTime.LocalDate)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate earlyJune = new LocalDate(2010, 6, 5);
LocalDate lateJune = new LocalDate(2010, 6, 25);
LocalDate min = LocalDate.Min(earlyJune, lateJune);
Console.WriteLine(min);
```

Output:

```text
Saturday, 05 June 2010

```

---
uid: NodaTime.LocalDate.FromDateTime(System.DateTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

DateTime earlyJune = new DateTime(2010, 6, 5);
LocalDate date = LocalDate.FromDateTime(earlyJune);
Console.WriteLine(date);
```

Output:

```text
Saturday, 05 June 2010

```

---
uid: NodaTime.LocalDate.FromDateTime(System.DateTime,NodaTime.CalendarSystem)
snippet: *content
---

```csharp
using NodaTime;
using System;

DateTime earlyJune = new DateTime(2010, 6, 5);
CalendarSystem calendar = CalendarSystem.ForId("Julian");
LocalDate date = LocalDate.FromDateTime(earlyJune, calendar);
// Between the years 2000 and 2099, the Julian calendar is 13 days behind the Gregorian calendar.
Console.WriteLine(date.Year);
Console.WriteLine(date.Month);
Console.WriteLine(date.Day);
```

Output:

```text
2010
5
23

```

---
uid: NodaTime.LocalDate.#ctor(NodaTime.Calendars.Era,System.Int32,System.Int32,System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.Calendars;
using System;

LocalDate date = new LocalDate(Era.BeforeCommon, 2010, 6, 16);
Console.WriteLine(date);
```

Output:

```text
Sunday, 16 June 2010

```

---
uid: NodaTime.LocalDate.FromWeekYearWeekAndDay(System.Int32,System.Int32,NodaTime.IsoDayOfWeek)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = LocalDate.FromWeekYearWeekAndDay(2010, 24, IsoDayOfWeek.Wednesday);
Console.WriteLine(date);
```

Output:

```text
Wednesday, 16 June 2010

```

---
uid: NodaTime.LocalDate.FromYearMonthWeekAndDay(System.Int32,System.Int32,System.Int32,NodaTime.IsoDayOfWeek)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = LocalDate.FromYearMonthWeekAndDay(2010, 6, 3, IsoDayOfWeek.Wednesday);
Console.WriteLine(date);
```

Output:

```text
Wednesday, 16 June 2010

```

---
uid: NodaTime.LocalDate.Next(NodaTime.IsoDayOfWeek)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16);
LocalDate result = date.Next(IsoDayOfWeek.Thursday);
Console.WriteLine(result);
```

Output:

```text
Thursday, 17 June 2010

```

---
uid: NodaTime.LocalDate.ToYearMonth
snippet: *content
---

```csharp
using NodaTime;
using System;

YearMonth yearMonth = new LocalDate(2010, 6, 16).ToYearMonth();
Console.WriteLine(yearMonth);
```

Output:

```text
2010-06

```

---
uid: NodaTime.LocalDate.Plus(NodaTime.Period)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 1, 30).Plus(Period.FromMonths(1));
Console.WriteLine(date);
```

Output:

```text
Sunday, 28 February 2010

```

---
uid: NodaTime.LocalDate.Subtract(NodaTime.LocalDate,NodaTime.Period)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = LocalDate.Subtract(new LocalDate(2010, 2, 28), Period.FromMonths(1));
Console.WriteLine(date);
```

Output:

```text
Thursday, 28 January 2010

```

---
uid: NodaTime.LocalDate.Minus(NodaTime.Period)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDate date = new LocalDate(2010, 6, 16).Minus(Period.FromDays(1));
Console.WriteLine(date);
```

Output:

```text
Tuesday, 15 June 2010

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
Console.WriteLine(dt.ToString("uuuu-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture));
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
uid: NodaTime.OffsetDate.#ctor(NodaTime.LocalDate,NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

OffsetDate offsetDate = new OffsetDate(
    new LocalDate(2019, 5, 3),
    Offset.FromHours(3));

Console.WriteLine(offsetDate.Offset);
Console.WriteLine(offsetDate.Date);
```

Output:

```text
+03
Friday, 03 May 2019

```

---
uid: NodaTime.OffsetDate.WithOffset(NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

var original = new OffsetDate(
    new LocalDate(2019, 5, 3),
    Offset.FromHours(3));

OffsetDate updated = original.WithOffset(Offset.FromHours(-3));
Console.WriteLine(updated.Offset);
Console.WriteLine(updated.Date);
```

Output:

```text
-03
Friday, 03 May 2019

```

---
uid: NodaTime.OffsetDate.With(System.Func{NodaTime.LocalDate,NodaTime.LocalDate})
snippet: *content
---

```csharp
using NodaTime;
using System;

var original = new OffsetDate(
    new LocalDate(2019, 5, 3),
    Offset.FromHours(3));

Func<LocalDate, LocalDate> tomorrowAdjuster = x => x + Period.FromDays(1);
OffsetDate updated = original.With(tomorrowAdjuster);
Console.WriteLine(updated.Offset);
Console.WriteLine(updated.Date);
```

Output:

```text
+03
Saturday, 04 May 2019

```

---
uid: NodaTime.OffsetDate.WithCalendar(NodaTime.CalendarSystem)
snippet: *content
---

```csharp
using NodaTime;
using System;

var original = new OffsetDate(
    new LocalDate(2019, 5, 3, CalendarSystem.Iso),
    Offset.FromHours(3));

OffsetDate updated = original.WithCalendar(CalendarSystem.Gregorian);
Console.WriteLine(updated.Offset);
Console.WriteLine(updated.Calendar);
```

Output:

```text
+03
Gregorian

```

---
uid: NodaTime.OffsetDateTime.#ctor(NodaTime.LocalDateTime,NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDateTime localDateTime = new LocalDateTime(1985, 10, 26, 1, 18);
Offset offset = Offset.FromHours(-5);
OffsetDateTime offsetDateTime = new OffsetDateTime(localDateTime, offset);

Console.WriteLine(offsetDateTime.LocalDateTime);
Console.WriteLine(offsetDateTime.Offset);
```

Output:

```text
10/26/1985 01:18:00
-05

```

---
uid: NodaTime.OffsetDateTime.WithCalendar(NodaTime.CalendarSystem)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDateTime localDateTime = new LocalDateTime(1985, 10, 26, 1, 18, CalendarSystem.Iso);
Offset offset = Offset.FromHours(-5);
OffsetDateTime original = new OffsetDateTime(localDateTime, offset);
OffsetDateTime updated = original.WithCalendar(CalendarSystem.Julian);

Console.WriteLine(updated.LocalDateTime.ToString("r", null));

Console.WriteLine(updated.Offset);
```

Output:

```text
1985-10-13T01:18:00.000000000 (Julian)
-05

```

---
uid: NodaTime.OffsetDateTime.WithOffset(NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDateTime localDateTime = new LocalDateTime(1985, 10, 26, 1, 18);
Offset offset = Offset.FromHours(-3);
OffsetDateTime original = new OffsetDateTime(localDateTime, offset);
OffsetDateTime updated = original.WithOffset(Offset.FromHours(-2));

Console.WriteLine(updated.LocalDateTime);
Console.WriteLine(updated.Offset);
```

Output:

```text
10/26/1985 02:18:00
-02

```

---
uid: NodaTime.OffsetDateTime.With(System.Func{NodaTime.LocalDate,NodaTime.LocalDate})
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDateTime localDateTime = new LocalDateTime(1985, 10, 26, 1, 18);
Offset offset = Offset.FromHours(-5);
OffsetDateTime original = new OffsetDateTime(localDateTime, offset);
var dateAdjuster = DateAdjusters.AddPeriod(Period.FromYears(30));
OffsetDateTime updated = original.With(dateAdjuster);

Console.WriteLine(updated.LocalDateTime);
Console.WriteLine(updated.Offset);
```

Output:

```text
10/26/2015 01:18:00
-05

```

---
uid: NodaTime.OffsetDateTime.With(System.Func{NodaTime.LocalTime,NodaTime.LocalTime})
snippet: *content
---

```csharp
using NodaTime;
using System;

LocalDateTime localDateTime = new LocalDateTime(1985, 10, 26, 1, 18);
Offset offset = Offset.FromHours(-5);
OffsetDateTime original = new OffsetDateTime(localDateTime, offset);
OffsetDateTime updated = original.With(TimeAdjusters.TruncateToHour);

Console.WriteLine(updated.LocalDateTime);
Console.WriteLine(updated.Offset);
```

Output:

```text
10/26/1985 01:00:00
-05

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
uid: NodaTime.Offset.Add(NodaTime.Offset,NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

var leftHandOffset = Offset.FromHours(5);
var rightHandOffset = Offset.FromHours(6);
var result = Offset.Add(leftHandOffset, rightHandOffset);
Console.WriteLine(result);
```

Output:

```text
+11

```

---
uid: NodaTime.Offset.Subtract(NodaTime.Offset,NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

var leftHandOffset = Offset.FromHours(7);
var rightHandOffset = Offset.FromHours(5);
var result = Offset.Subtract(leftHandOffset, rightHandOffset);
Console.WriteLine(result);
```

Output:

```text
+02

```

---
uid: NodaTime.Offset.CompareTo(NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

var smallerOffset = Offset.FromHours(3);
var largerOffset = Offset.FromHours(5);

var lessThan = smallerOffset.CompareTo(largerOffset);
var equal = smallerOffset.CompareTo(smallerOffset);
var greaterThan = largerOffset.CompareTo(smallerOffset);

Console.WriteLine(lessThan);
Console.WriteLine(equal);
Console.WriteLine(greaterThan);
```

Output:

```text
-1
0
1

```

---
uid: NodaTime.Offset.Equals(NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

var offset1 = Offset.FromHoursAndMinutes(1, 30);
var inequalOffset = Offset.FromHours(2);

var unequal = offset1.Equals(inequalOffset);
var equal = offset1.Equals(offset1);

Console.WriteLine(unequal);
Console.WriteLine(equal);
```

Output:

```text
False
True

```

---
uid: NodaTime.Offset.Max(NodaTime.Offset,NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

var smallerOffset = Offset.FromHours(3);
var largerOffset = Offset.FromHours(5);
var result = Offset.Max(smallerOffset, largerOffset);
Console.WriteLine(result);
```

Output:

```text
+05

```

---
uid: NodaTime.Offset.Min(NodaTime.Offset,NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

var smallerOffset = Offset.FromHours(3);
var largerOffset = Offset.FromHours(5);
var result = Offset.Min(smallerOffset, largerOffset);
Console.WriteLine(result);
```

Output:

```text
+03

```

---
uid: NodaTime.Offset.Negate(NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

var offsetToNegate = Offset.FromHours(2);
var result = Offset.Negate(offsetToNegate);
Console.WriteLine(result);
```

Output:

```text
-02

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
uid: NodaTime.OffsetTime.#ctor(NodaTime.LocalTime,NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

OffsetTime offsetTime = new OffsetTime(
    new LocalTime(15, 20, 48),
    Offset.FromHours(3));

Console.WriteLine(offsetTime.TimeOfDay);
Console.WriteLine(offsetTime.Offset);
```

Output:

```text
15:20:48
+03

```

---
uid: NodaTime.OffsetTime.WithOffset(NodaTime.Offset)
snippet: *content
---

```csharp
using NodaTime;
using System;

OffsetTime original = new OffsetTime(
    new LocalTime(15, 20, 48),
    Offset.FromHours(3));

OffsetTime updated = original.WithOffset(Offset.FromHours(-3));

Console.WriteLine(updated.TimeOfDay);
Console.WriteLine(updated.Offset);
```

Output:

```text
15:20:48
-03

```

---
uid: NodaTime.OffsetTime.With(System.Func{NodaTime.LocalTime,NodaTime.LocalTime})
snippet: *content
---

```csharp
using NodaTime;
using System;

OffsetTime original = new OffsetTime(
    new LocalTime(15, 20, 48),
    Offset.FromHours(3));

OffsetTime updated = original.With(x => x.PlusHours(5));

Console.WriteLine(updated.TimeOfDay);
Console.WriteLine(updated.Offset);
```

Output:

```text
20:20:48
+03

```

---
uid: NodaTime.PeriodBuilder.#ctor(NodaTime.Period)
snippet: *content
---

```csharp
using NodaTime;
using System;

Period existingPeriod = Period.FromYears(5);
PeriodBuilder periodBuilder = new PeriodBuilder(existingPeriod);
periodBuilder.Months = 6;
Period period = periodBuilder.Build();
Console.WriteLine(period.Years);
Console.WriteLine(period.Months);
```

Output:

```text
5
6

```

---
uid: NodaTime.PeriodBuilder.Build
snippet: *content
---

```csharp
using NodaTime;
using System;

PeriodBuilder periodBuilder = new PeriodBuilder()
{
    Years = 2,
    Months = 3,
    Days = 4
};
Period period = periodBuilder.Build();
Console.WriteLine(period.Years);
Console.WriteLine(period.Months);
Console.WriteLine(period.Days);
```

Output:

```text
2
3
4

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
uid: NodaTime.Period.Normalize
snippet: *content
---

```csharp
using NodaTime;
using System;

var original = new PeriodBuilder { Weeks = 2, Days = 5 }.Build();
var actual = original.Normalize();
Console.WriteLine(actual);
```

Output:

```text
P19D

```

---
uid: NodaTime.Period.ToDuration
snippet: *content
---

```csharp
using NodaTime;
using System;

Period oneDayAsPeriod = Period.FromDays(1);
var actual = oneDayAsPeriod.ToDuration();
Console.WriteLine(oneDayAsPeriod.HasTimeComponent);
Console.WriteLine(actual);
```

Output:

```text
False
1:00:00:00

```

---
uid: System.Func{NodaTime.LocalTime,NodaTime.LocalTime}.Invoke(NodaTime.LocalTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

var time = LocalTime.FromMinutesSinceMidnight(63);
var truncated = TimeAdjusters.TruncateToHour(time);
Console.WriteLine(truncated);
```

Output:

```text
01:00:00

```

---
uid: System.Func{NodaTime.LocalTime,NodaTime.LocalTime}.Invoke(NodaTime.LocalTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

var time = LocalTime.FromSecondsSinceMidnight(127);
var truncated = TimeAdjusters.TruncateToMinute(time);
Console.WriteLine(truncated);
```

Output:

```text
00:02:00

```

---
uid: System.Func{NodaTime.LocalTime,NodaTime.LocalTime}.Invoke(NodaTime.LocalTime)
snippet: *content
---

```csharp
using NodaTime;
using System;

var time = LocalTime.FromMillisecondsSinceMidnight(3042);
var truncated = TimeAdjusters.TruncateToSecond(time);
Console.WriteLine(truncated);
```

Output:

```text
00:00:03

```

---
uid: NodaTime.DateTimeZone.GetUtcOffset(NodaTime.Instant)
snippet: *content
---

```csharp
using NodaTime;
using System;
using System.Globalization;

// Yes, in 1900 Paris did (according to TZDB) have a UTC offset of 9 minutes, 21 seconds.
DateTimeZone paris = DateTimeZoneProviders.Tzdb["Europe/Paris"];
Offset offset = paris.GetUtcOffset(Instant.FromUtc(1900, 1, 1, 0, 0));
Console.WriteLine(offset.ToString("G", CultureInfo.InvariantCulture));
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
uid: NodaTime.YearMonth.#ctor(System.Int32,System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using System;

YearMonth yearMonth = new YearMonth(2019, 5);
Console.WriteLine(yearMonth.Year);
Console.WriteLine(yearMonth.Month);
Console.WriteLine(yearMonth.Calendar);
```

Output:

```text
2019
5
ISO

```

---
uid: NodaTime.YearMonth.#ctor(NodaTime.Calendars.Era,System.Int32,System.Int32)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.Calendars;
using System;

YearMonth yearMonth = new YearMonth(Era.Common, 1994, 5);
Console.WriteLine(yearMonth.Year);
Console.WriteLine(yearMonth.Month);
Console.WriteLine(yearMonth.Calendar);
Console.WriteLine(yearMonth.Era);
```

Output:

```text
1994
5
ISO
CE

```

---
uid: NodaTime.YearMonth.#ctor(System.Int32,System.Int32,NodaTime.CalendarSystem)
snippet: *content
---

```csharp
using NodaTime;
using System;

YearMonth yearMonth = new YearMonth(2014, 3, CalendarSystem.Julian);
Console.WriteLine(yearMonth.Year);
Console.WriteLine(yearMonth.Month);
Console.WriteLine(yearMonth.Calendar);
```

Output:

```text
2014
3
Julian

```

---
uid: NodaTime.YearMonth.#ctor(NodaTime.Calendars.Era,System.Int32,System.Int32,NodaTime.CalendarSystem)
snippet: *content
---

```csharp
using NodaTime;
using NodaTime.Calendars;
using System;

YearMonth yearMonth = new YearMonth(Era.Common, 2019, 5, CalendarSystem.Gregorian);
Console.WriteLine(yearMonth.Year);
Console.WriteLine(yearMonth.Month);
Console.WriteLine(yearMonth.Calendar);
Console.WriteLine(yearMonth.Era);
```

Output:

```text
2019
5
Gregorian
CE

```

---
uid: NodaTime.YearMonth.ToDateInterval
snippet: *content
---

```csharp
using NodaTime;
using System;

YearMonth yearMonth = new YearMonth(2019, 5);
DateInterval interval = yearMonth.ToDateInterval();
Console.WriteLine(interval.Start);
Console.WriteLine(interval.End);
```

Output:

```text
Wednesday, 01 May 2019
Friday, 31 May 2019

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
// However, Europe/Dublin is also odd in terms of having its standard time as *summer* time,
// and its winter time as "daylight saving time". The saving offset in winter is -1 hour,
// as opposed to the more common "+1 hour in summer".
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
// However, Europe/Dublin is also odd in terms of having its standard time as *summer* time,
// and its winter time as "daylight saving time". The saving offset in winter is -1 hour,
// as opposed to the more common "+1 hour in summer".
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
Console.WriteLine(pattern.Format(zoned.PlusHours(1)));
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
Console.WriteLine(pattern.Format(zoned.PlusMinutes(1)));
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
Console.WriteLine(pattern.Format(zoned.PlusSeconds(1)));
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
Console.WriteLine(pattern.Format(zoned.PlusMilliseconds(1)));
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
Console.WriteLine(pattern.Format(zoned.PlusTicks(1)));
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
Console.WriteLine(pattern.Format(zoned.PlusNanoseconds(1)));
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

