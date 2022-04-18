@Title="Release process"

This document describes how Noda Time is released. It is intended to be used as
a checklist by the person doing a release. It only covers doing a
new major/minor release; patch releases are generally just a matter
of editing `Build.Directory.props`, tagging the right branch and
creating a release in GitHub. The examples given are for releasing 3.1.0.

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
- In GitHub, create branch `3.1.x` from main
- Protect the branch (in GitHub repository settings)
- Create a release in GitHub, with a new tag `3.1.0` against the new branch
  - A GitHub action will automatically build and push to NuGet
        
## Post-release

Make changes in the main branch in the nodatime.org repo:

### History branch (nodatime.org repo)

We keep the docfx metadata and snippets for each minor version in the history branch of the nodatime.org repo.
This used to be maintained via a `buildhistory.sh` script which would fetch all sources and rebuild from scratch.
As tools have aged, this has become infeasible - but we can easily add new versions.

- Check out clean clones of nodatime, nodatime.org and nodatime.serialization
- Change to the `build` directory of the nodatime.org repo
- Delete any local cache of history: `rm -rf ./history`
- Run the build for all API documentation: `./buildapidocs.sh`
- Copy the just-generated metadata into the history branch (edit for the right version number!):
  `for x in tmp/metadata/*/unstable; do cp -r $x history/$(basename $(realpath $x/..))/3.1.x; done`
- Commit and push the changes:
  - `cd history`
  - `git add --all`
  - `git commit -m "Add history for 3.1.x"
  - `git push`

### Web site pages (nodatime.org repo; best done in VS using the NodaTime.Web solution)

- Edit the version history to record the release
- Rename the `unstable` directory in NodaTime.Web/Markdown to `3.1.x`
- Edit `3.1.x/index.json` to specify the name `3.1.x`
- Create a new `unstable` directory and copy index.json from `3.1.x`
- Edit `unstable/index.json` to have a name of `unstable` and a
  parent of `3.1.x`
- Edit `build/docfx/docfx-web.json` to add the new version

## Time zone update scripts (nodatime repo)

- Copy (or move) `tzdbupdate/update-3.0.sh` to `update-3.1.sh` and edit it accordingly
- Edit `tzdbupdate/update-all.sh` to call the new script
