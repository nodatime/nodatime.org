# API changes from 3.1.x to unstable

## New classes

- [`NodaTime.Extensions.TimeProviderExtensions`](xref:NodaTime.Extensions.TimeProviderExtensions)

## New type members, by type

### New members in [`NodaTime.AnnualDate`](xref:NodaTime.AnnualDate)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.AnnualDate.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.Duration`](xref:NodaTime.Duration)

- [`AdditiveIdentity`](xref:NodaTime.Duration.AdditiveIdentity)
- [`FromNanoseconds(Int128)`](xref:NodaTime.Duration.FromNanoseconds(System.Int128))
- [`Multiply(double, Duration)`](xref:NodaTime.Duration.Multiply(System.Double%2CNodaTime.Duration))
- [`operator *(double, Duration)`](xref:NodaTime.Duration.op_Multiply(System.Double%2CNodaTime.Duration))
- [`operator +(Duration)`](xref:NodaTime.Duration.op_UnaryPlus(NodaTime.Duration))
- [`IXmlSerializable.GetSchema()`](xref:NodaTime.Duration.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)
- [`ToInt128Nanoseconds()`](xref:NodaTime.Duration.ToInt128Nanoseconds)

### New members in [`NodaTime.Instant`](xref:NodaTime.Instant)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.Instant.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)
- [`ToUnixTimeSecondsAndNanoseconds()`](xref:NodaTime.Instant.ToUnixTimeSecondsAndNanoseconds)

### New members in [`NodaTime.Interval`](xref:NodaTime.Interval)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.Interval.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.LocalDate`](xref:NodaTime.LocalDate)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.LocalDate.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.LocalDateTime`](xref:NodaTime.LocalDateTime)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.LocalDateTime.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.LocalTime`](xref:NodaTime.LocalTime)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.LocalTime.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.Offset`](xref:NodaTime.Offset)

- [`AdditiveIdentity`](xref:NodaTime.Offset.AdditiveIdentity)
- [`IMinMaxValue<Offset>.MaxValue`](xref:NodaTime.Offset.System%23Numerics%23IMinMaxValue%7BNodaTime%23Offset%7D%23MaxValue)
- [`IMinMaxValue<Offset>.MinValue`](xref:NodaTime.Offset.System%23Numerics%23IMinMaxValue%7BNodaTime%23Offset%7D%23MinValue)
- [`IXmlSerializable.GetSchema()`](xref:NodaTime.Offset.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.OffsetDate`](xref:NodaTime.OffsetDate)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.OffsetDate.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.OffsetDateTime`](xref:NodaTime.OffsetDateTime)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.OffsetDateTime.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.OffsetTime`](xref:NodaTime.OffsetTime)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.OffsetTime.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.Period`](xref:NodaTime.Period)

- [`AdditiveIdentity`](xref:NodaTime.Period.AdditiveIdentity)
- [`MaxValue`](xref:NodaTime.Period.MaxValue)
- [`MinValue`](xref:NodaTime.Period.MinValue)
- [`operator -(Period)`](xref:NodaTime.Period.op_UnaryNegation(NodaTime.Period))
- [`operator +(Period)`](xref:NodaTime.Period.op_UnaryPlus(NodaTime.Period))

### New members in [`NodaTime.PeriodBuilder`](xref:NodaTime.PeriodBuilder)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.PeriodBuilder.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.Text.InstantPattern`](xref:NodaTime.Text.InstantPattern)

- [`TwoDigitYearMax`](xref:NodaTime.Text.InstantPattern.TwoDigitYearMax)
- [`WithTwoDigitYearMax(int)`](xref:NodaTime.Text.InstantPattern.WithTwoDigitYearMax(System.Int32))

### New members in [`NodaTime.Text.LocalDatePattern`](xref:NodaTime.Text.LocalDatePattern)

- [`TwoDigitYearMax`](xref:NodaTime.Text.LocalDatePattern.TwoDigitYearMax)
- [`WithTwoDigitYearMax(int)`](xref:NodaTime.Text.LocalDatePattern.WithTwoDigitYearMax(System.Int32))

### New members in [`NodaTime.Text.LocalDateTimePattern`](xref:NodaTime.Text.LocalDateTimePattern)

