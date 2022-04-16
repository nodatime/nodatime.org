@Title="Release process"

This document describes how Noda Time is released. It is intended to be used as
a checklist by the person doing a release. It only covers doing a
new major/minor release; patch releases are generally just a matter
of editing `Build.Directory.props`, tagging the right branch and
creating a release in GitHub. The examples given are for releasing 2.3.0 (a long
time ago, of course).

## Prerequisites

- .NET SDKs (version will change over time; check global.json)
- git for Windows
- Bash that comes with git for Windows

## When to release

When everybody's happy, there are no issues outstanding for the milestone, and
all the tests pass.

Search the issue tracker for open issues with the right milestone (e.g.
`is:open is:issue milestone:1.4.0`).

## Releasing

- Update the `Directory.Build.props` in the root directory; this contains the version number
- Commit this change, and push it in a PR that describes the changes in this release
- In GitHub, create branch `2.3.x` from master
- Protect the branch (in GitHub repository settings)
- Create a release in GitHub, with a new tag `2.3.0` against the new branch
  - A GitHub action will automatically build and push to NuGet
        
## Post-release

Make changes in the master branch in the nodatime.org repo:

- Edit the version history to record the release
- Rename the `unstable` directory in NodaTime.Web/Markdown to `2.3.x`
- Edit `2.3.x/index.json` to specify the name `2.3.x`
- Create a new `unstable` directory and copy index.json from `2.3.x`
- Edit `unstable/index.json` to have a name of `unstable` and a
  parent of `2.3.x`
- Change the build scripts
  - Edit `buildhistory.sh` (this is pretty involved)
  - Edit `buildapidocs.sh`
  - Copy `build/docfx/docfx-2.2.x.json` to `docfx-2.3.x.json`
  - Edit `docfx-unstable.json` to include 2.3.x in `content` and `overwrite`
  - Copy `tzdbupdate/update-2.2.sh` to `update-2.3.sh` and edit it accordingly
  - Edit `tzdbupdate/update-all.sh` to call the new script
- Rebuild history
  - Run `build/buildhistory.sh`
  - Check the results, and push them as indicated by the script

Create a pull request with all these changes in. Review carefully,
and merge. The web site will then automatically be updated.
