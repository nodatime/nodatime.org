@Title="Serialization"

XML serialization
-----------------

As of Noda Time 1.2, the following types implement `IXmlSerializable` and can therefore be serialized:

- `Instant`
- `OffsetDateTime`
- `ZonedDateTime`
- `LocalDateTime`
- `LocalDate`
- `LocalTime`
- `Offset`
- `Interval`
- `Duration`
- `PeriodBuilder` (see note below)

XML serialization raises a few ugly issues which users should be aware of. Most importantly, it's designed for
mutable types with a parameterless constructor - which is somewhat problematic for a library composed primarily
of immutable types. However, as all structs implicitly have a parameterless constructor, and the `this` expression
is effectively a `ref` parameter in methods in structs, all the value types listed above have `ReadXml` methods which effectively end with:

```csharp
this = valueParsedFromXml;
```

This looks somewhat alarming, but is effectively sensible. It doesn't mutate the existing value so much as replace it with a completely new
value. XML serialization has been performed using explicit interface implementation in all types, so it's very unlikely that you'll end up
accidentally changing the value of a variable when you didn't expect to.

`Period` presents a rather greater challenge - as a reference type, we don't have the luxury of reassigning `this`, and we don't have a parameterless
constructor (nor do we want one). `PeriodBuilder` is a mutable type with a parameterless constructor, however, making it ideal for serialization. Typically
other classes wouldn't contain a `PeriodBuilder` property or field of course - but by exposing a "proxy" property solely for XML serialization purposes,
an appropriate effect can be achieved. The class might look something like this:

```csharp
/// <summary>
/// Sample class to show how to serialize classes which have Period properties.
/// </summary>
public class XmlSerializationDemo
{
    /// <summary>
    /// Use this property!
    /// </summary>
    [XmlIgnore]
    public Period Period { get; set; }

    /// <summary>
    /// Don't use this property! It's only present for the purposes of XML serialization.
    /// </summary>
    [XmlElement("period")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public PeriodBuilder PeriodBuilder
    {
        get { return Period == null ? null : Period.ToBuilder(); }
        set { Period = value == null ? null : value.Build(); }
    }
}
```

When serializing, the `XmlSerializer` will fetch the value from the `PeriodBuilder` property, which will in turn fetch the period from the `Period` property and convert it into a builder.
When deserializing, the `XmlSerializer` will set the value of `PeriodBuilder` from the XML - and the property will in turn build the builder and set the `Period` property.

In an ideal world we'd also decorate the `PeriodBuilder` property with `[Obsolete("Only present for serialization", true)]` but unfortunately the XML serializer ignores obsolete
properties, which would entirely defeat the point of the exercise.

