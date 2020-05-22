@Title="The nodatime.org website"

## Overview

[The nodatime.org website](http://nodatime.org/) is currently served from an
ASP.NET Core web application hosted by Azure. The canonical source for the
website is the [nodatime.org GitHub
repository](https://github.com/nodatime/nodatime.org); changes to this
repository are automatically deployed to the website (via a webhook that
pings Azure to perform a pull/redeploy).

The source for most of the website is the [NodaTime.Web
project](https://github.com/nodatime/nodatime.org/tree/master/src/NodaTime.Web).
This contains both the web application and the website content,
particularly:

- [`Markdown/`](https://github.com/nodatime/nodatime.org/tree/master/src/NodaTime.Web/Markdown)
  containing the Markdown content used to generate the user and developer
  guides. More on how this directory structure works [below](#markdown).
- [`wwwroot/`](https://github.com/nodatime/nodatime.org/tree/master/src/NodaTime.Web/wwwroot)
  containing static content for the site.
- [`Views/`](https://github.com/nodatime/nodatime.org/tree/master/src/NodaTime.Web/Views)
  containing the programmatic content, for e.g. the home page and downloads
  page.

The [downloads](/downloads/) and [TZDB NZD files](/tzdb/) are stored on
Google Cloud Storage, under the `https://storage.googleapis.com/nodatime/`
bucket. The downloads are served to users directly via GCS URLs, but the NZD
files are fetched (and held permanently in memory) and then re-served via
the nodatime.org web application (mostly because we do not want to serve
redirects or non-nodatime.org URLs to clients).

By default, the contents of the Google Cloud Storage bucket are enumerated
on startup, and then refreshed every few minutes, to populate these lists.

## Running the website locally

Before the `NodaTime.Web` application can run, you must first create a
directory containing DocFX outputs for each NodaTime version, for the API
documentation.

This directory is `src/NodaTime.Web/docfx/`, and can be created empty (no
API documentation will be available), or can be populated with real outputs
as described [below](#docfx).

Once `src/NodaTime.Web/docfx/` exists, the application can be run directly.
Unless credentials to list the Google Cloud Storage bucket have been
provided, the `DISABLE_GCS` environment variable should also be set (to any
value) to use a hard-coded list of downloads and NZD files.

```sh
$ cd src/NodaTime.Web/
$ DISABLE_GCS=1 dotnet run
Hosting environment: Production
Content root path: [...]/src/NodaTime.Web
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

After this, visiting [http://localhost:5000/](http://localhost:5000/) should
show the front page.

Content is rendered at startup, so you'll need to restart the application if
you change anything.

<a name="markdown"></a>
## Markdown handling

The
[`Markdown/`](https://github.com/nodatime/nodatime.org/tree/master/src/NodaTime.Web/Markdown)
directory contains the source for files deployed at the root of the website
(`root/`), for the developer guide (`developer/`) and for the user guide
(`1.0.x/`, `1.1.x/`, ..., `unstable/`).

Each directory (or "bundle") contains an `index.json` ([for
example](https://github.com/nodatime/nodatime.org/blob/master/src/NodaTime.Web/Markdown/2.3.x/index.json))
that names all the Markdown source files (and static files) that should be
included in the output directory.

Importantly, `index.json` also defines a `parent` bundle that will be
consulted if a source file is not found in the current bundle. Bundles
therefore form a tree, and are used in the user guide to inherit unchanged
documentation files from a prior version.

The source is mostly vanilla Markdown, though there is some [light
additional
processing](https://github.com/nodatime/nodatime.org/blob/master/src/NodaTime.Web/Services/MarkdownLoader.cs)
that allows issues and API links to be referenced using an explicit syntax.

<a name="docfx"></a>
## docfx source

API documentation is produced from docfx-generated source stored in
`src/NodaTime.Web/docfx`. This is not committed to the repository, but can
be generated as follows.

The
[`build/buildapidocs.sh`](https://github.com/nodatime/nodatime.org/blob/master/build/buildapidocs.sh)
script can be run to generate the content (into `build/tmp/docfx/`) from a
`nodatime/` directory parallel to this repoistory's root (presumed to
contain the current development API, which will be placed in `unstable/`).

Note that historical docfx sources are fetched from a [`history`
branch](https://github.com/nodatime/nodatime.org/tree/history) on the
nodatime.org repository. These can be completely regenerated using the
[`build/buildhistory.sh`](https://github.com/nodatime/nodatime.org/blob/master/build/buildhistory.sh)
script.

## Minified source

By default, the website will use bundled and minified source files generated
as `wwwroot/css/site.min.css` and `wwwroot/js/site.min.js`. These are automatically
regenerated in the build process.

Alternatively, the original source files can be used by running under the
Development environment instead of the Production environment. The easiest
way to do this is by setting the [`ASPNETCORE_ENVIRONMENT` environment
variable](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments)
to `Development` when running the application.

## Deploying nodatime.org

The
[`build/buildweb.sh`](https://github.com/nodatime/nodatime.org/blob/master/build/buildweb.sh)
script regenerates the contents of the nodatime.org repository entirely from
scratch, so to push a new version of the website, simply run
`build/buildweb.sh /path/to/nodatime.org/repo` and then commit and push the
repository.
