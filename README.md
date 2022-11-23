[![AppVeyor status](https://ci.appveyor.com/api/projects/status/1od487l2paotghwm?svg=true)](https://ci.appveyor.com/project/nodatime/nodatime-org)

This repository now contains the web site code. The bulk of this repository
is an ASP.NET Core web application that serves the nodatime.org site.

Site content is spread among the following locations:

- `src/NodaTime.Web/Markdown/`: Markdown source.
- `src/NodaTime.Web/Views/`: HTML templates.
- `src/NodaTime.Web/wwwroot/`: Static content.
- `build/docfx/` DocFX templates and contents (for nodatime.org/api).

In order to actually build the web site, including the docs, you
need the `nodatime` and `nodatime.serialization` repositories as
well, cloned as peers of this one.
