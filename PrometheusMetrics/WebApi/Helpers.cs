using StatusCodes = Microsoft.AspNetCore.Http.StatusCodes;

namespace WebApi
{
    public static class Helpers
    {
        private static readonly Random Rnd = new();

        public static readonly int[] OkCodes =
        {
            StatusCodes.Status200OK,
            StatusCodes.Status202Accepted
        };

        public static readonly int[] ErrorCodes =
        {
            StatusCodes.Status400BadRequest,
            StatusCodes.Status401Unauthorized,
            StatusCodes.Status404NotFound,
            StatusCodes.Status429TooManyRequests
        };

        public static readonly int[] ServerErrorCodes =
        {
            StatusCodes.Status500InternalServerError,
            StatusCodes.Status501NotImplemented,
            StatusCodes.Status503ServiceUnavailable,
            StatusCodes.Status504GatewayTimeout
        };

        public static IResult Random2xx()
        {
            return Results.StatusCode(OkCodes[Rnd.Next(OkCodes.Length)]);
        }

        public static IResult Random4xx()
        {
            return Results.StatusCode(ErrorCodes[Rnd.Next(ErrorCodes.Length)]);
        }

        public static IResult Random5xx()
        {
            return Results.StatusCode(ServerErrorCodes[Rnd.Next(ServerErrorCodes.Length)]);
        }

        public static async Task<IResult> RandomDurationMs(int duration)
        {
            await Task.Delay(Rnd.Next(duration) + duration);
            return Results.Ok();
        }

        public static async Task<IResult> Mix(int duration, int errorsPercent)
        {
            // типа работаем
            await Task.Delay(Rnd.Next(duration) + duration);

            // тут решаем возвращать ОК или ОШИБКУ
            if (Rnd.Next(100) < errorsPercent)
            {
                // ошибки могут быть разными, 500 кидаем чаще чем остальные
                switch (Rnd.Next(8))
                {
                    case < 4:
                        return Random5xx();
                    case >= 4 and < 7:
                        return Random4xx();
                    case 7:
                        throw new Exception("Страшная не перехваченная ошибка");
                }
            }

            // иногда вдруг задача оказывается ОЧЕНЬ тяжелой
            if (Rnd.Next(50) == 42)
            {
                await Task.Delay(TimeSpan.FromSeconds(60 + Rnd.Next(100)));
            }

            return Random2xx();
        }
    }
}
