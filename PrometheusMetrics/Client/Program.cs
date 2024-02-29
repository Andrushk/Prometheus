// Клиент, создающий тестовую нагрузку на WebApi
// Лупит без остановки как бешеный

var uri = new Uri("http://webapi:8080");
var client = new HttpClient { BaseAddress = uri };

// доступные эндпоинты, на каждой итерации выбираем один случайным образом
var endpoints = new List<string> { "/code-2xx", "/code-4xx", "/code-5xx", "/ms-200", "/ms-500", "/ms-1000" };
var rnd = new Random();

Console.WriteLine($"Crazy Client started at {uri}");
while (true)
{
    var id = rnd.Next(endpoints.Count);
    _ = client.GetAsync(endpoints[id]).ContinueWith(async (r) =>
    {
        try
        {
            var response = r.Result;
            var body = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"uri={response.RequestMessage?.RequestUri}, status={response.StatusCode}, body={body}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    });
}