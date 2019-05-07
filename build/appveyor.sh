#!/bin/bash

# Build script run by Appveyor.

set -e

declare -r ROOT=$(realpath $(dirname $0)/..)
cd $ROOT

dotnet --info

# Check all the tools still build
dotnet build build/Tools.sln

# And now the actual web site
dotnet build -c Release src/NodaTime-Web.sln
dotnet test -c Release src/NodaTime.Web.Test
