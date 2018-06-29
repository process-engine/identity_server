FROM microsoft/dotnet:2.0.0-sdk

RUN apt-get update && apt-get install -y curl git

WORKDIR /usr/src

# RUN git clone --branch master https://github.com/process-engine/identity_server.git

COPY . /usr/src/identity_server

WORKDIR /usr/src/identity_server

RUN dotnet restore

ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

CMD ["/bin/bash", "-c", "dotnet run --launch-profile 'Docker'"]
