@Title="Text handling"

There are two options for text handling in Noda Time. For some elements of
formatting, you can follow the "normal" approach from the .NET Base Class
Library (BCL) - in particular, most of the core Noda Time types implements
`IFormattable`. However, no parsing support is provided in this way. (It used
to be, but the whole approach is so convoluted that documenting it accurately
proved too great an overhead.)

The preferred approach is to use the "pattern" classes such as `LocalDatePattern`
and so forth. This leads to clearer, more robust code, and performs better. The formatting
support present in the BCL style is mostly present to work well with compound format strings,
where you may wish to mix several values of different types in a single formatting call.

All the types responsible for text in Noda Time are in the
[NodaTime.Text][3] namespace.

The pattern-based API
---------------------

A *pattern* is an object capable of *parsing* from text to a specific
type, and *formatting* a value to text. Parsing and formatting don't
take any other options: the pattern knows everything about how to
map between the value and text. In particular, internationalization
is handled by having the pattern hold a [`CultureInfo`][2].

Whereas using the BCL approach the format
information has to be specified on every call, using the pattern
approach the format information is fixed for any particular pattern.
Convenience methods are provided to create new pattern instances
based on existing ones but with different internationalization
information or other options.

Each core Noda type has its own pattern type such as
[`OffsetPattern`](noda-type://NodaTime.Text.OffsetPattern). All
these patterns implement the
[`IPattern<T>`](noda-type://NodaTime.Text.IPattern_1) interface,
which has simple `Format` and `Parse` methods taking just the value
and text respectively. The result of `Parse` is a
[`ParseResult<T>`](noda-type://NodaTime.Text.ParseResult_1) which
encapsulates both success and failure results.

The BCL-based API
-----------------

Most of the core Noda Time types ([`LocalDateTime`][4],
[`Instant`][5] etc) provide methods with the
following signatures:

- `ToString()`: Formats the value using the default pattern for the
current thread's format provider.
- `ToString(string, IFormatProvider)`: Formats the value with the
given pattern and format provider. The pattern text for this call is
exactly the same as when creating a pattern object with the preferred API.

Pattern text
------------

Each type has its own separate pattern text documentation. The
available patterns are as consistent as possible within reason, but
documenting each separately avoids confusion with some field
specifiers being available for some types but not others.

- [Offset patterns](offset-patterns.html)
- [Instant patterns](instant-patterns.html)
- [LocalTime patterns](localtime-patterns.html)
- [LocalDate patterns](localdate-patterns.html)
- [LocalDateTime patterns](localdatetime-patterns.html)
- [Period patterns](period-patterns.html)

Note that at present, `ZonedDateTime`, `OffsetDateTime` and `Duration` do not support
any form of parsing or user-specified formatting.

<a name="custom-patterns">Custom patterns</a>
---------------

All custom patterns support the following characters:

<table>
  <thead>
    <tr>
      <td>Character</td>
      <td>Meaning</td>
      <td>Example</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><code>%</code></td>
      <td>Escape to force a single-character custom pattern to be treated as such.</td>
      <td><code>%H</code> => <code>5</code></td>
    </tr>
    <tr>
      <td><code>'</code></td>
      <td>
        Open and close a text literal, which can include
        double quotes.
      </td>
      <td><code>HH'H'mm'm'</code> => <code>07H30M</code></td>
    </tr>
    <tr>
      <td><code>"</code></td>
      <td>
        Open and close a text literal, which can include
        single quotes.
      </td>
      <td><code>HH"'"mm</code> => <code>07'30</code></td>
    </tr>
    <tr>
      <td><code>\</code></td>
      <td>
        Escapes the following character.
      </td>
      <td><code>HH\'mm</code> => <code>07'30</code></td>
    </tr>
  </tbody>
</table>

Additionally:

- Where valid, `:` always refers to the culture-specific time separator (a colon in the invariant culture)
- Where valid, `/` always refers to the culture-specific date separator (a forward slash in the invariant culture)

Any characters within a custom format which *don't* have a specific
meaning are treated as text literals (when parsing, they must be
matched exactly; when formatting they are reproduced exactly). This
is supported mostly for the sake of compatibility. We **strongly
recommend** that you quote any text literals, to avoid nasty
surprises if extra characters take on special meanings in later
versions.

  [2]: http://msdn.microsoft.com/en-us/library/system.globalization.cultureinfo.aspx
  [3]: noda-ns://NodaTime.Text
  [4]: noda-type://NodaTime.LocalDateTime
  [5]: noda-type://NodaTime.Instant
