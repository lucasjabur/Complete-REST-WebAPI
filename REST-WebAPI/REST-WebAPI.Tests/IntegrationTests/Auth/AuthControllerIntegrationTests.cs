using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace REST_WebAPI.Tests.IntegrationTests.Auth {

    [TestCaseOrderer(
        TestConfig.TestCaseOrdererFullName, TestConfig.TestCaseOrdererAssembly)]

    public class AuthControllerIntegrationTests : IClassFixture<SqlServerFixture> {
        private readonly HttpClient _httpClient;
        private static TokenDTO? _token;
        private static AccountCredentialsDTO? _createdUser;

        public AuthControllerIntegrationTests(SqlServerFixture sqlFixture) {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions {
                    BaseAddress = new Uri("http://localhost")
                }
            );
        }

        [Fact(DisplayName = "01 - Create User")]
        [TestPriority(1)]
        public async Task CreateUser_ShouldReturnCreatedUser() {
            var request = new AccountCredentialsDTO {
                Username = "solomon",
                Fullname = "Solomon Hykes",
                Password = "Test@1234"
            };

            var response = await _httpClient.PostAsJsonAsync(
                "/api/auth/create-user", request);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AccountCredentialsDTO>();

            result.Should().NotBeNull();
            result.Username.Should().Be("solomon");
            result.Fullname.Should().Be("Solomon Hykes");

            _createdUser = result;
        }

        [Fact(DisplayName = "02 - Sign In")]
        [TestPriority(2)]
        public async Task SignIn_ShouldReturnToken() {
            var credentials = new UserDTO {
                Username = "solomon",
                Password = "Test@1234"
            };

            var response = await _httpClient.PostAsJsonAsync(
                "/api/auth/sign-in", credentials);

            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<TokenDTO>();

            token.Should().NotBeNull();
            token.AccessToken.Should().NotBeNullOrWhiteSpace();
            token.RefreshToken.Should().NotBeNullOrWhiteSpace();

            _token = token;
        }

        [Fact(DisplayName = "03 - Refresh Token")]
        [TestPriority(3)]
        public async Task RefreshToken_ShouldReturnNewToken() {

            if (_token == null) {
                var credentials = new UserDTO { Username = "solomon", Password = "Test@1234" };
                var signInResp = await _httpClient.PostAsJsonAsync("/api/auth/sign-in", credentials);

                if (signInResp.StatusCode == HttpStatusCode.Unauthorized) {
                    var createReq = new AccountCredentialsDTO { Username = "solomon", Fullname = "Solomon Hykes", Password = "Test@1234" };
                    var createResp = await _httpClient.PostAsJsonAsync("/api/auth/create-user", createReq);
                    createResp.EnsureSuccessStatusCode();
                    signInResp = await _httpClient.PostAsJsonAsync("/api/auth/sign-in", credentials);
                }

                signInResp.EnsureSuccessStatusCode();
                _token = await signInResp.Content.ReadFromJsonAsync<TokenDTO>();
            }

            var response = await _httpClient.PostAsJsonAsync(
                "/api/auth/refresh-token", _token);

            response.EnsureSuccessStatusCode();

            var newToken = await response.Content.ReadFromJsonAsync<TokenDTO>();

            newToken.Should().NotBeNull();
            newToken.AccessToken.Should().NotBeNullOrWhiteSpace();
            newToken.RefreshToken.Should().NotBeNullOrWhiteSpace();

            newToken.AccessToken.Should().NotBeSameAs(_token?.AccessToken);
            newToken.RefreshToken.Should().NotBeSameAs(_token?.RefreshToken);

            _token = newToken;
        }

        [Fact(DisplayName = "04 - Revoke Token")]
        [TestPriority(4)]
        public async Task RevokeToken_ShouldReturnNoContent() {
            if (_token == null) {
                var credentials = new UserDTO {
                    Username = "solomon",
                    Password = "Test@1234"
                };

                var signInResp = await _httpClient.PostAsJsonAsync(
                    "/api/auth/sign-in", credentials);

                if (signInResp.StatusCode == HttpStatusCode.Unauthorized) {
                    var createReq = new AccountCredentialsDTO { Username = "solomon", Fullname = "Solomon Hykes", Password = "Test@1234" };
                    var createResp = await _httpClient.PostAsJsonAsync("/api/auth/create-user", createReq);
                    createResp.EnsureSuccessStatusCode();
                    signInResp = await _httpClient.PostAsJsonAsync("/api/auth/sign-in", credentials);
                }

                signInResp.EnsureSuccessStatusCode();

                _token = await signInResp.Content.ReadFromJsonAsync<TokenDTO>();
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            var response = await _httpClient.PostAsync("/api/auth/revoke-token", null);

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "05 - Sign In With Invalid Credentials")]
        [TestPriority(5)]
        public async Task SignIn_WithInvalidCredentials_ShouldReturnUnauthorized() {
            var credentials = new UserDTO {
                Username = "solomon",
                Password = "WrongPassword"
            };
            var response = await _httpClient.PostAsJsonAsync(
                "/api/auth/sign-in", credentials);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact(DisplayName = "06 - Revoke Token Without Authorization Header")]
        [TestPriority(6)]
        public async Task RefreshToken_WithoutAuthorizationHeader_ShouldReturnUnauthorized() {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            var response = await _httpClient.PostAsync(
                "/api/auth/revoke-token", null);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
