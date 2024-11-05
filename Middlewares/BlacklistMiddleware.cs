using Microsoft.AspNetCore.Http;
using MinimalApiLoggingApp.Services;
using System.Threading.Tasks;

namespace MinimalApiLoggingApp.Middlewares
{
    public class BlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly BlacklistStore _blacklistStore;

        public BlacklistMiddleware(RequestDelegate next, BlacklistStore blacklistStore)
        {
            _next = next;
            _blacklistStore = blacklistStore;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress?.ToString();

            // Verify if the Ip address is stoe in the IP list in memory
            if (_blacklistStore.GetBlacklistedIps().Contains(remoteIp))
            {
                
                //context.Abort();      // In case you prefer to abort instead of a 403 response
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied.");


                return; // 
            }

            await _next(context); // Continues the middleware chain if it is not a malicious IP
        }
    }


}
