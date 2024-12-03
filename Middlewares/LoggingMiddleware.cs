using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Obtein the ILoggingService inside the request context
        var loggingService = context.RequestServices.GetRequiredService<ILoggingService>();

        // Generate a unique request ID for tracking
        var requestId = Guid.NewGuid();
        var stopwatch = Stopwatch.StartNew();

        // Read the body of the HTTP request
        context.Request.EnableBuffering(); // Allows multiple reads of the body
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0; // Resets the position of the stream for future reads 

        // Formats the header of the request
        var requestHeaders = string.Join(Environment.NewLine, context.Request.Headers.Select(h => $"{h.Key}: {h.Value}"));


        // Captures the incoming request details, including HTTP version and body
        var requestLog = new RequestLog
        {
            RequestId = requestId,
            HttpMethod = context.Request.Method,
            RequestPath = context.Request.Path,
            QueryString = context.Request.QueryString.ToString(),
            
            RequestHeaders = "", // requestHeaders,  // Captures the formatted request 
            ClientIp = context.Connection.RemoteIpAddress?.ToString(),
            UserAgent = context.Request.Headers["User-Agent"].ToString(),
            RequestTime = DateTime.UtcNow,
            HttpVersion = context.Request.Protocol,  // Captures the HTTP version
            RequestBody = ""//requestBody // Captures the body of the HTTP request
        };

        // Create a temporal MemoryStream to capture the response 
        using (var memoryStream = new MemoryStream())
        {
            // Stores the original stream of the response
            var originalBodyStream = context.Response.Body;
            context.Response.Body = memoryStream;

            
            await _next(context);

            // Stops the chronometer to measure the time of the request
            stopwatch.Stop();


            
            var responseHeaders = string.Join(Environment.NewLine, context.Response.Headers.Select(h => $"{h.Key}: {h.Value}"));

           

            // Captures the details of the response
            var responseLog = new ResponseLog
            {
                RequestId = requestId,
                StatusCode = context.Response.StatusCode,
                ResponseHeaders = responseHeaders,  

                ResponseTime = DateTime.UtcNow,
                DurationMs = stopwatch.ElapsedMilliseconds,
                ServerIp = context.Connection.LocalIpAddress?.ToString(),
                ResponseSizeInBytes = memoryStream.Length 
            };

            // Rests the MemoryStream and copies the content of the original stream 
            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(originalBodyStream);
            context.Response.Body = originalBodyStream;

            // Save the request logs and the response logs asynchronously
            await loggingService.SaveLogsAsync(requestLog, responseLog);
        }
    }
}



