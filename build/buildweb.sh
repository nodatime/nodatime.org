#!/bin/bash

set -e

ROOT=$(git rev-parse --show-toplevel)
cd $ROOT/build

# Note: use --skip-api to skip building API docs

# Disable msbuild node reuse, in an attempt to stabilize the build.
# The bundler/minimizer seems to have problems which *may* be related
# to node reuse.
export MSBUILDDISABLENODEREUSE=1

skip_api_build=false
if [[ "$1" == "--skip-api" ]]
then
  skip_api_build=true
fi

echo "Building with BUILD_ENVIRONMENT=$BUILD_ENVIRONMENT"

# Build the API docs with docfx
if [[ "$skip_api_build" != "true" ]]
then
  ./buildapidocs.sh
  rm -rf $ROOT/src/NodaTime.Web/docfx
  cp -r tmp/site $ROOT/src/NodaTime.Web/docfx
fi

echo "Unstable TOC"
cat tmp/web/unstable/api/toc.yml

# Build the web site ASP.NET Core
rm -rf $ROOT/src/NodaTime.Web/bin/Release
# Make sure minification happens before publish...
dotnet build -nologo -clp:NoSummary -v quiet -c Release $ROOT/src/NodaTime.Web
dotnet publish -nologo -clp:NoSummary -v quiet -c Release $ROOT/src/NodaTime.Web

# Fix up blazor.config to work in Unix
# (Blazor is currently disabled.)
# sed -i 's/\\/\//g' $WEB_DIR/NodaTime.Web.Blazor.blazor.config

# Run a smoke test to check it still works, but without using GCS
# Skip this if we don't have API docs.
if [[ "$skip_api_build" != "true" ]]
then
  STORAGE__BUCKET=local:fakestorage ASPNETCORE_URLS=http://127.0.0.1:8080 dotnet test ../src/NodaTime.Web.SmokeTest
fi

# Add diagnostic text files
# commit.txt only contains commit info
# build.txt is the commit info and build time
# At some point we probably only want one of these.
declare -r publish=$ROOT/src/NodaTime.Web/bin/Release/net10.0/publish
declare -r nodatime_org_commit=$(git -C $ROOT rev-parse HEAD)
declare -r nodatime_commit=$(git -C $ROOT/../nodatime rev-parse HEAD)
declare -r nodatime_serialization_commit=$(git -C $ROOT/../nodatime.serialization rev-parse HEAD)

echo "Commits:" > $publish/wwwroot/commit.txt
echo "nodatime: $nodatime_commit" >> $publish/wwwroot/commit.txt
echo "nodatime.org: $nodatime_org_commit" >> $publish/wwwroot/commit.txt
echo "nodatime.serialization: $nodatime_serialization_commit" >> $publish/wwwroot/commit.txt

echo "Built at $(date -u -Iseconds)" > $publish/wwwroot/build.txt
cat $publish/wwwroot/commit.txt >> $publish/wwwroot/build.txt

echo Unstable TOC html
cat $publish/docfx/unstable/api/toc.html
