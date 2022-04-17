# API changes from 3.0.x to unstable

## New classes

- [`NodaTime.Extensions.DateOnlyExtensions`](xref:NodaTime.Extensions.DateOnlyExtensions)
- [`NodaTime.Extensions.TimeOnlyExtensions`](xref:NodaTime.Extensions.TimeOnlyExtensions)

## New type members, by type

### New members in [`NodaTime.LocalDate`](xref:NodaTime.LocalDate)

- [`FromDateOnly(DateOnly)`](xref:NodaTime.LocalDate.FromDateOnly(System.DateOnly))
- [`ToDateOnly()`](xref:NodaTime.LocalDate.ToDateOnly)

### New members in [`NodaTime.LocalDateTime`](xref:NodaTime.LocalDateTime)

- [`MaxIsoValue`](xref:NodaTime.LocalDateTime.MaxIsoValue)
- [`MinIsoValue`](xref:NodaTime.LocalDateTime.MinIsoValue)

### New members in [`NodaTime.LocalTime`](xref:NodaTime.LocalTime)

- [`FromTimeOnly(TimeOnly)`](xref:NodaTime.LocalTime.FromTimeOnly(System.TimeOnly))
- [`ToTimeOnly()`](xref:NodaTime.LocalTime.ToTimeOnly)

### New members in [`NodaTime.NodaConstants`](xref:NodaTime.NodaConstants)

- [`MicrosecondsPerSecond`](xref:NodaTime.NodaConstants.MicrosecondsPerSecond)
- [`NanosecondsPerMicrosecond`](xref:NodaTime.NodaConstants.NanosecondsPerMicrosecond)

### New members in [`NodaTime.Period`](xref:NodaTime.Period)

- [`Add(Period, Period)`](xref:NodaTime.Period.Add(NodaTime.Period%2CNodaTime.Period))
- [`Between(YearMonth, YearMonth)`](xref:NodaTime.Period.Between(NodaTime.YearMonth%2CNodaTime.YearMonth))
- [`Between(YearMonth, YearMonth, PeriodUnits)`](xref:NodaTime.Period.Between(NodaTime.YearMonth%2CNodaTime.YearMonth%2CNodaTime.PeriodUnits))
- [`DaysBetween(LocalDate, LocalDate)`](xref:NodaTime.Period.DaysBetween(NodaTime.LocalDate%2CNodaTime.LocalDate))
- [`Subtract(Period, Period)`](xref:NodaTime.Period.Subtract(NodaTime.Period%2CNodaTime.Period))

### New members in [`NodaTime.Text.UnparsableValueException`](xref:NodaTime.Text.UnparsableValueException)

- [`UnparsableValueException(String, Exception)`](xref:NodaTime.Text.UnparsableValueException.%23ctor(System.String%2CSystem.Exception))

### New members in [`NodaTime.YearMonth`](xref:NodaTime.YearMonth)

- [`PlusMonths(Int32)`](xref:NodaTime.YearMonth.PlusMonths(System.Int32))
- [`ToString()`](xref:NodaTime.YearMonth.ToString)
