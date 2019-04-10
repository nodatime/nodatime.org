---
uid: NodaTime
summary: *content
---
The NodaTime namespace contains the core types for Noda Time,
including the main public classes for time zone and calendar support
which have implementations (and less frequently used types) in other
namespaces. For formatting and parsing functionality, see the
@NodaTime.Text namespace.

---
uid: NodaTime.Calendars
summary: *content
---
The NodaTime.Calendars namespace contains types related to calendars beyond the
@NodaTime.CalendarSystem type in the core @NodaTime namespace.

---
uid: NodaTime.Extensions
summary: *content
---
The NodaTime.Extensions namespace contains extensions for all Noda Time production types
(regardless of their original namespace). Developers using C# 6 onwards are encouraged to import
specific types from within this namespace in order to only expose the extension methods
they're directly interested in; developers using earlier versions of C# will have to import
the whole namespace and ignore extension methods they are not interested in.

---
uid: NodaTime.Globalization
summary: *content
---
The NodaTime.Globalization namespace contains types related to culture-sensitive
aspects of behaviour, principally for the sake of text formatting and parsing.

---
uid: NodaTime.Text
summary: *content
---
The NodaTime.Text namespace contains types related to formatting and parsing date and time
values to and from text. Each core Noda Time type has its own "pattern" class to create a more
object-oriented (and efficient) approach to text handling than the one taken to the BCL.
See the user guide for more information.

---
uid: NodaTime.TimeZones
summary: *content
---
The NodaTime.TimeZones namespace contains types related to time zones beyond the core
@NodaTime.DateTimeZone class in the @NodaTime namespace. Most users will have no need
to refer to the types in this namespace.

---
uid: NodaTime.TimeZones.Cldr
summary: *content
---
The NodaTime.TimeZones.Cldr namespace contains types related to time zone information provided
by the Unicode CLDR (Common Locale Data Repository) project.

---
uid: NodaTime.Utility
summary: *content
---
The NodaTime.Utility namespace contains helper classes which don't really fit anywhere else.

---
uid: NodaTime.Testing
summary: *content
---
The NodaTime.Testing namespace contains types designed to help write tests against
code which uses Noda Time. "Child" namespaces are organized to reflect the same structure
as the main project.
    
---
uid: NodaTime.Testing.Extensions
summary: *content
---
The NodaTime.Testing.Extensions namespace contains static classes
with extension methods to allow easier creation of test values.

---
uid: NodaTime.Testing.TimeZones
summary: *content
---
The NodaTime.Testing.TimeZones namespace provides types to aid testing production
code which uses time zones, to avoid hard-coding knowledge of "real" time zones into tests.

---
uid: NodaTime.Serialization.JsonNet
summary: *content
---
The NodaTime.Serialization.JsonNet namespace contains support code to enable
JSON serialization for Noda Time types using [Json.NET](https://www.newtonsoft.com/json).

---
uid: NodaTime.Serialization.Protobuf
summary: *content
---
The NodaTime.Serialization.Protobuf namespace contains extension
methods to convert between NodaTime types and the
[Protocol Buffers](https://developers.google.com/protocol-buffers/)
types exposed in [Google.Protobuf](https://www.nuget.org/packages/Google.Protobuf)
and [Google.Api.CommonProtos](https://www.nuget.org/packages/Google.Api.CommonProtos).
