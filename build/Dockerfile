# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Note: we expect to build this from a parent
# directory including clones of all three repos.
COPY nodatime /source/nodatime
COPY nodatime.org /source/nodatime.org
COPY nodatime.serialization /source/nodatime.serialization

WORKDIR /source/nodatime.org/build
RUN chmod +x buildweb.sh
RUN /bin/bash -c buildweb.sh

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /source/nodatime.org/src/NodaTime.Web/bin/Release/net8.0/publish ./

ENV ASPNETCORE_URLS=http://127.0.0.1:8080
ENTRYPOINT ["dotnet", "NodaTime.Web.dll"]
