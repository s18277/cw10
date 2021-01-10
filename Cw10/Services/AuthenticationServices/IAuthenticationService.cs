using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cw10.Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        JwtSecurityToken Login(string username, string password);
        int UpdateRefreshToken(string username, string refreshToken);
        ClaimsPrincipal ValidateJwtAndGetClaimsPrincipal(string jwt);
        JwtSecurityToken RefreshJwt(string username, string refreshToken);
    }
}