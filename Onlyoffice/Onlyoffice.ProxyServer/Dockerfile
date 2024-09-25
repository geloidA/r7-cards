FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY "Onlyoffice/Onlyoffice.Api/Onlyoffice.Api.csproj" "Onlyoffice/Onlyoffice.Api/Onlyoffice.Api.csproj"
COPY "Onlyoffice/Onlyoffice.ProxyServer/Onlyoffice.ProxyServer.csproj" "Onlyoffice/Onlyoffice.ProxyServer/Onlyoffice.ProxyServer.csproj"
COPY "WebUI/Cardmngr.Shared/Cardmngr.Shared.csproj" "WebUI/Cardmngr.Shared/Cardmngr.Shared.csproj"
COPY "Cardmngr.Domain/Cardmngr.Domain.csproj" "Cardmngr.Domain/Cardmngr.Domain.csproj"
COPY "Cardmngr.ServiceDefaults/Cardmngr.ServiceDefaults.csproj" "Cardmngr.ServiceDefaults/Cardmngr.ServiceDefaults.csproj"

# Escape long restore
ENV DOTNET_NUGET_SIGNATURE_VERIFICATION=false 

RUN dotnet restore "Onlyoffice/Onlyoffice.ProxyServer/Onlyoffice.ProxyServer.csproj"

COPY . .
WORKDIR /src/Onlyoffice/Onlyoffice.ProxyServer
RUN dotnet publish --no-restore -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build /src/Onlyoffice/Onlyoffice.ProxyServer/out .
ENTRYPOINT ["dotnet", "Onlyoffice.ProxyServer.dll"]