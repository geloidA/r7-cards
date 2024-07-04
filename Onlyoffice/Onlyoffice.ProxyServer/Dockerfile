FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

RUN dotnet workload install aspire

COPY "Cardmngr.sln" "Cardmngr.sln"

COPY "games/Cardmngr.Tetris/Cardmngr.Tetris.csproj" "games/Cardmngr.Tetris/Cardmngr.Tetris.csproj"
COPY "Onlyoffice/Onlyoffice.Api/Onlyoffice.Api.csproj" "Onlyoffice/Onlyoffice.Api/Onlyoffice.Api.csproj"
COPY "Onlyoffice/Onlyoffice.ProxyServer/Onlyoffice.ProxyServer.csproj" "Onlyoffice/Onlyoffice.ProxyServer/Onlyoffice.ProxyServer.csproj"
COPY "tests/Cardmngr.Domain.Tests/Cardmngr.Domain.Tests.csproj" "tests/Cardmngr.Domain.Tests/Cardmngr.Domain.Tests.csproj"
COPY "tests/Cardmngr.Shared.Tests/Cardmngr.Shared.Tests.csproj" "tests/Cardmngr.Shared.Tests/Cardmngr.Shared.Tests.csproj"
COPY "tests/Cardmngr.Tests/Cardmngr.Tests.csproj" "tests/Cardmngr.Tests/Cardmngr.Tests.csproj"
COPY "WebUI/Cardmngr/Cardmngr.csproj" "WebUI/Cardmngr/Cardmngr.csproj"
COPY "WebUI/Cardmngr.Server/Cardmngr.Server.csproj" "WebUI/Cardmngr.Server/Cardmngr.Server.csproj"
COPY "WebUI/Cardmngr.Shared/Cardmngr.Shared.csproj" "WebUI/Cardmngr.Shared/Cardmngr.Shared.csproj"
COPY "Cardmngr.AppHost/Cardmngr.AppHost.csproj" "Cardmngr.AppHost/Cardmngr.AppHost.csproj"
COPY "Cardmngr.Application/Cardmngr.Application.csproj" "Cardmngr.Application/Cardmngr.Application.csproj"
COPY "Cardmngr.Domain/Cardmngr.Domain.csproj" "Cardmngr.Domain/Cardmngr.Domain.csproj"
COPY "Cardmngr.FeedbackService/Cardmngr.FeedbackService.csproj" "Cardmngr.FeedbackService/Cardmngr.FeedbackService.csproj"
COPY "Cardmngr.Reports/Cardmngr.Reports.csproj" "Cardmngr.Reports/Cardmngr.Reports.csproj"
COPY "Cardmngr.ServiceDefaults/Cardmngr.ServiceDefaults.csproj" "Cardmngr.ServiceDefaults/Cardmngr.ServiceDefaults.csproj"

RUN dotnet restore "Cardmngr.sln"

COPY . .
WORKDIR /src/Onlyoffice/Onlyoffice.ProxyServer
RUN dotnet publish --no-restore -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app
COPY --from=build /src/Onlyoffice/Onlyoffice.ProxyServer/out .
ENTRYPOINT ["dotnet", "Onlyoffice.ProxyServer.dll"]