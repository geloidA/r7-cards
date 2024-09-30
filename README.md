Проект представляет собой визуальную обертку над api, предоставляемым onlyoffice (что касается проектов)

> Onlyoffice и R7-Office - взаимозаменяемые термины.

# Структура проекта

- [Cardmngr.AppHost](/Cardmngr.AppHost) (.NET Aspire оркестратор см. [.NET Aspire docs](https://learn.microsoft.com/en-us/dotnet/aspire/));
- [Cardmngr.Application](/Cardmngr.Application) (классы-обертки над классами из `Onlyoffice.Api`);
- [Cardmngr.Domain](/Cardmngr.Domain) (классы предметной области приложения);
- [Cardmngr.FeedbackService](/Cardmngr.FeedbackService) (webapi для функциональности обратной связи в приложении);
- [Cardmngr.Reports](/Cardmngr.Reports) (классы использующиеся для создания отчетов в приложении);
- [Cardmngr.ServiceDefaults](/Cardmngr.ServiceDefaults) (необходим для `Cardmngr.AppHost`);
- Onlyoffice
  - [Onlyoffice.Api](/Onlyoffice/Onlyoffice.Api) (классы для работы с onlyofficeapi);
  - [Onlyoffice.ProxyServer](/Onlyoffice/Onlyoffice.ProxyServer) (приложение перенаправляющее запросы от blazorwasm → onlyofficeapi);
- WebUI
  - [Cardmngr](/WebUI/Cardmngr) (blazorwasm приложение);
  - [Cardmngr.Server](/WebUI/Cardmngr.Server) (kernel сервер приложения);
  - [Cardmngr.Shared](/WebUI/Cardmngr.Shared) (общий для сервера и приложения код).

# Общий принцип работы

![interaction scheme](/doc-images/interaction-scheme.png)

Как видно из диаграммы, [Cardmngr.Server](/WebUI/Cardmngr.Server) является фасадом к другим частям системы для клиента. (возможно лучше сделать, чтобы клиент делал запросы напрямую) Вдобавок к этому, сервер содержит [код](/WebUI/Cardmngr.Server/Hubs/) SignalR взаимодействия.

Все преобразования данных из R7-структур в собственные структуры-данных, происходит на стороне клиента. Т.о. клиент отправляет запрос серверу, который перенаправляет его в proxy-сервер, который перенаправляет его на R7-сервер. Ответ, пройдя обратный путь, достигает клиента, где преобразуется в "удобоваримые" структуры-данных, которые затем отображаются в UI.

Сертификаты для TLS/SSL подключения [хранятся](/WebUI/Cardmngr.Server/config/) вместе с сервером. Применяются они в [Program.cs](/WebUI/Cardmngr.Server/Program.cs) сервера.

```cs
opt.Listen(host, address.Port, listenOptions =>
{
    listenOptions.UseHttps(X509Certificate2.CreateFromPemFile(certificatePath, keyCertificate));
});
```

[Сервис отзывов](Cardmngr.FeedbackService) сохраняет свое состояние напрямую в json-файле на host-машине.

# Клиентское приложение

Приложение использует css-движок [Tailwindcss](https://tailwindcss.com/docs/), поэтому для изменения стилей в приложении необходимо его запустить:

```bash
npx tailwindcss -i wwwroot/css/site.css -o wwwroot/css/site.min.css -w
```

Следует также упомянуть о существования проекта KolBlazor, который содержит важные razor-компоненты для работы приложения.

# Разработка

Для работы проекта необходимо настроить подключения к R7-серверу (или onlyoffice). Этот сервер нужно развернуть локально.

Где настраиваются подключения: 

1. Клиентское приложение ([appsettings.json](/WebUI/Cardmngr/wwwroot/appsettings.json), [appsettings.Development.json](/WebUI/Cardmngr/wwwroot/appsettings.json))
2. Onlyoffice-Proxy ([appsettings.Production.json](/Onlyoffice/Onlyoffice.ProxyServer/appsettings.Production.json), [appsettings.Development.json](/Onlyoffice/Onlyoffice.ProxyServer/appsettings.Development.json))

Чтобы запустить проект для отладки нужно:

1. Удостоверится в работающем Onlyoffice/R7-Сервере и правильности адресов, указанных в файлах-конфигурациях;
2. Запустить команду `dotnet watch` в проекте [Cardmngr.AppHost](/Cardmngr.AppHost);
3. Запустить css-движок из директории `WebUI/Cardmngr/`.

Процесс разработки оставляет желать лучшего, но как настроить его лучше я не знаю...

# Развертывание

Для развертывания приложения создан [скрипт](/release.sh).

Этот скрипт на Bash предназначен для автоматизации процесса сборки и развертывания Docker-образов для приложения. Он выполняет следующие действия:

1. Читает версию из файла [appversion](WebUI/Cardmngr.Server/appversion);
2. Создает папку для релиза с именем release-<$VERSION>
3. Экспортирует переменные VERSION и HOST_FEEDBACK_DIR;
4. Копирует шаблон [docker-compose.yml](/docker-compose.yml.build.template) и заменяет переменную версии;
5. Собирает Docker-образы с помощью docker-compose build;
6. Сохраняет Docker-образы в tar.gz файлы и удаляет их;
7. Копирует скрипт для развертывания в папку релиза;
8. Копирует шаблон [docker-compose.yml](/docker-compose.yml.deploy.template) и заменяет переменную версии для развертывания.

Скрипт также проверяет наличие обязательных файлов и переменных, и выводит сообщения об ошибках, если что-то идет не так.

Поэтому для развертывания необходимо перенести созданную папку в deploy-окружение, где установлен docker, и выполнить скрипт deploy.sh.
