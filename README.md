# r7-cards

визуальная обертка над api, предоставляемым onlyoffice (по части проектов)

## project structure

на данный момент приложение состоит из:

- `Cardmngr.AppHost` (.NET Aspire оркестратор см. [.NET Aspire docs](https://learn.microsoft.com/en-us/dotnet/aspire/))
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

## deployment

приложение развертывается в kubernetes кластере см. [Kubernetes docs](https://kubernetes.io/docs/home/)

как делал я:

1. Развернуть локально kubernetes кластер с помощью [kind](https://kind.sigs.k8s.io/docs/user/working-offline/);
2. С помощью aspirate dotnet tool (см. [aspirate repo](https://github.com/prom3theu5/aspirational-manifests)) создал kubernetes конфигурацию (в окружении без доступа к Интернет вручную переносил все необходимые docker-контейнеры);
3. Добавил к имеющимся файлам некоторые собственные (напр. [configmap.yaml](Cardmngr.AppHost/aspirate-output/configmap.yaml) хранит сертификаты для `Cardmngr.Server` приложения);
4. выполнил скрипт [release.sh](/release.sh);
5. выполнил скрипт deploy.sh
6. Используя kubectl развернул приложение одной командой
```bash
kubectl apply -k /aspirate-output
```
