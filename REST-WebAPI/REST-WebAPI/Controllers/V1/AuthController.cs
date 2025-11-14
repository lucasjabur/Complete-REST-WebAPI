using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Services;
using System.Runtime.CompilerServices;

namespace REST_WebAPI.Controllers.V1 {

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {
        private readonly ILoginServices _loginService;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserAuthServices _userAuthServices;

        public AuthController(ILoginServices loginService, ILogger<AuthController> logger, IUserAuthServices userAuthServices) {
            _loginService = loginService;
            _logger = logger;
            _userAuthServices = userAuthServices;
        }

        [HttpPost("sign-in")]
        [AllowAnonymous]
        public IActionResult SignIn([FromBody] UserDTO user) {
            _logger.LogInformation($"Attempting to sign in user: '{user.Username}'");
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password)) {
                _logger.LogWarning($"Sign in failed. Missing username and/or password");
                return BadRequest("Username and password are required");
            }

            var token = _loginService.ValidateCredentials(user);
            if (token == null) {
                return Unauthorized();
            }

            _logger.LogInformation($"User '{user.Username}' signed in successfully.");

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public IActionResult Refresh([FromBody] TokenDTO tokenDto) {
            if (tokenDto == null) {
                return BadRequest("Invalid client request");
            }

            var newToken = _loginService.ValidateCredentials(tokenDto);
            if (newToken == null) {
                _logger.LogWarning("Token refresh failed. Invalid tokens provided");
                return Unauthorized();
            }

            _logger.LogInformation("Token refreshed successfully.");

            return Ok(newToken);
        }

        [HttpPost("revoke-token")]
        [Authorize]
        public IActionResult Revoke() {
            var username = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(username)) {
                return BadRequest("Invalid client request");
            }
            var result = _loginService.RevokeToken(username);
            if (!result) {
                _logger.LogWarning($"Token revocation failed for user: '{username}'");
                return BadRequest("Invalid client request");
            }
            _logger.LogInformation($"Token revoked successfully for user: '{username}'");
            return NoContent();
        }

        [HttpPost("create-user")]
        [AllowAnonymous]
        public IActionResult CreateUser([FromBody] AccountCredentialsDTO user) {
            if (user == null) {
                return BadRequest("Invalid cliente request");
            }
            var result = _loginService.Create(user);
            return Ok(result);
        }
    }
}
