using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using HealthCare.Domain.Shared.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace HealthCare.Middleware
{
    public class TimeOutMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TimeSpan _requestTimeout;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TimeOutMiddleware(
            RequestDelegate next,
            TimeSpan requestTimeout,
            IServiceScopeFactory serviceScopeFactory
        ) {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
            _requestTimeout = requestTimeout;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var authService = scope.ServiceProvider.GetRequiredService<AuthenticationService>();

                var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                if (!string.IsNullOrEmpty(token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                    var authClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "type" && c.Value == "auth");

                    if ( jwtToken.Claims.Any(c => c.Type == "type" && c.Value == "auth") )
                    {
                        var uidClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "uid");

                        if ( await authService.UserTimedOut(uidClaim.Value, _requestTimeout) )
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            await context.Response.WriteAsync("Session expired due to inactivity");
                            return;
                        }
                    }
                }

                await _next(context);
            }
        }
    }
}