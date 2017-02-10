@Title="Patterns for Instant values"

The [`Instant`](noda-type://NodaTime.Instant) type supports the following patterns:

Standard Patterns
-----------------

The following standard patterns are supported:

- `g`: General format pattern.  
  The ISO-8601 representation of this instant in UTC, using the
  pattern "yyyy-MM-ddTHH:mm:ss'Z'".
  
- `n`: Numeric with thousand separators.  
  This gives the number of ticks since the Unix epoch as an integer,
  including thousands separators. Sample on September 16th 2011:
  "13,161,548,674,473,131"
  
- `d`: Numeric without thousand separators.  
  This gives the number of ticks since the Unix epoch as an integer,
  not including thousands separators. Sample on September 16th 2011:
  "13161548674473131"

Custom Patterns
---------------

[`Instant`](noda-type://NodaTime.Instant) supports all the [`LocalDateTime` custom patterns](localdatetime-patterns.html).
The pattern allows the culture to be specified, but *always* uses the ISO-8601 calendar, and *always* uses the UTC
time zone. The "template value" is always the unix epoch.
