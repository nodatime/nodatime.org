# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Copy .NET 6 and .NET Core 3.1 packs so that we have the XML files
# to be used by SauceControl.InheritDoc.
COPY --from=mcr.microsoft.com/dotnet/sdk:6.0 /usr/share/dotnet/packs /usr/share/dotnet/packs
COPY --from=mcr.microsoft.com/dotnet/core/sdk:3.1 /usr/share/dotnet/packs /usr/share/dotnet/packs

WORKDIR /source

# Note: we expect to build this from a parent
# directory including clones of all three repos.
COPY nodatime nodatime
COPY nodatime.org nodatime.org
COPY nodatime.serialization nodatime.serialization

WORKDIR /source/nodatime.org/build
RUN chmod +x *.sh
RUN /bin/bash -c ./buildweb.sh

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /source/nodatime.org/src/NodaTime.Web/bin/Release/net8.0/publish ./

ENV ASPNETCORE_URLS=http://127.0.0.1:8080
ENTRYPOINT ["dotnet", "NodaTime.Web.dll"]
