using OpenTelemetry.Metrics;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

// Можно запустить мониторинг в PowerShell:
// dotnet-counters monitor -n WebApi --counters Microsoft.AspNetCore.Hosting, System.Runtime

builder.Services.AddOpenTelemetry().WithMetrics(b =>
{
    b.AddPrometheusExporter();
    b.AddMeter("Microsoft.AspNetCore.Hosting",
        "Microsoft.AspNetCore.Server.Kestrel");
    b.AddView("http.server.request.duration",
        new ExplicitBucketHistogramConfiguration
        {
            Boundaries = new[]
            {
                0, 0.005, 0.01, 0.025, 0.05,
                0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10
            }
        });
});

var app = builder.Build();
app.MapPrometheusScrapingEndpoint();
app.UseHttpsRedirection();

app.MapGet("/", () => $"Hello World! {DateTime.Now}");

// Делаем тестовые эндпоинты как в уроке от Слерм
// https://github.com/s-buhar0v/4-golden-signals-demo/tree/main
app.MapGet("/code-2xx", Helpers.Random2xx);
app.MapGet("/code-4xx", Helpers.Random4xx);
app.MapGet("/code-5xx", Helpers.Random5xx);
app.MapGet("/ms-200", () => Helpers.RandomDurationMs(200));
app.MapGet("/ms-500", () => Helpers.RandomDurationMs(500));
app.MapGet("/ms-1000", () => Helpers.RandomDurationMs(1000));

app.Run();