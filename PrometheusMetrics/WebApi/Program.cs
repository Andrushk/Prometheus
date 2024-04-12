using OpenTelemetry.Metrics;
using WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<GreetingMetrics>();

builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddOpenTelemetry()
    .WithMetrics(b =>
    {
        b.AddPrometheusExporter();
        b.AddAspNetCoreInstrumentation();
        b.AddHttpClientInstrumentation();
        b.AddRuntimeInstrumentation();
        b.AddMeter("My.Greetings");
    });

//Если нужна детальная настройка
//builder.Services.AddOpenTelemetry()
//    .ConfigureResource(r => r.AddService(serviceName: builder.Environment.ApplicationName))
//    .WithMetrics(mb =>
//    {
//        mb.AddPrometheusExporter();
//        mb.AddMeter("My.Greetings");
//        mb.AddMeter("Microsoft.AspNetCore.Hosting");
//        mb.AddMeter("Microsoft.AspNetCore.Routing");
//        mb.AddMeter("Microsoft.AspNetCore.Diagnostics");
//        mb.AddMeter("Microsoft.AspNetCore.RateLimiting"); // пусто
//        mb.AddMeter("Microsoft.AspNetCore.HeaderParsing"); // пусто
//        mb.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
//        mb.AddMeter("Microsoft.AspNetCore.Http.Connections"); // пусто
//        mb.AddMeter("System.Net.NameResolution"); // пусто
//        mb.AddMeter("System.Net.Http"); // пусто
//        mb.AddMeter("System.Net.Sockets"); // пусто
//        mb.AddMeter("System.Net.Security"); // пусто
//        mb.AddMeter("System.Runtime"); // пусто
//        mb.AddMeter("Microsoft.Extensions.Diagnostics.HealthChecks"); // пусто
//        mb.AddMeter("Microsoft.Extensions.Diagnostics.ResourceMonitoring"); // пусто
//    });

var app = builder.Build();
app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseHttpsRedirection();

app.MapGet("/", () => $"Hello World! {DateTime.Now}");

app.MapGet("/hello/{name}", (string? name, GreetingMetrics metrics) =>
{
    var correctName = name ?? "noname";
    metrics.Greet(correctName);
    return $"Hello {correctName}! {DateTime.Now}";
});

// Можно добавить тестовых эндпоинтов как в уроке от Слерм:
// https://github.com/s-buhar0v/4-golden-signals-demo/tree/main
// но они скучные в том смысле что либо всегда выдают ошибку, либо всегда ок
//app.MapGet("/code-2xx", Helpers.Random2xx);
//app.MapGet("/code-4xx", Helpers.Random4xx);
//app.MapGet("/code-5xx", Helpers.Random5xx);
//app.MapGet("/ms-200", () => Helpers.RandomDurationMs(200));
//app.MapGet("/ms-500", () => Helpers.RandomDurationMs(500));
//app.MapGet("/ms-1000", () => Helpers.RandomDurationMs(1000));

// и еще несколько эндпоинтов более приближенных к реальности
// (могут и нормально отработать, а могут залипнуть или вернуть ошибку)
// mix5 - ошибок не более 5%, mix10 - ошибок не более 10%
app.MapGet("/mix5", () => Helpers.Mix(1000, 5));
app.MapGet("/mix10", () => Helpers.Mix(1000, 10));

app.Run();