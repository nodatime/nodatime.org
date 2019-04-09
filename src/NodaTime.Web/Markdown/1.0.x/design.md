@Title="Design philosophy and conventions"

(I should note that the "we" below is mostly the royal "we",
expressed by the author, Jon Skeet. Hopefully most of the
contributors to Noda Time agree with most of the below, but please
don't assume that they all agree with everything.)

Noda Time has been designed with the following goals in mind:

- **We want to force you to think about decisions you really *need* to
think about.**
In particular, what kind of data do you *really* have
and *really* need? Is it local or global? Do you need the system
time zone or some other time zone? If you're converting from a local
time to a global one in a particular time zone, how do you want to
handle ambiguities or gaps?

- **We want to solve the 99% case.**
Noda Time doesn't support leap
seconds, relativity or various other subtleties around time lines.
While we support some other calendars, we don't support you creating
your own calendar - because you almost certainly don't need to.
By focusing on the needs of the vast majority of users, we will make
*their* lives easier... but this does mean that if you need
something really specialized, Noda Time isn't likely to be right for
you.

  While we sympathize with the "make simple things easy, make
complex things possible" motto, our experience is typically that in
making complex things possible (with no indication that anyone
actually wants to do those things), the simple things become that
much harder.

- **We don't want to be your performance bottleneck.**
We regard Noda Time as a system-level library: we don't know exactly
how you'll use it, or how performance-critical that use will be.
We're willing to do more work (occasionally at the expense of
*internal* complexity) to get out of your way, but we're not going
to sacrifice *public* elegance for this.

- **We want your code to be robust in the face of new versions.**
Noda Time follows [Semantic Versioning][2] so you should be able to
spot incompatible versions - but additionally, by limiting the
amount of "hooks" we provide, we've reduced the opportunities for coupling between
your code and ours.

- **We want your code using Noda Time to be testable.**
To some people that will conjure up images of interfaces and virtual
methods everywhere - but that's not the case. Instead, we recommend
that you inject appropriate dependencies (such as clocks) and we
provide designed-for-testing implementations of types such as time
zones.

- **We don't like defaults.**
Just about the only thing Noda Time will default for you is the use
of the ISO calendar, as we believe that's what the majority of
developers want. However, we do *not* default to using the system
time zone, or using "now" as a default date/time value, or using
the current thread's current format provider for parsing and
formatting (except for the BCL-compatible method calls; see [text
handling](text) for more information on this).
  We make it easy to do all of these things, but they're just not
appropriate as implicit defaults. It's too easy to *accidentally*
depend on the time zone your system happens to be running in (etc)
without anything obvious in your code.

What this means in practice
---------------------------

- There are rather more types and [concepts](concepts) to learn about in
Noda Time than in .NET. One of the *problems* with .NET's date and
time API is that `DateTime` doesn't have a single well-defined
meaning.

- There are more value types in Noda Time than in many other
libraries. We believe they're justified as value types (they
represent single values) but you need to be aware of the impact on
boxing and the like. In many cases the default value of the type (e.g. `new
LocalDateTime()` or `default(LocalDateTime)`) is *not* a useful
value. This is unfortunate, but hard to avoid.

- All the value types and almost all the reference types are
immutable and [thread-safe](threading). We expect objects like calendars, time
zones, and patterns for formatting and parsing text to be reused
freely between many threads. Occasionally there's hidden mutability
in terms of caches, but this should not affect you, the user. We make sure it
all stays thread-safe for you.

- Almost all types are sealed, and there actually aren't very many
interfaces. Even the abstract classes often have internal abstract
methods, so can't be derived from by your code. This follows Josh
Bloch's approach of "design for inheritance or prohibit it" -
inheritance takes a lot of effort to do robustly, and we don't want
to break your code because you happen to rely on us calling a
particular method at a particular time.

  We're aware that this is one of the most contentious aspects of
Noda Time's design - particularly as [Joda Time][3] is *very*
extensible - but we believe that it's highly unlikely that you'll
ever *want* to extend Noda Time anyway. We want to be an
externally-simple library you can just rely on. If you have extra
requirements, chances are someone else will want to do something
similar - so pop along to the [mailing list][4] and we can collaborate on
trying to solve your problem within Noda Time itself.

[2]: http://semver.org/
[3]: http://www.joda.org/joda-time/
[4]: https://groups.google.com/group/noda-time
