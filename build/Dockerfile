# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /source

RUN git clone https://github.com/nodatime/nodatime.git
RUN git clone https://github.com/nodatime/nodatime.serialization.git
COPY . nodatime.org

ARG build_environment
ENV BUILD_ENVIRONMENT=$build_environment

# See https://github.com/saucecontrol/InheritDoc/issues/15
ENV NUGET_XMLDOC_MODE=none

WORKDIR /source/nodatime.org/build
RUN chmod +x *.sh
RUN /bin/bash -c ./buildweb.sh

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /source/nodatime.org/src/NodaTime.Web/bin/Release/net8.0/publish ./

ENTRYPOINT ["dotnet", "NodaTime.Web.dll"]
