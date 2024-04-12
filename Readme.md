## ASP.NET Web Api + Prometheus + Grafana
по мотивам https://github.com/s-buhar0v/4-golden-signals-demo но на c#

### Требования
- Visual Studio
- Docker + docker-compose

### Структура репозитория
```
├── LocalMetrics - никаких контейнеров, только клиент, web api и dotnet-counters monitor
│   ├── Client   - приложение создающее тестовую нагрузку на WebApi
│   └── WebApi   - демо-приложение, отдающее метрики
│
└── PrometheusMetrics - контейнеры, Прометей, Графана и т.п.
    ├── Configs       - конфигурация Prometheus и дашборды Grafana
    ├── Client        - приложение создающее тестовую нагрузку на WebApi
    ├── WebApi        - демо-приложение, отдающее метрики
    └── docker-compose.yml
``` 

### Описание проектов

##### LocalMetrics
Демо, работающее без контейнеров. Метрики можно посмотреть в PowerShell:

```
dotnet-counters monitor -n WebApi --counters Microsoft.AspNetCore.Hosting, System.Runtime
```


##### PrometheusMetrics
Демо в котором каждое приложение в своем контейнере. 
Запускаем. Клиент начинает слать запросы на WebApi, который генерирует некую телеметрию, которую раз в 30 сек собирает Prometheus, который опрашивает Grafana, чтобы рисовать красивые дашборды.

###### Prometheus

- Если хочется на него посмотреть/понастраивать, то заходим сюда: http://localhost:9090

###### Grafana

- Заходим сюда: http://localhost:3000
- Логин/пароль: admin/admin
- Необходимо добавить источник данных: http://prometheus:9090
- Дашборды можно нарисовать самостоятельно, импортировать из папки Configs\dashboards или скачать https://grafana.com/orgs/dotnetteam


