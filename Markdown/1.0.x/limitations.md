@Title="Limitations of Noda Time"

Noda Time is a work in progress. It has various limitations, some of
which we'd obviously like to remove over time. Here's a list of some
aspects we'd like to improve; see the 
[issues list](https://github.com/nodatime/nodatime/issues) for
others.

Without *your* help, we won't know which of these features to work
on first - or whether there are other limitations we hadn't even
considered. If there's something that should be within Noda Time's
scope, but we don't support it yet, then *please* either raise an
issue or post on the
[mailing list](https://groups.google.com/group/noda-time).


Fuller text support
===================

Currently there's no way of parsing or formatting a
[`ZonedDateTime`](noda-type://NodaTime.ZonedDateTime) or 
[`OffsetDateTime`](noda-type://NodaTime.OffsetDateTime), partly due to
the difficulty of representing time zones or offsets. While this is
the primary deficiency of our current text support, it's not the
only one. Some other types lack flexible formatting, and we may want
to optimize further at some point too.

Better resource handling
========================

We'd like to be able to bundle appropriate patterns (and other
internationalizable materials) within Noda Time while keeping it as
a single DLL. (Satellite DLLs are fine for some scenarios, but messy
in others.) Additionally we'd like to allow these resources to be
augmented or replaced by the caller at execution time, to allow
hot-fixes for cultures which we don't support as well as we might.

More time zone information
==========================

[CLDR](http://cldr.unicode.org) provides useful information about
time zones such as a canonical ID and user-friendly representations
(countries and sample cities). We'd also like to make it clearer
when one zoneinfo time zone is an alias for another.

More calendars
==============

There will probably always be more calendars we could support. The
highest priority is probably an adapter for the BCL calendars.

Smarter arithmetic
==================

As noted in the [arithmetic guide](arithmetic.html), arithmetic using
[`Period`](noda-type://NodaTime.Period) is pretty simplistic. We may
want something smarter, probably to go alongside the "dumb but
predictable" existing logic. This will definitely be driven by real
user requirements though - it would be far too easy to speculate.
