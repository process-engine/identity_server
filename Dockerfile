FROM microsoft/dotnet:2.1-sdk

WORKDIR /usr/src

COPY . /usr/src/identity_server

WORKDIR /usr/src/identity_server

RUN dotnet restore

ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT Development

EXPOSE 5000

CMD ["/bin/bash", "-c", "dotnet run --no-launch-profile"]
