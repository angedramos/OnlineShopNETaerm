using Microsoft.IdentityModel.Tokens;
using OnlineShopNET.Domain.Config;
using OnlineShopNET.Domain.Entities;
using OnlineShopNET.Infrastructure.Jwt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineShopNET.JwtService
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService( IConfiguration configuration)
        {
                _configuration = configuration;
        }
        public string GenerateJwtToken(Task<User> user)
        {
            var jwt = _configuration.GetSection("JwtSettings").Get<JwtClass>();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Result.user_id.ToString()),
                new Claim(ClaimTypes.Name, user.Result.username),
                new Claim(ClaimTypes.Email, user.Result.email),
                new Claim(ClaimTypes.Role, user.Result.role_type.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
            expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal DecodeJwtToken(string token)
        {
            var jwt = _configuration.GetSection("JwtSettings").Get<JwtClass>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwt.Key);

            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal;
        }

        public string GetTokenFromHeader (string token)
        {
            if (token == null)
            {
                return Constant_Messages.NULL_TOKEN;
            }
            string bearerToken = token.Substring("Bearer ".Length).Trim();
            return bearerToken;
        }
    }
}
