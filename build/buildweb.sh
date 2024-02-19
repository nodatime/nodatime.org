#!/bin/bash

set -e

ROOT=$(git rev-parse --show-toplevel)
cd $ROOT/build

# Note: use --skip-api to skip building API docs

# Disable msbuild node reuse, in an attempt to stabilize the build.
# The bundler/minimizer seems to have problems which *may* be related
# to node reuse.
export MSBUILDDISABLENODEREUSE=1

# Build the API docs with docfx
if [[ "$1" != "--skip-api" ]]
then
  ./buildapidocs.sh
  rm -rf $ROOT/src/NodaTime.Web/docfx
  cp -r tmp/site $ROOT/src/NodaTime.Web/docfx
fi

# Build the web site ASP.NET Core
rm -rf $ROOT/src/NodaTime.Web/bin/Release
# Make sure minification happens before publish...
dotnet build -v quiet -c Release $ROOT/src/NodaTime.Web
dotnet publish -v quiet -c Release $ROOT/src/NodaTime.Web

# Fix up blazor.config to work in Unix
# (Blazor is currently disabled.)
# sed -i 's/\\/\//g' $WEB_DIR/NodaTime.Web.Blazor.blazor.config

# Run a smoke test to check it still works, but without using GCS
STORAGE__BUCKET=local:fakestorage dotnet test ../src/NodaTime.Web.SmokeTest

# Add diagnostic text files
# commit.txt only contains commit info
# build.txt is the commit info and build time
# At some point we probably only want one of these.
declare -r publish=$ROOT/src/NodaTime.Web/bin/Release/net8.0/publish
declare -r nodatime_org_commit=$(git -C $ROOT rev-parse HEAD)
declare -r nodatime_commit=$(git -C $ROOT/../nodatime rev-parse HEAD)
declare -r nodatime_serialization_commit=$(git -C $ROOT/../nodatime.serialization rev-parse HEAD)

echo "Commits:" > $publish/wwwroot/commit.txt
echo "nodatime: $nodatime_commit" >> $publish/wwwroot/commit.txt
echo "nodatime.org: $nodatime_org_commit" >> $publish/wwwroot/commit.txt
echo "nodatime.serialization: $nodatime_serialization_commit" >> $publish/wwwroot/commit.txt

echo "Built at $(date -u -Iseconds)" > $publish/wwwroot/build.txt
cat $publish/wwwroot/commit.txt >> $publish/wwwroot/build.txt
