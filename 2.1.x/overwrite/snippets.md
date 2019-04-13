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

