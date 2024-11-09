using System.Security.Claims;

namespace EventGoAPI.API.Middlewares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;

            if (Guid.TryParse(userId, out var userIdGuid))
            {
                context.Items["UserId"] = userIdGuid;
            }

            if (userRole != null)
            {
                context.Items["UserRole"] = userRole;
            }

            await _next(context);
        }
    }
}
