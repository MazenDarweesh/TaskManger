using System.Diagnostics;

namespace TaskManagementSolution.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            await _next(context);

            stopwatch.Stop();
            var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                var logLevel = context.Response.StatusCode >= 500 ? "Error" : "Warning";
                var logMessage = $"HTTP {context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode} in {elapsedMilliseconds}ms";
                if (logLevel == "Error")
                {
                    Serilog.Log.Error(logMessage);
                }
                else
                {
                    Serilog.Log.Warning(logMessage);
                }
            }
            else
            {
                Serilog.Log.Information($"HTTP {context.Request.Method} {context.Request.Path} responded {context.Response.StatusCode} in {elapsedMilliseconds}ms");
            }
        }
    }
}