It's also worth noting that the XML serialization in .NET doesn't allow any user-defined types to be
serialized via attributes. So while it would make perfect sense to be able to apply
[`[XmlAttribute]`](http://msdn.microsoft.com/en-us/library/system.xml.serialization.xmlattributeattribute)
to a particular property and have it serialized as an attribute, in reality you need to use
[`[XmlElement]`](http://msdn.microsoft.com/en-us/library/system.xml.serialization.xmlelementattribute.aspx)
instead. There's nothing Noda Time can do here; it's just a [limitation of .NET XML serialization](http://connect.microsoft.com/VisualStudio/feedback/details/277641/xmlattribute-xmltext-cannot-be-used-to-encode-types-implementing-ixmlserializable).

Finally, serialization of `ZonedDateTime` comes with the tricky question of which `IDateTimeZoneProvider` to use in order to convert a time zone ID specified in the XML into a `DateTimeZone`.
Noda Time has no concept of a "time zone provider registry" nor does a time zone "know" which provider it came from. Likewise XML serialization doesn't allow any particular local context to be
specified as part of the deserialization process. As a horrible workaround, a static (thread-safe) `DateTimeZoneProviders.Serialization` property is used. This would normally be set on application start-up,
and will be consulted when deserializing `ZonedDateTime` values. It defaults (lazily) to using `DateTimeZoneProviders.Tzdb`.

While these details are undoubtedly unpleasant, it is hoped that they strike a pragmatic balance, providing a significant benefit to those who require XML serialization support, while staying
out of the way of those who don't.

### Serialized representation

Most serialized forms just consist of element text using a specified text handling pattern, as described below. Types which support multiple calendar systems additionally use the `calendar` attribute for the calendar system ID (but only when the calendar system of the value isn't the ISO calendar), while `ZonedDateTime` always uses an extra `zone` attribute for the time zone ID.

`PeriodBuilder` and `Interval` are somewhat different: `PeriodBuilder` uses the round-trip text representation of the `Period` that would be built by it, while `Interval` has only `start` and `end` attributes, each of which is represented as the respective `Instant` converted using the extended ISO pattern.

<table>
  <thead>
    <tr>
      <td>Type</td>
      <td>Description or pattern</td>
      <td>Example</td>
    </tr>
  </thead>
  <tbody>    
    <tr>
      <td><code>Instant</code></td>
      <td>Extended ISO pattern</td>
      <td><code>&lt;value&gt;2013-07-26T16:45:20.1234567Z&lt;/value&gt;</td>
    </tr>
    <tr>
      <td><code>LocalDate</code></td>
      <td>ISO pattern, optional calendar</td>
      <td><code>&lt;value calendar="Gregorian 3"&gt;2013-07-26&lt;/value&gt;</td>
    </tr>
    <tr>
      <td><code>LocalTime</code></td>
      <td>Extended ISO pattern</td>
      <td><code>&lt;value&gt;16:45:20.1234567&lt;/value&gt;</td>
    </tr>
    <tr>
      <td><code>LocalDateTime</code></td>
      <td>Extended ISO pattern, optional calendar</td>
      <td><code>&lt;value calendar="Gregorian 3"&gt;2013-07-26T16:45:20.1234567&lt;/value&gt;</td>
    </tr>
    <tr>
      <td><code>OffsetDateTime</code></td>
      <td>RFC 3339 pattern (extended ISO but with offset in form +/-HH:mm or Z), optional calendar</td>
      <td><code>&lt;value calendar="Gregorian 3"&gt;2013-07-26T16:45:20.1234567+01:00&lt;/value&gt;</td>
    </tr>
    <tr>
      <td><code>ZonedDateTime</code></td>
      <td>Extended ISO pattern, optional calendar</td>
      <td><code>&lt;value calendar="Gregorian 3" zone="Europe/London"&gt;2013-07-26T16:45:20.1234567+01&lt;/value&gt;</td>
    </tr>
    <tr>
      <td><code>Interval</code></td>
      <td>Extended ISO pattern, optional calendar</td>
      <td><code>&lt;value start="2013-07-26T16:45:20.1234567Z" end="2013-07-26T17:45:20.1234567Z" /&gt;</td>
    </tr>
    <tr>
      <td><code>Offset</code></td>
      <td>General pattern</td>
      <td><code>&lt;value&gt;+01&lt;/value&gt;</td>
    </tr>
    <tr>
      <td><code>PeriodBuilder</code></td>
      <td>Round-trip pattern</td>
      <td><code>&lt;value&gt;P10Y5DT3H20S&lt;/value&gt;</td>
    </tr>
    <tr>
      <td><code>Duration</code></td>
      <td>Round-trip pattern</td>
      <td><code>&lt;value&gt;1:12:34:56.1234567&lt;/value&gt;</td>
    </tr>
  </tbody>
</table> 

Binary serialization
--------------------

As of Noda Time 1.2, for the desktop build only, the following types implement `ISerializable` and have the `[Serializable]` attribute applied to them, and can therefore be serialized using `BinaryFormatter`: 

- `Instant`
- `OffsetDateTime`
- `ZonedDateTime`
- `LocalDateTime`
- `LocalDate`
- `LocalTime`
- `Offset`
- `Interval`
- `Duration`
- `Period`

Binary serialization is simpler than XML serialization in terms of not interfering with immutability, which is why `Period` itself is serializable. However, the issue of requiring a time zone provider to be configured via `DateTimeZoneProviders.Serialization` is still present. (The same property is used for both binary and XML serialization.)

The PCL itself does not support binary serialization, so the interface and attribute are not applied to the above types in the PCL Noda Time build.

The serialized form is not documented here as it is not expected to be consumed other than by the `BinaryFormatter`; the relevant code is easily discovered in each serialized type, however.

Third-party serialization
-------------------------

The Noda Time project itself has support for [Json.NET](http://json.net). Additionally, there is a separate project for [ServiceStack.Text](https://github.com/ServiceStack/ServiceStack.Text/) support. Details of both are given below.

Json.NET: NodaTime.Serialization.JsonNet
----------------------------------------

[Json.NET](http://json.net) is supported within the `NodaTime.Serialization.JsonNet` assembly and the namespace
of the same name. This assembly is built against Json.NET 4.5.11, and is available in both portable and desktop flavours. It can be installed using NuGet, again with a package name of `NodaTime.Serialization.JsonNet`. See the
[installation guide](installation.html) for more details.

An extension method of `ConfigureForNodaTime` is provided on both `JsonSerializer` and
`JsonSerializerSettings`. Alternatively, the [`NodaConverters`](noda-type://NodaTime.Serialization.JsonNet.NodaConverters) type provides public static read-only fields
for individual converters. (All converters are immutable.)

Custom converters can be created easily from patterns using [`NodaPatternConverter`](noda-type://NodaTime.Serialization.JsonNet.NodaPatternConverter_1).

### Disabling automatic date parsing ###

Between releases 4.0 and 4.5.11, Json.NET introduced automatic date parsing: by default, if the parser detects a value which looks like a date, it will automatically convert it to a `DateTime` or (optionally) to a `DateTimeOffset`. In order to give Noda Time control over the serialization, this must be disabled in `JsonSerializerSettings` or `JsonSerializer`, like this:

```csharp
settings.DateParseHandling = DateParseHandling.None;
```

(The same code is valid for both `JsonSerializer` and `JsonSerializerSettings`.)

The `ConfigureForNodaTime` extension methods described above both disable automatic date parsing automatically.

### Supported types and default representations

All default patterns use the invariant culture.

- `Instant`: an ISO-8601 pattern extended to handle fractional seconds: `yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFF'Z'`
- `LocalDate`: ISO-8601 date pattern: `yyyy'-'MM'-'dd`
- `LocalTime`: ISO-8601 time pattern, extended to handle fractional seconds: `HH':'mm':'ss.FFFFFFF`
- `LocalDateTime`: ISO-8601 date/time pattern with no time zone specifier, extended to handle fractional seconds: `yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFF`
- `OffsetDateTime`: RFC3339 pattern:
 `yyyy'-'MM'-'dd'T'HH':'mm':'ss;FFFFFFFo<Z+HH:mm>`; note that the
 offset always includes hours and minutes, to conform with ECMA-262.
 It does not support round-tripping offsets with sub-minute components.
- `ZonedDateTime`: As `OffsetDateTime`, but with a time zone ID at the end: `yyyy'-'MM'-'dd'T'HH':'mm':'ss;FFFFFFFo<G> z`
- `Interval`: A compound object of the form `{ Start: xxx, End: yyy }` where `xxx` and `yyy` are represented however the serializer sees fit. (Typically using the default representation above.) An alternative form can be specified using the `WithIsoIntervalConverter` extension method on `JsonSerializer`/`JsonSerializerSettings`.
- `Offset`: general pattern, e.g. `+05` or `-03:30`
- `Period`: The round-trip period pattern; `NodaConverters.NormalizingIsoPeriodConverter` provides a converter using the normalizing ISO-like pattern
- `Duration`: A duration pattern of `-H:mm:ss.FFFFFFF` (like the standard round-trip pattern, but starting with hours instead of days)
- `DateTimeZone`: The ID, as a string

### Limitations

- Currently only ISO calendars are supported, and handling for negative and non-four-digit years will depend on the appropriate underlying pattern implementation. (Non-ISO converters are now possible, but the results would be very specific to Noda Time.)
- There's no indication of the time zone provider or its version in the `DateTimeZone` representation.

ServiceStack.Text: NodaTime.Serialization.ServiceStackText
-----------------------------------------------------------

[ServiceStack.Text](https://github.com/ServiceStack/ServiceStack.Text/) is supported via a
[separate project on GitHub](https://github.com/AnthonyCarl/NodaTime.Serialization.ServiceStackText).
There is a [NuGet package](https://www.nuget.org/packages/NodaTime.Serialization.ServiceStackText/), and documentation is on the project page. The package supports Noda Time version ≥ 1.2.0 and ServiceStack.Text ≥ 3.9.44.

The JSON representation is the same as that used by Json.NET above.
