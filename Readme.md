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
    ├── Configs       - конфигурация Prometheus
    ├── Client        - приложение создающее тестовую нагрузку на WebApi
    ├── WebApi        - демо-приложение, отдающее метрики
    └── docker-compose.yml
``` 