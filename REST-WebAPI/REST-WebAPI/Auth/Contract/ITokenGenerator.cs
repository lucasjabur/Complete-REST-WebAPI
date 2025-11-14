using System.Security.Claims;

namespace REST_WebAPI.Auth.Contract {
    public interface ITokenGenerator {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
