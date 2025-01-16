using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Application.Utilities.Constants;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Token
{
    public class TokenGenerator : ITokenGeneratorService
    {
        private readonly IConfiguration configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Application.DTOs.Token CreateAccesToken(
            int second,
            Customer customer,
            IList<string> userRoles
        )
        {
            Application.DTOs.Token token = new();

            SymmetricSecurityKey securityKey =
                new(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"]));

            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            token.Expiration = DateTime.UtcNow.AddSeconds(second);

            JwtSecurityToken securityToken =
                new(
                    audience: configuration["Token:Issuer"],
                    issuer: configuration["Token:Issuer"],
                    expires: token.Expiration,
                    signingCredentials: signingCredentials,
                    claims: new List<Claim>
                    {
                        new(ClaimTypes.Email, customer.Email),
                        new(ClaimTypes.Role, userRoles.FirstOrDefault() ?? Roles.Customer),
                    }
                );
            JwtSecurityTokenHandler tokenHandler = new();

            token.AccessToken = tokenHandler.WriteToken(securityToken);

            return token;
        }
    }
}
