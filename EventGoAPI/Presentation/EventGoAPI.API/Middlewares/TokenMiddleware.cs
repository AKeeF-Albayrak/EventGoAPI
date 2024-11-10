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
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                // Token'i kullanarak herhangi bir işlem yapabilirsiniz
                // Örneğin, kara liste kontrolü burada yapılabilir veya belirli bir veri konteynerine eklenebilir
                context.Items["Token"] = token;
            }

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
