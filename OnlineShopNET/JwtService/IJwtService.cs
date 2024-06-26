using OnlineShopNET.Domain.Entities;
using System.Security.Claims;

namespace OnlineShopNET.JwtService
{
    public interface IJwtService
    {
        string GenerateJwtToken(Task<User> user);
        ClaimsPrincipal DecodeJwtToken(string token);
        string GetTokenFromHeader(string token);
        bool IsValidJwtToken(string token);
    }
}
