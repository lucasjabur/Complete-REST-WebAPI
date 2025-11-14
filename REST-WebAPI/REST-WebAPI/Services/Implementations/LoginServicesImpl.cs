using Microsoft.AspNetCore.Identity;
using REST_WebAPI.Auth.Configuration;
using REST_WebAPI.Auth.Contract;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace REST_WebAPI.Services.Implementations {
    public class LoginServicesImpl : ILoginServices {
        
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private readonly IUserAuthServices _userAuthService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenService;
        private readonly TokenConfig _configurations;

        public LoginServicesImpl(
            IUserAuthServices userAuthService,
            IPasswordHasher passwordHasher,
            ITokenGenerator tokenService,
            TokenConfig configurations)
        {
            _userAuthService = userAuthService;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _configurations = configurations;
        }

        public AccountCredentialsDTO Create(AccountCredentialsDTO dto) {
            var user = _userAuthService.Create(dto);
            return new AccountCredentialsDTO {
                Username = user.Username,
                Fullname = user.FullName,
                Password = "************"
            };
        }

        public TokenDTO ValidateCredentials(UserDTO dto) {
            var user = _userAuthService.FindByUsername(dto.Username);
            if (user == null) {
                return null;
            }

            if (!_passwordHasher.Verify(dto.Password, user.Password)) {
                return null;
            }

            return GenerateToken(user);
        }

        public TokenDTO ValidateCredentials(TokenDTO token) {
            var principal = _tokenService.GetPrincipalFromExpiredToken(token.AccessToken);
            var username = principal.Identity?.Name;
            var user = _userAuthService.FindByUsername(username);

            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now) {
                return null;
            }

            return GenerateToken(user, principal.Claims);
        }

        public bool RevokeToken(string username) {
            return _userAuthService.RevokeToken(username);
        }

        private TokenDTO GenerateToken(User user, IEnumerable<Claim>? existingClaims = null) {
            var claims = existingClaims?.ToList() ?? new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")), // convert guid in text without hyphens
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_configurations.DaysToExpiry);

            _userAuthService.Update(user);

            var createdDate = DateTime.Now;
            var expirationDate = createdDate.AddMinutes(_configurations.Minutes);

            return new TokenDTO {
                Authenticated = true,
                Created = createdDate.ToString(DATE_FORMAT),
                Expiration = expirationDate.ToString(DATE_FORMAT),
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
