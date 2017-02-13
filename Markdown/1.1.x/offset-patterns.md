@Title="Patterns for Offset values"

The [`Offset`](noda-type://NodaTime.Offset) type supports the following patterns:

Standard Patterns
-----------------

The following standard patterns are supported:

- `f`: Full format, displaying all information including fractional seconds.  
  Typical pattern text: `+HH:mm:ss.fff`
- `l`: Long format, displaying information down to the second.  
  Typical pattern text: `+HH:mm:ss`
- `m`: Medium format, displaying information down to the minute.  
  Typical pattern text: `+HH:mm`
- `s`: Short format, displaying information down to the hour.  
  Typical pattern text: `+HH`
- `g`: General pattern. Formatting depends on the value passed in:
  - If the offset has fractional seconds, the full format is used; otherwise
  - If the offset has seconds, the long format is used; otherwise
  - If the offset has minutes, the medium format is used; otherwise
  - The short format is used
  When parsing, the other standard format patterns are tried one at a time.
- `G`: As `g`, but using `Z` for an offset of 0, as if it were Z-prefixed. (See below.)

Custom Patterns
---------------

The following custom offset pattern characters are supported for local times. See [custom pattern notes](text.html#custom-patterns)
for general notes on custom patterns, including characters used for escaping and text literals.

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
      <td><code>Z</code></td>
      <td>
        This can only occur at the start of a pattern, as a prefix to a normal pattern. When it's
		used, an offset of zero is always formatted as "Z", and "Z" will be parsed as a zero offset.
		When formatting, a non-zero offset falls back to the remainder of the pattern. When parsing,
		a non-Z value is always parsed by the remainder of the pattern, and a result of a zero offset
		is still acceptable. (So a pattern of "ZHH:mm" can still parse a value of "00:00" even though
		it would be formatted as "Z".)
      </td>
      <td>
        Zero: <code>ZHH:mm</code> => <code>Z</code> <br />
        5 hours: <code>ZHH:mm</code> => <code>05:00</code>
      </td>
    </tr>
    <tr>
      <td><code>H</code> or <code>HH</code></td>
      <td>
        Number of hours in the offset. <code>HH</code> is zero-padded; <code>H</code> is not.
      </td>
      <td>
        <code>H:mm</code> => <code>7:30</code> <br />
        <code>HH:mm</code> => <code>07:30</code>
      </td>
    </tr>
    <tr>
      <td><code>m</code> or <code>mm</code></td>
      <td>
        Number of minutes within the hour. <code>mm</code> is zero-padded; <code>m</code> is not.
      </td>
      <td>
        5 minutes: <code>m:ss</code> => <code>5:00</code> <br />
        5 minutes: <code>mm:ss</code> => <code>05:00</code>
      </td>
    </tr>
    <tr>
      <td><code>s</code> or <code>ss</code></td>
      <td>
        Number of seconds within the minute. <code>ss</code> is zero-padded; <code>s</code> is not.
      </td>
      <td>
        5 seconds: <code>s.fff</code> => <code>5.000</code> <br />
        5 seconds: <code>ss.fff</code> => <code>05.000</code>
      </td>
    </tr>
    <tr>
      <td><code>f</code>, <code>ff</code> or <code>fff</code></td>
      <td>
        The fractional second part of the offset, using exactly the specified number of characters.
		Trailing digits are truncated towards zero.
      </td>
      <td>
        1 second, 340 milliseconds: <code>s.fff</code> => 340 <br />
        1 second, 340 milliseconds: <code>s.ff</code> => 34 <br />
        1 second, 340 milliseconds: <code>s.f</code> => 3 <br />
      </td>
    </tr>
    <tr>
      <td><code>F</code>, <code>FF</code> or <code>FFF</code></td>
      <td>
        The fractional second part of the offset, using at most the specified number of characters.
		Trailing digits are truncated towards zero, and trailing insignificant zeroes are truncated.
		If this comes after a decimal separator and the value is zero, the decimal separator is
		also truncated.
      </td>
      <td>
        1 second, 340 milliseconds: <code>s.FFF</code> => <code>1.34</code> <br />
        1 second, 340 milliseconds: <code>s.FF</code> => <code>1.34</code> <br />
        1 second, 340 milliseconds: <code>s.F</code> => <code>1.3</code> <br />
        Exactly 1 second: <code>s.F</code> => <code>1</code> <br />
      </td>
    </tr>
    <tr>
      <td><code>+</code></td>
      <td>
        The sign of the value, always specified whether positive or negative.
        The character used will depend on the format provider; <code>+</code> and <code>-</code> are
        used by the invariant culture. A positive offset is used when local time is ahead of
		UTC (e.g. Europe) and a negative offset is used when local time is behind UTC (e.g. America).
      </td>
      <td>
        Positive value: <code>+HH:mm</code> => <code>+07:30</code> <br />
        Negative value: <code>+HH:mm</code> => <code>-07:30</code>
      </td>
    </tr>
    <tr>
      <td><code>-</code></td>
      <td>
        The sign of the value, only specified when the value is negative.
        The character used will depend on the format provider; <code>-</code> is
        used by the invariant culture.
      </td>
      <td>
        Positive value: <code>-HH:mm</code> => <code>07:30</code> <br />
        Negative value: <code>-HH:mm</code> => <code>-07:30</code>
      </td>
    </tr>
	<tr>
	  <td><code>.</code></td>
	  <td>
	    This is *always* a period ("."); not a culture-sensitive decimal separator as one might expect. This
		follows the example of other standard libraries, however odd it may appear. The only difference
		between a period and any other literal character is that when followed by a series of "F" characters,
		the period will be removed if there are no fractional seconds.
      </td>
	  <td>
	    12 seconds, 500 milliseconds (en-US): <code>ss.FFF</code> => <code>12.5</code> <br />
	    12 seconds, 500 milliseconds (fr-FR): <code>ss.FFF</code> => <code>12.5</code>
      </td>
    <tr>
      <td><code>:</code></td>
      <td>
        The time separator for the format provider; colon in the invariant culture.
      </td>
      <td><code>HH:mm</code> => <code>07:30</code></td>
    </tr>
  </tbody>    
</table>
