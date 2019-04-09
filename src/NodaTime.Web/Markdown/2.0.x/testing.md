@Title="Unit testing with Noda Time"

This page is not about how Noda Time itself is tested - it's about how you to test code
which *uses* Noda Time.

NodaTime.Testing
----------------

Firstly, get hold of the [NodaTime.Testing](https://www.nuget.org/packages/NodaTime.Testing) assembly. It's currently fairly
small, but it will no doubt grow - and it will make your life much easier. The purpose of the assembly is to provide
easy-to-use test doubles which can be used instead of the real implementations.

While you *can* use Noda Time without dependency injection, it will make your code harder to test. Noda Time has
no particular support for any specific dependency injection framework, but should be easy to configure with any
reasonably-powerful implementation. (If it's not, please file a bug report.)

Clocks
======

The most obvious dependency is a clock - an implementation of [`NodaTime.IClock`](noda-type://NodaTime.IClock),
which simply provides "the current date and time" (as an `Instant`, given that the concept of "now" isn't
inherently bound to any time zone or calendar). The [`FakeClock`](noda-type://NodaTime.Testing.FakeClock) can
be set to any given instant, advanced manually, or set to advance a given amount each time it's accessed. The production
environment should normally inject the singleton [`SystemClock`](noda-type://NodaTime.SystemClock) instance which simply
uses `DateTime.UtcNow` behind the scenes.

Often you may want to repeatedly access the current time or date in a specific time zone.
The [`ZonedClock`](noda-type://NodaTime.ZonedClock) class helps with this, but doesn't need to be faked out: the
expectation is that you'll only need to fake out the `IClock`, then wrap that in a `ZonedClock` for the appropriate
zone, possibly with one of the extension methods in [`NodaTime.Extensions.ClockExtensions`](noda-type://NodaTime.Extensions.ClockExtensions).

Time zone providers and sources
===============================

For code which is sensitive to time zone fetching, an [`IDateTimeZoneProvider`](noda-type://NodaTime.IDateTimeZoneProvider) can
be injected. There are currently no test doubles for this interface, but usually
[`DateTimeZoneCache`](noda-type://NodaTime.TimeZones.DateTimeZoneCache) works perfectly well.

The cache uses another interface
- [`IDateTimeZoneSource`](noda-type://NodaTime.TimeZones.IDateTimeZoneSource) - to retrieve time zones under the hood, and there
we *do* have a test double: [`FakeDateTimeZoneSource`](noda-type://NodaTime.Testing.TimeZones.FakeDateTimeZoneSource). This source
is constructed (via a builder) with any `DateTimeZone` instances you want, and you can also specify custom Windows `TimeZoneInfo` ID mappings.

If you only need to specify the time zones, it's simplest to just use a collection initializer with the builder, like this:

```csharp
var source = new FakeDateTimeZoneSource.Builder
{
    // Where CreateZone is just a method returning a DateTimeZone...
    CreateZone("x"),
    CreateZone("y"),
    CreateZone("a"),
    CreateZone("b")
}.Build();
```

If you need to set other properties on the builder, the zones have to be specified through the `Zones` property:

```csharp
var source = new FakeDateTimeZoneSource.Builder
{
    VersionId = "CustomVersionId",
    Zones = { CreateZone("x"), CreateZone("y") }
}.Build();
```

The production environment should usually be
configured with one of the providers in [`DateTimeZoneProviders`](noda-type://NodaTime.DateTimeZoneProviders).

Time zones
==========

For time zones themselves, there are two fake implementations.
[`SingleTransitionDateTimeZone`](noda-type://NodaTime.Testing.TimeZones.SingleTransitionDateTimeZone) represents a time zone
with a single transition between different offsets, and is suitable for most test purposes.

```csharp
var transition = new Instant(100000L); // Or use Instant.FromUtc
var zone = new SingleTransitionDateTimeZone(transition, 3, 5);
```

This will create a zone which moves from UTC+3 to UTC+5 at the transition point. The ID of the zone can also be specified,
and the names of the early/late zone intervals are based on the ID. The zone intervals within the time zone do not have a
daylight saving aspect; they just have the specified standard offsets.

For more complex scenarios, [`MultiTransitionDateTimeZone`](noda-type://NodaTime.Testing.TimeZones.MultiTransitionDateTimeZone)
allows you to create a time zone from a collection of transitions, using a builder type. The standard and saving offsets for
each part of the time zone can be specified separately. For example:

```csharp
var transition1 = new Instant(0L);
var transition2 = new Instant(100000L);
var zone = new MultiTransitionDateTimeZone.Builder(2, 1, "X")
{
    { transition1, 2, 0, "Y" },
    { transition2, 1, 1, "Z" }
}.Build();
```

The offsets and ID provided to the constructor are used for the beginning of time up until the first specified transition,
at which point the offsets and ID provided with that transition are used until the next transition, etc.

(With both `SingleTransitionDateTimeZone` and `MultiTransitionDateTimeZone`, the offsets can also be specified with
`Offset` values, but in many tests it's simplest just to give the number of hours, as in the code above.)

Creating a time zone with no transitions at all is simple via `DateTimeZone.ForOffset`.
