@Title="Calendars"

As described in the [core concepts documentation](concepts), a calendar system is a scheme
for dividing time into eras, years, months and days and so on. As a matter of simplification,
Noda Time treats every day as starting and ending at midnight, despite some calendars (such as
the Islamic and Hebrew calendars) traditionally having days stretching from sunset to sunset.

Additionally, Noda Time only handles calendars that *do* split time into eras, years, months and days - if we ever need to support any calendar which has other subdivisions, that would require specific
support.

Finally, only calculated calendars are supported. *Observational* calendars (where years and months
start based on unpredictable conditions such as the weather, or periodic decisions by political or religious leaders)
are currently out of scope for Noda Time.

Noda Time supports the calendars listed below. If you would like us to consider adding support for
another calendar system, please contact [the mailing list](https://groups.google.com/group/noda-time) or
[file a bug](https://github.com/nodatime/nodatime/issues).

ISO
===

First supported in v1.0.0  
API access: [`CalendarSystem.Iso`](noda-property://NodaTime.CalendarSystem.Iso)

This is the default calendar system when constructing values without explicitly specifying a calendar.
It is designed to correspond to the calendar described in [ISO-8601](https://en.wikipedia.org/wiki/ISO_8601),
and is equivalent to the Gregorian calendar in all respects other than the century and year-of-century values.
(In the ISO calendar, 1985 is year number 85 in century 19, and 1900 is year number 0 in century 19. In the
Gregorian calendar, the same years would be year 85 in century 20 and year 100 in century 19 respectively.)
This difference is rarely relevant in code.

The ISO calendar system always has at least 4 days in the first week of the week-year, where a week always
runs Monday to Sunday. The first week of the week-year is therefore always the one containing the first Thursday
in the year.

Gregorian
===

First supported in v1.0.0  
API access: [`CalendarSystem.GetGregorianCalendar()`](noda-type://NodaTime.CalendarSystem##NodaTime_CalendarSystem_GetGregorianCalendar_System_Int32_)

The [Gregorian calendar](https://en.wikipedia.org/wiki/Gregorian_calendar) was a refinement to the Julian calendar,
changing the formula for which years are leap years from "whenever the year is divisible by 4" to
"whenever the year is divisible by 4, except when it's also divisible by 100 and *not* divisible by 400". This keeps
the calendar in closer sync with the observed rotation of the earth around the sun.

Although the Gregorian calendar was introduced by Pope Gregory XIII in 1582, it was adopted in different places
at different times. Noda Time's implementation is *proleptic*, in that it extends into the distant past. There is
no support for a "cutover" calendar system which observes the Julian calendar until some point, at which point it
cuts over to the Gregorian calendar system. Although such a calendar system would more accurately represent
the calendar observed in many countries over time, it causes all sorts of API difficulties.

The parameter to `GetGregorianCalendar()` indicates the minimum number of days in the first week of the week-year.

Julian
===

First supported in v1.0.0  
API access: [`CalendarSystem.GetJulianCalendar()`](noda-type://NodaTime.CalendarSystem##NodaTime_CalendarSystem_GetJulianCalendar_System_Int32_)

The [Julian calendar](https://en.wikipedia.org/wiki/Julian_calendar) was introduced by Julius Caesar in 46BC, and took
effect in 45BC. It was like the Gregorian calendar, but with a simpler leap year rule - every year number divisible by
4 was a leap year.

The Noda Time implementation of the Julian calendar is proleptic, and ignores the fact that before around 4AD the leap
year rule was accidentally implemented as a leap year every *three* years. See the linked Wikipedia article for more
details of this leap year error, along with suggestions of how history might have actually played out.

The parameter to `GetJulianCalendar()` indicates the minimum number of days in the first week of the week-year.

Coptic (Alexandrian)
===

First supported in v1.0.0  
API access: [`CalendarSystem.GetCopticCalendar()`](noda-type://NodaTime.CalendarSystem#NodaTime_CalendarSystem_GetCopticCalendar_System_Int32_)

The Coptic calendar system is used by the Coptic Orthodox Church. Each year has 12 months of exactly 30 days, followed by
one month with either 5 or 6 days depending on whether or not the year is a leap year. Like the Julian calendar,
every year number divisible by 4 is a leap year in the Coptic calendar.

Year 1 in the Coptic calendar began on August 29th 284 CE (Julian). The implementation is not proleptic;
dates earlier than year 1 cannot be represented in the Coptic calendar in Noda Time.

The parameter to `GetCopticCalendar()` indicates the minimum number of days in the first week of the week-year.

Islamic (Hijri)
===

First supported in v1.0.0  
API access: [`CalendarSystem.GetIslamicCalendar()`](noda-type://NodaTime.CalendarSystem#NodaTime_CalendarSystem_GetIslamicCalendar_NodaTime_Calendars_IslamicLeapYearPattern_NodaTime_Calendars_IslamicEpoch_)

The [Islamic (or Hijri) calendar](https://en.wikipedia.org/wiki/Islamic_calendar) is a lunar calendar with years of 12
months, totalling either 355 or 354 days depending on whether or not it's a leap year. There are various schemes
for determining which years are leap years, all based on a 30 year cycle. Noda Time supports four options here,
specified in the [`IslamicLeapYearPattern`](noda-type://NodaTime.Calendars.IslamicLeapYearPattern) enumeration.

In the Islamic calendar, each day officially begins at sunset, but the Noda Time implementation (like most other date/time
APIs) ignores this and treats every day as beginning and ending at midnight.

Year 1 in the Islamic calendar began on July 15th or 16th, 622 CE (Julian) - different sources appear to use different
epochs, and the "sunset vs midnight" difference exacerbates this. Within Noda Time, the two epochs are known as
astronomical (July 15th CE Julian) and civil (July 16th CE Julian) and are specified in the
[`IslamicEpoch`](noda-type://NodaTime.Calendars.IslamicEpoch) enumeration.

The `GetIslamicCalendar()` method accepts two parameters, specifying the leap year pattern and epoch. You should carefully
consider which other systems you need to interoperate with when deciding which values to specify for these parameters.

This calendar is not to be confused with the Solar Hijri calendar which is implemented in a simplified form within
Noda Time, as described below.

Persian (or Solar Hijri)
===

First supported in v1.3.0  
API access: [`CalendarSystem.GetPersianCalendar()`](noda-type://NodaTime.CalendarSystem#NodaTime_CalendarSystem_GetPersianCalendar)

The [Persian (or Solar Hijri) calendar](https://en.wikipedia.org/wiki/Solar_Hijri_calendar) is the official calendar of
Iran and Pakistan. The first day of the Persian calendar is March 18th 622CE (Julian).

This is properly an observational calendar, but the implementation in Noda Time is equivalent to that of
the BCL [`PersianCalendar`](https://msdn.microsoft.com/en-us/library/system.globalization.persiancalendar.aspx) class,
which has a simple leap cycle of 33 years, where years 1, 5, 9, 13, 17, 22, 26, and 30 in each cycle are leap years.
There is a more complicated algorithmic version proposed by Ahmad Birashk, but this has not been implemented in Noda Time.

Hebrew
===

First supported in v1.3.0  
API access: [`CalendarSystem.GetHebrewCalendar()`](noda-type://NodaTime.CalendarSystem##NodaTime_CalendarSystem_GetHebrewCalendar_NodaTime_Calendars_HebrewMonthNumbering_)

The [Hebrew calendar](https://en.wikipedia.org/wiki/Hebrew_calendar) is a lunisolar calendar used primarily for determination
of religious holidays within Judaism. It was originally observational, but a computed version is now commonly used. Even
this is very complicated, however: most years have 12 months, but 7 in every 19 years are leap years, with 13 months. Two
of the months in the regular calendar have variable numbers of days depending on other aspects of the calendar, in order
to avoid religious holidays from falling on inappropriate days of the week.

The additional month in a leap year presents challenges for text handling, as well as for calendrical calculations in general.
The support in Noda Time 1.3.0 should be seen as somewhat experimental, but feedback is very warmly welcomed. It's important
to note that parsing and formatting of month names is *expected* to be incorrect in this version.

Like the Islamic calendar, a Hebrew day properly starts at sunset, but this is not modelled within Noda Time.

The `GetHebrewCalendar()` method accepts one parameter, specifying which month numbering system to use. The scriptural
month numbering system uses Nisan as month 1, even though the new year (when the year number changes) occurs at the start of
Tishri. In the scriptural system, Adar is month 12 in a non-leap year, and Adar I and Adar II are months 12 and 13 in a leap year.

The civil month numbering system uses Tishri as month 1 (so the year number increases when the month number becomes 1 again,
as in most calendars) but this means that Adar is month 6 in a non-leap year, and Adar I and Adar II are months 6 and 7 in a leap year.
This then means that the subsequent months (Nisan, Iyar, Sivan, Tamuz, Av, Elul) have different numbers in leap and non-leap years.

Unlike the parameters for the Islamic calendar, the month numbering in the Hebrew calendar doesn't affect any calculations - it *only*
affects the numeric values of the months both accepted when constructing values (such as in the `LocalDate` constructor)
and retrieving them (such as with `LocalDate.Month`).
