# r7-cards

визуальная обертка над api, предоставляемым onlyoffice (по части проектов)

## структура проекта

на данный момент приложение состоит из:

- `Cardmngr.AppHost` (.NET Aspire оркестратор см. [.NET Aspire documentation](https://learn.microsoft.com/en-us/dotnet/aspire/))
- `Cardmngr.Application` (классы-обертки над классами из `Onlyoffice.Api`)
- `Cardmngr.Domain` (классы предметной области приложения)
- `Cardmngr.FeedbackService` (webapi для функциональности обратной связи в приложении)
- `Cardmngr.Reports` (классы использующиеся для создания отчетов в приложении)
- `Cardmngr.ServiceDefaults` (необходим для `Cardmngr.AppHost`)
- Onlyoffice
  - `Onlyoffice.Api` (классы для работы с onlyofficeapi)
  - `Onlyoffice.ProxyServer` (приложение перенаправляющее запросы от blazorwasm -> onlyofficeapi)
- WebUI
  - `Cardmngr` (blazorwasm приложение)
  - `Cardmngr.Server` (kernel сервер приложения)
  - `Cardmngr.Shared` (общий для сервера и приложения код)

