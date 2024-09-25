FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

COPY "Onlyoffice/Onlyoffice.Api/Onlyoffice.Api.csproj" "Onlyoffice/Onlyoffice.Api/Onlyoffice.Api.csproj"
COPY "WebUI/Cardmngr/Cardmngr.csproj" "WebUI/Cardmngr/Cardmngr.csproj"
COPY "WebUI/Cardmngr.Server/Cardmngr.Server.csproj" "WebUI/Cardmngr.Server/Cardmngr.Server.csproj"
COPY "WebUI/Cardmngr.Shared/Cardmngr.Shared.csproj" "WebUI/Cardmngr.Shared/Cardmngr.Shared.csproj"
COPY "Cardmngr.Application/Cardmngr.Application.csproj" "Cardmngr.Application/Cardmngr.Application.csproj"
COPY "Cardmngr.Domain/Cardmngr.Domain.csproj" "Cardmngr.Domain/Cardmngr.Domain.csproj"
COPY "Cardmngr.Reports/Cardmngr.Reports.csproj" "Cardmngr.Reports/Cardmngr.Reports.csproj"
COPY "Cardmngr.ServiceDefaults/Cardmngr.ServiceDefaults.csproj" "Cardmngr.ServiceDefaults/Cardmngr.ServiceDefaults.csproj"

# Escape long restore
ENV DOTNET_NUGET_SIGNATURE_VERIFICATION=false

RUN dotnet restore "WebUI/Cardmngr.Server/Cardmngr.Server.csproj"

COPY . .
WORKDIR /src/WebUI/Cardmngr.Server
RUN dotnet publish --no-restore -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build /src/WebUI/Cardmngr.Server/out .
ENTRYPOINT ["dotnet", "Cardmngr.Server.dll"]