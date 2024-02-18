# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# Cache-busting for Docker based on the latest commit
ADD https://api.github.com/repos/nodatime/nodatime/commits?per_page=1 nodatime-commits.txt
ADD https://api.github.com/repos/nodatime/nodatime.org/commits?per_page=1 nodatime-org-commits.txt
ADD https://api.github.com/repos/nodatime/nodatime.serialization/commits?per_page=1 nodatime-serialization-commits.txt

RUN git clone https://github.com/nodatime/nodatime.git
RUN git clone https://github.com/nodatime/nodatime.org.git
RUN git clone https://github.com/nodatime/nodatime.serialization.git

WORKDIR /source/nodatime.org/build
RUN ./buildweb.sh

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /source/nodatime.org/src/NodaTime.Web/bin/Release/net8.0/publish ./

ENV ASPNETCORE_URLS=http://127.0.0.1:8080
ENTRYPOINT ["dotnet", "NodaTime.Web.dll"]
