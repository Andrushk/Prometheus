## ASP.NET Web Api + Prometheus + Grafana
по мотивам https://github.com/s-buhar0v/4-golden-signals-demo но на c#

### Требования
- Visual Studio
- Docker + docker-compose

### Структура репозитория
```
├── Configs - конфигурация Prometheus
├── Client  - приложение создающее тестовую нагрузку на WebApi
└── WebApi  - демо-приложение, отдающее метрики
```