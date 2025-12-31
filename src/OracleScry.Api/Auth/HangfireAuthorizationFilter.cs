using System.Text;
using Hangfire.Dashboard;

namespace OracleScry.Api.Auth;

/// <summary>
/// Basic authentication filter for Hangfire dashboard.
/// Credentials are configured in appsettings.json.
/// </summary>
public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    private readonly string _username;
    private readonly string _password;

    public HangfireAuthorizationFilter(string username, string password)
    {
        _username = username;
        _password = password;
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();

        // Check for Authorization header
        var authHeader = httpContext.Request.Headers.Authorization.FirstOrDefault();

        if (authHeader is not null && authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            var encodedCredentials = authHeader["Basic ".Length..].Trim();

            try
            {
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                var parts = credentials.Split(':', 2);

                if (parts.Length == 2 && parts[0] == _username && parts[1] == _password)
                {
                    return true;
                }
            }
            catch
            {
                // Invalid base64, fall through to request auth
            }
        }

        // Request authentication
        httpContext.Response.Headers.WWWAuthenticate = "Basic realm=\"Hangfire Dashboard\"";
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return false;
    }
}
