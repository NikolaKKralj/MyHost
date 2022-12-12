using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyHostAPI.Common.Configurations;
using MyHostAPI.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyHostAPI.Common.Helpers
{
    public static class TokenHandler
    {
        public static string CreateToken(string email, JwtSection jwtSection, string userId, Role role)
        {
            var TokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(jwtSection.JWTKey);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email,email),
                    new Claim("id", userId),
                    new Claim(ClaimTypes.Role, role.ToString())
                }),

                Audience = jwtSection.ValidAudience,

                Issuer = jwtSection.ValidIssuer,

                Expires = DateTime.UtcNow.AddYears(10),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature
                    )
            };

            var token = TokenHandler.CreateToken(tokenDescriptor);      

            return TokenHandler.WriteToken(token);
        }

        public static string? ValidateJwtToken(string token, JwtSection jwtSection)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSection.JWTKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value.ToString();

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
    }
}
