using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Application.Utilities.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Persistence.middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration configuration;

        public AuthMiddleware(RequestDelegate _next, IConfiguration configuration)
        {
            next = _next;
            this.configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.ToString().Contains("Auth"))
            {
                await next(context);
                return;
            }

            var accessToken = context.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(accessToken) || !ValidateAccessToken(accessToken))
            {
                await UnAuth(context);
                return;
            }

            string email = GetEmailFromClaims(accessToken);
            context.Items["Email"] = email;
            string role = GetRoleFromClaims(accessToken);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role ?? Roles.Customer),
                new Claim(ClaimTypes.Email, email),
            };

            var identity = new ClaimsIdentity(claims, "Bearer");
            context.User = new ClaimsPrincipal(identity);

            await next(context);
        }

        private string GetRoleFromClaims(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"])
            );
            var tokenValidationParameters = GetTokenValidationParameters(securityKey);

            var claimsPrincipal = tokenHandler.ValidateToken(
                accessToken.Split("Bearer ")[1],
                tokenValidationParameters,
                out _
            );

            return claimsPrincipal?.Claims.First(claim => claim.Type == ClaimTypes.Role)?.Value;
        }

        private bool ValidateAccessToken(string accessToken)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"])
                );
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenValidationParameters = GetTokenValidationParameters(securityKey);

                tokenHandler.ValidateToken(
                    accessToken.Split("Bearer ")[1],
                    tokenValidationParameters,
                    out _
                );
                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                // TODO generate token
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private TokenValidationParameters GetTokenValidationParameters(SecurityKey securityKey)
        {
            return new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateIssuer = true,
                ValidAudience = configuration["Token:Issuer"],
                ValidIssuer = configuration["Token:Issuer"],
                ValidateAudience = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
            };
        }

        private string GetEmailFromClaims(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"])
            );
            var tokenValidationParameters = GetTokenValidationParameters(securityKey);

            var claimsPrincipal = tokenHandler.ValidateToken(
                accessToken.Split("Bearer ")[1],
                tokenValidationParameters,
                out _
            );

            return claimsPrincipal?.Claims.First(claim => claim.Type == ClaimTypes.Email)?.Value;
        }

        private static async Task UnAuth(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}
