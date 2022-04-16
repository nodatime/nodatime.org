@Title="Migrating from 2.x to 3.0"

Compared to Noda Time 2.0, Noda Time 3.0 contains very few breaking changes. In particular, with the exception of the areas
noted below, we intend for Noda Time 3.0 to be both *source- and binary-compatible* with Noda Time 2.x.

Note that making use of binary compatibility to upgrade an existing deployment without recompiling will require adding
appropriate assembly binding redirects so that the new library can be loaded in place of the old (and also requires running
on a platform version supported by Noda Time 3.0).

System requirements
====

Noda Time 2.x provided separate libraries supporting the `net45` and `netstandard1.3` frameworks (.NET 4.5+ and .NET Core 1.0+);
in comparison, Noda Time 3.0 supports only `netstandard2.0`. This effectively increases the minimum required platform versions for
Noda Time 3.x to *.NET Framework 4.7.2+* and *.NET Core 2.0+*.  See the [.NET Standard
Versions](https://github.com/dotnet/standard/blob/main/docs/versions.md) documentation for more details.

AllowPartiallyTrustedCallers (.NET Framework only)
====

The `net45` library in Noda Time 2.x was marked with the `AllowPartiallyTrustedCallers` attribute to allow it to be used by
partially trusted code.

The concept of partially trusted code is no longer supported, and the attribute has no effect in .NET Core (and was not present on
the `netstandard1.3` library in Noda Time 2.x), so has been removed from Noda Time 3.x.

This may be a breaking change if you have a deployment that depends upon this attribute being present.

Binary serialization
====

Support for .NET binary serialization has been removed in Noda Time 3.0. Noda Time objects still support native XML serialization,
and [various other projects](https://nodatime.org/serialization/api/) support serialization of Noda Time objects to other
serialization formats.

Obsolete members
====

The incorrectly-named extension method `IsoDayOfWeek.ToIsoDayOfWeek()` (which returns a `System.DayOfWeek`) has been removed. Code
referencing this method should use `IsoDayOfWeek.ToDayOfWeek()` instead, which has been available since Noda Time 2.1.

The `DateTimeZoneProviders.Serialization` property has been newly marked as obsolete in Noda Time 3.0. The replacement (to which
the obsolete property delegates) is `NodaTime.Xml.XmlSerializationSettings.DateTimeZoneProvider`.

BclDateTimeZone and midnight transitions
====

This could be considered a bugfix, but it is an incompatible change nonetheless. Transitions that would be represented as 24:00 in
the IANA time zone database ("following day midnight") are instead represented as 23:59:59.999 in the Windows database.
`BclDateTimeZone` in Noda Time 1.x and 2.x will use this value as provided, resulting in (for example) the start of a day being
reported as one millisecond earlier than intended (but matching the behaviour of `TimeZoneInfo`). Noda Time 3.0 will instead
recognise this pattern and interpret it as midnight of the following day, matching the 'correct' behaviour.
