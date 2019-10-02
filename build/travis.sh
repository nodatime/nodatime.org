#!/bin/bash

set -e

dotnet --info

# Check all the tools still build
dotnet build build/Tools.sln

# And now the actual web site
dotnet build -c Release src/NodaTime-Web.sln
dotnet test -c Release src/NodaTime.Web.Test
