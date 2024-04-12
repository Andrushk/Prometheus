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

//���� ����� ��������� ���������
//builder.Services.AddOpenTelemetry()
//    .ConfigureResource(r => r.AddService(serviceName: builder.Environment.ApplicationName))
//    .WithMetrics(mb =>
//    {
//        mb.AddPrometheusExporter();
//        mb.AddMeter("My.Greetings");
//        mb.AddMeter("Microsoft.AspNetCore.Hosting");
//        mb.AddMeter("Microsoft.AspNetCore.Routing");
//        mb.AddMeter("Microsoft.AspNetCore.Diagnostics");
//        mb.AddMeter("Microsoft.AspNetCore.RateLimiting"); // �����
//        mb.AddMeter("Microsoft.AspNetCore.HeaderParsing"); // �����
//        mb.AddMeter("Microsoft.AspNetCore.Server.Kestrel");
//        mb.AddMeter("Microsoft.AspNetCore.Http.Connections"); // �����
//        mb.AddMeter("System.Net.NameResolution"); // �����
//        mb.AddMeter("System.Net.Http"); // �����
//        mb.AddMeter("System.Net.Sockets"); // �����
//        mb.AddMeter("System.Net.Security"); // �����
//        mb.AddMeter("System.Runtime"); // �����
//        mb.AddMeter("Microsoft.Extensions.Diagnostics.HealthChecks"); // �����
//        mb.AddMeter("Microsoft.Extensions.Diagnostics.ResourceMonitoring"); // �����
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

// ����� �������� �������� ���������� ��� � ����� �� �����:
// https://github.com/s-buhar0v/4-golden-signals-demo/tree/main
// �� ��� ������� � ��� ������ ��� ���� ������ ������ ������, ���� ������ ��
//app.MapGet("/code-2xx", Helpers.Random2xx);
//app.MapGet("/code-4xx", Helpers.Random4xx);
//app.MapGet("/code-5xx", Helpers.Random5xx);
//app.MapGet("/ms-200", () => Helpers.RandomDurationMs(200));
//app.MapGet("/ms-500", () => Helpers.RandomDurationMs(500));
//app.MapGet("/ms-1000", () => Helpers.RandomDurationMs(1000));

// � ��� ��������� ���������� ����� ������������ � ����������
// (����� � ��������� ����������, � ����� ��������� ��� ������� ������)
// mix5 - ������ �� ����� 5%, mix10 - ������ �� ����� 10%
app.MapGet("/mix5", () => Helpers.Mix(1000, 5));
app.MapGet("/mix10", () => Helpers.Mix(1000, 10));

app.Run();