using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace REST_Services
{

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var authenticationHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authenticationHeader) || !authenticationHeader.StartsWith("Basic "))
                {
                    return AuthenticateResult.Fail("Authorization header missing or malformed.");
                }

                var authHeaderValue = authenticationHeader.Substring("Basic ".Length).TrimEnd();
                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue)).Split(':');
                var username = credentials.FirstOrDefault();
                var password = credentials.LastOrDefault();

                // Here, you would validate username and password against your user store (e.g., database)
                //var userService = Context.RequestServices.GetRequiredService<IUserService>();
                //var user = await userService.Authenticate(username, password);

                //if (user == null)
                //{
                //    return AuthenticateResult.Fail("Invalid username or password.");
                //}

                if (!(username.ToLower() == "anaiyaan" && password == "Anaiyaan@123"))
                {
                    // returns unauthorized error  
                    return AuthenticateResult.Fail("Invalid username or password.");
                }

                var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "anaiyaan"),
                new Claim(ClaimTypes.Name, "anaiyaan"),
                };

                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            catch (Exception)
            {
                return AuthenticateResult.Fail("Error during authentication.");
            }
        }
    }
}
