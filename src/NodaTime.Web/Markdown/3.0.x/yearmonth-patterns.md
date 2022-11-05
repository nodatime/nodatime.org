@Title="Patterns for YearMonth values"

The [`YearMonth`](noda-type://NodaTime.YearMonth) type supports the following patterns:

Standard Patterns
-----------------

The following standard patterns are supported:

- `g`: ISO format pattern, which is always "uuuu'-'MM" using the invariant culture.

Custom Patterns
---------------

The following custom format pattern characters are supported for year/month values. These are equivalent to the [custom patterns for `LocalDate`](localdate-patterns),
but without the day-specific parts.

See [custom pattern notes](text#custom-patterns)
for general notes on custom patterns, including characters used for escaping and text literals.

For the meanings of "absolute" years and text handling, see later details.

<table>
  <thead>
    <tr>
      <td class="pattern-char">Character</td>
      <td class="pattern-description">Meaning</td>
      <td class="pattern-example">Example</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td><code>yy</code></td>
      <td>
        Two digit year of era, in the range 0-99. When parsing, the "base century" is chosen from the template
        value; if the two-digit year is greater than 30, the corresponding year in the previous century is used, unless the century of the template value is already the first century of the era. If the
        template value is in the first century and the input is "00", an exception will be thrown on parsing.
        Note that when formatting, no checking is performed to ensure that the year will be parsed to
        the same value. (For example, 1725 would be formatted as 25 but parsed as 2025.) In general, use of
        this pattern specifier is discouraged, on the grounds of it leading to ambiguity.
      </td>
      <td>
	    Assuming a template value of 2000 (the default):
        2012: <code>yy</code> => <code>12</code> <br />
        2040: <code>yy</code> => <code>40</code> - parsing "40" would give a date in 1940 <br />
      </td>
    </tr>
    <tr>
      <td><code>yyyy</code></td>
      <td>
        The year of era as 4 digits.
      </td>
      <td>
        2000 A.D. (ISO calendar, en-US): <code>yyyy g</code> => <code>2000 A.D.</code>
        13 B.C. (ISO calendar, en-US): <code>yyyy g</code> => <code>0013 B.C.</code>
      </td>
    </tr>
	<tr>
	  <td><code>u</code>, <code>uu</code>, <code>uuu</code>, <code>uuuu</code>
	  <td>
	    The absolute year, zero-padded as necessary to the same number of characters as the number of 'u' characters,
      with an optional leading <code>-</code> sign. See notes below.
    </td>
	  <td>
	    3 B.C.: <code>uuuu</code> => <code>-0002</code>
	  </td>
	</tr>
	<tr>
	  <td><code>g</code> or <code>gg</code></td>
	  <td>
	    The name of the era. This is calendar and culture specific. See notes below.
	  </td>
	  <td>
	    13 B.C. (ISO calendar, en-US): <code>y g</code> => <code>13 B.C.</code>
	  </td>
	</tr>
    <tr>
      <td><code>M</code> or <code>MM</code></td>
      <td>
        Month of year specified as a number. <code>MM</code> is zero-padded; <code>M</code> is not.
      </td>
      <td>
	    June: <code>M</code> => <code>6</code> <br />
	    June: <code>MM</code> => <code>06</code> <br />
	    December: <code>M</code> => <code>12</code> <br />
	    December: <code>MM</code> => <code>12</code> <br />
      </td>
    </tr>
    <tr>
      <td><code>MMM</code></td>
      <td>
	    Abbreviated month name, parsed case-insensitively. This is culture-sensitive.
      </td>
      <td>
	    (In an English locale.) <br />
	    June: <code>MMM</code> => <code>Jun</code> (can parse from "jun" or "JUN" etc.)<br />
	    December: <code>MMM</code> => <code>Dec</code> (can parse from "dec" or "DEC" etc.)<br />
      </td>
    </tr>
    <tr>
      <td><code>MMMM</code></td>
      <td>
	    Full month name, parsed case-insensitively. This is culture-sensitive.
      </td>
      <td>
	    (In an English locale.) <br />
	    June: <code>MMMM</code> => <code>June</code> (can parse from "june" or "JUNE" etc.)<br />
	    December: <code>MMMM</code> => <code>December</code> (can parse from "december" or "DECEMBER" etc.)<br />
      </td>
    </tr>
    <tr>
      <td><code>c</code></td>
      <td>
        The Noda-specific calendar system ID. This would rarely be appropriate
		for user-visible text, but allows exact round-tripping when serializing values via text.
      </td>
      <td><code>ISO</code><br />
	      <code>Coptic 3</code><br />
		  <code>Hijri Astronomical-Base16</code></td>
    </tr>
    <tr>
      <td><code>/</code></td>
      <td>
        The date separator for the format provider; slash in the invariant culture.
      </td>
      <td>en-US: <code>uuuu/MM/dd</code> => <code>2011/10/09</code><br />
          de-DE: <code>uuuu/MM/dd</code> => <code>2011.10.09</code>
          de-DE: <code>uuuu/MM/dd</code> => <code>2011.10.09</code></td>
    </tr>
  </tbody>

</table>

Notes
-----

**Absolute and era years**

Some calendars support multiple eras. For example, the ISO calendar supports the B.C. / B.C.E. and A.D. / C.E. eras.
A mapping is provided between "year within era" and "absolute" year - where an absolute year uniquely identifies the date,
and does not generally skip. In the ISO calendar, the absolute year 0 is deemed to be 1 B.C. and the absolute year 1 is
deemed to be 1 A.D. thus giving a simplified arithmetic system.

Negative absolute years can be both parsed and formatted - so "13 B.C." would be formatted as "-0012" using the "uuuu" format.

Note that the meaning of the "y" specifier has changed over time: in Noda Time 1.x, this meant "absolute year"; it now means
"year of era" to be consistent with the BCL. (This used to be handled by the "Y" specifier.) The "u" specifier is now used for
"absolute year".

**Text sources**

Noda Time comes with its own limited set of era names, but month and day names are taken from the .NET framework.
Unfortunately these are not available on a per-calendar basis, so the same names are used for all calendars, based solely
on culture. It is hoped that future release of Noda Time may use information from the [Unicode CLDR](http://cldr.unicode.org/)
to provide a more comprehensive treatment.

**Hebrew month names**

The Hebrew calendar has two month numbering systems (scriptural and civil), each with their
own benefits and drawbacks. Both have issues for text handling: as of Noda Time 1.3.0, the
civil month numbering is assumed as that corresponds with the BCL month numbering... but due to
the inclusion of a leap month, the month name/number correspondence changes in a leap year.
Until this is fixed, it is strongly recommended that you only use month *numbers* in any textual
representations of dates in the Hebrew calendar. Additionally, you may wish to consider how to
best clarify whether that month number is in the scriptural or civil numbering system.