- [`DateHourIso`](xref:NodaTime.Text.LocalDateTimePattern.DateHourIso)
- [`DateHourMinuteIso`](xref:NodaTime.Text.LocalDateTimePattern.DateHourMinuteIso)
- [`TwoDigitYearMax`](xref:NodaTime.Text.LocalDateTimePattern.TwoDigitYearMax)
- [`VariablePrecisionIso`](xref:NodaTime.Text.LocalDateTimePattern.VariablePrecisionIso)
- [`WithTwoDigitYearMax(int)`](xref:NodaTime.Text.LocalDateTimePattern.WithTwoDigitYearMax(System.Int32))

### New members in [`NodaTime.Text.LocalTimePattern`](xref:NodaTime.Text.LocalTimePattern)

- [`HourIso`](xref:NodaTime.Text.LocalTimePattern.HourIso)
- [`HourMinuteIso`](xref:NodaTime.Text.LocalTimePattern.HourMinuteIso)
- [`VariablePrecisionIso`](xref:NodaTime.Text.LocalTimePattern.VariablePrecisionIso)

### New members in [`NodaTime.Text.OffsetDatePattern`](xref:NodaTime.Text.OffsetDatePattern)

- [`TwoDigitYearMax`](xref:NodaTime.Text.OffsetDatePattern.TwoDigitYearMax)
- [`WithTwoDigitYearMax(int)`](xref:NodaTime.Text.OffsetDatePattern.WithTwoDigitYearMax(System.Int32))

### New members in [`NodaTime.Text.OffsetDateTimePattern`](xref:NodaTime.Text.OffsetDateTimePattern)

- [`TwoDigitYearMax`](xref:NodaTime.Text.OffsetDateTimePattern.TwoDigitYearMax)
- [`WithTwoDigitYearMax(int)`](xref:NodaTime.Text.OffsetDateTimePattern.WithTwoDigitYearMax(System.Int32))

### New members in [`NodaTime.Text.UnparsableValueException`](xref:NodaTime.Text.UnparsableValueException)

- [`UnparsableValueException(string, string, int, Exception)`](xref:NodaTime.Text.UnparsableValueException.%23ctor(System.String%2CSystem.String%2CSystem.Int32%2CSystem.Exception))
- [`UnparsableValueException(string, string, int)`](xref:NodaTime.Text.UnparsableValueException.%23ctor(System.String%2CSystem.String%2CSystem.Int32))
- [`Index`](xref:NodaTime.Text.UnparsableValueException.Index)
- [`Value`](xref:NodaTime.Text.UnparsableValueException.Value)

### New members in [`NodaTime.Text.YearMonthPattern`](xref:NodaTime.Text.YearMonthPattern)

- [`TwoDigitYearMax`](xref:NodaTime.Text.YearMonthPattern.TwoDigitYearMax)
- [`WithTwoDigitYearMax(int)`](xref:NodaTime.Text.YearMonthPattern.WithTwoDigitYearMax(System.Int32))

### New members in [`NodaTime.Text.ZonedDateTimePattern`](xref:NodaTime.Text.ZonedDateTimePattern)

- [`TwoDigitYearMax`](xref:NodaTime.Text.ZonedDateTimePattern.TwoDigitYearMax)
- [`WithTwoDigitYearMax(int)`](xref:NodaTime.Text.ZonedDateTimePattern.WithTwoDigitYearMax(System.Int32))

### New members in [`NodaTime.YearMonth`](xref:NodaTime.YearMonth)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.YearMonth.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

### New members in [`NodaTime.ZonedDateTime`](xref:NodaTime.ZonedDateTime)

- [`IXmlSerializable.GetSchema()`](xref:NodaTime.ZonedDateTime.System%23Xml%23Serialization%23IXmlSerializable%23GetSchema)

## Newly obsolete type members, by type

### Newly obsolete members in [`NodaTime.Text.UnparsableValueException`](xref:NodaTime.Text.UnparsableValueException)

- [`UnparsableValueException()`](xref:NodaTime.Text.UnparsableValueException.%23ctor) 
- [`UnparsableValueException(string, Exception)`](xref:NodaTime.Text.UnparsableValueException.%23ctor(System.String%2CSystem.Exception)) 
- [`UnparsableValueException(string)`](xref:NodaTime.Text.UnparsableValueException.%23ctor(System.String)) 
