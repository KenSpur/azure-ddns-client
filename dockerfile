FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish AzureDDNSClient/AzureDDNSClient.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /App/out .

COPY entrypoint.sh ./
RUN chmod +x entrypoint.sh

ENTRYPOINT ["/bin/bash", "./entrypoint.sh"]