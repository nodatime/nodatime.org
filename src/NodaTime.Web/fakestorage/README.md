The subdirectories here contain sample files for local development.
Normally these files are loaded from Google Cloud Storage, but that
can slow down local development.

This directory doesn't contain *all* the data for each data type -
just enough to make local development painless.

appsettings.Devlopment.json specifies this directory to load files
from, and LocalStorageRepository performs the actual loading.

This directory is *not* copied on publish, as we don't want it
appearing anywhere "real". However, it is in source control to make
it trivial to get started.

The releases are dummy zip files, but the time zone data and
benchmarks are real. (The set of benchmark files may change over
time, as we probably want an interrelated set for comparisons of the
same environment over a few commits, etc.)
