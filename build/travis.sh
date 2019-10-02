#!/bin/bash

set -e

# Install the .NET Core 2.2 runtime, as we use ASP.NET Core 2.2 at the moment.
# In the future we'll update to ASP.NET Core 3.0, we can use just the .NET Core 3.0 SDK.
sudo apt-get update
sudo apt-get install apt-transport-https
sudo apt-get update
sudo apt-get install aspnetcore-runtime-2.2=2.2.0-1

dotnet --info

# Check all the tools still build
dotnet build build/Tools.sln

# And now the actual web site
dotnet build -c Release src/NodaTime-Web.sln
dotnet test -c Release src/NodaTime.Web.Test
