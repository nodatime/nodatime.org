@Title="Migrating from 2.x to 3.0"

Compared to Noda Time 2.0, Noda Time 3.0 contains very few breaking changes. In particular, with the exception of the areas
noted below, we intend for Noda Time 3.0 to be both *source- and binary-compatible* with Noda Time 2.x.

System requirements
====

Noda Time 2.x provided separate libraries supporting the `net45` and `netstandard1.3` frameworks (.NET 4.5+ and .NET Core 1.0+);
in comparison, Noda Time 3.0 supports only `netstandard2.0`. This effectively increases the minimum required platform versions for
Noda Time 3.x to *.NET Framework 4.7.2+* and *.NET Core 2.0+*.  See the [.NET Standard
Versions](https://github.com/dotnet/standard/blob/master/docs/versions.md) documentation for more details.

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
