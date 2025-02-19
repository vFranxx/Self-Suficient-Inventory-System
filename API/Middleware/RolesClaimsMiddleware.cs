using System.Security.Claims;
using System.Text.Json;

public class RolesMiddleware
{
    private readonly RequestDelegate _next;

    public RolesMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var roleClaim = context.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (roleClaim != null)
            {
                var roles = JsonSerializer.Deserialize<List<string>>(roleClaim.Value);
                var identity = (ClaimsIdentity)context.User.Identity;

                foreach (var role in roles)
                {
                    if (!identity.HasClaim(ClaimTypes.Role, role))
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, role));
                    }
                }
            }
        }

        await _next(context);
    }
}