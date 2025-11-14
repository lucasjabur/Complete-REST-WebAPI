using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Models;
using REST_WebAPI.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace REST_WebAPI.Tests.IntegrationTests.HATEOAS.Book {

    [TestCaseOrderer(
        TestConfig.TestCaseOrdererFullName, TestConfig.TestCaseOrdererAssembly)]
    public class BookControllerHATEOASTests : IClassFixture<SqlServerFixture> {
        private readonly HttpClient _httpClient;
        private static BookDTO? _book;
        private static TokenDTO? _token;

        public BookControllerHATEOASTests(SqlServerFixture sqlFixture) {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions {
                    BaseAddress = new Uri("http://localhost")
                }
            );
        }

        private void AssertLinkPattern(string content, string rel) {
            var pattern = $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/book/v1.*?""";
            Regex.IsMatch(content, pattern).Should().BeTrue($"Link with rel='{rel}' should exist and have valid href");
        }

        [Fact(DisplayName = "00 - Sign In")]
        [TestPriority(0)]
        public async Task SignIn_ShouldReturnToken() {
            // Arrange
            var credentials = new UserDTO {
                Username = "leandro",
                Password = "admin123"
            };

            // Act
            var response = await _httpClient
                .PostAsJsonAsync("api/auth/sign-in", credentials);

            // Assert
            response.EnsureSuccessStatusCode();

            var token = await response.Content
                .ReadFromJsonAsync<TokenDTO>();

            token.Should().NotBeNull();

            token.AccessToken.Should().NotBeNullOrWhiteSpace();
            token.RefreshToken.Should().NotBeNullOrWhiteSpace();

            _token = token;
        }

        [Fact(DisplayName = "01 - Create Book")]
        [TestPriority(1)]
        public async Task CreateBook_ShouldContainHATEOASLinks() {

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            var request = new BookDTO {
                Title = "Harry Potter and The Sorcerer's Stone",
                Author = "J. K. Rowling",
                Price = 29.90m,
                LaunchDate = new DateTime(2025 - 04 - 25)
            };

            var response = await _httpClient.PostAsJsonAsync(
                "/api/book/v1", request);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            _book = await response.Content.ReadFromJsonAsync<BookDTO>();

            AssertLinkPattern(content, "collection");
            AssertLinkPattern(content, "self");
            AssertLinkPattern(content, "create");
            AssertLinkPattern(content, "update");
            AssertLinkPattern(content, "delete");
        }

        [Fact(DisplayName = "02 - Update Book")]
        [TestPriority(2)]

        public async Task UpdateBook_ShouldContainHATEOASLinks() {

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            _book!.Price = 48.80m;
            var response = await _httpClient.PutAsJsonAsync(
                "/api/book/v1", _book);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _book = await response.Content.ReadFromJsonAsync<BookDTO>();

            AssertLinkPattern(content, "collection");
            AssertLinkPattern(content, "self");
            AssertLinkPattern(content, "create");
            AssertLinkPattern(content, "update");
            AssertLinkPattern(content, "delete");
        }

        [Fact(DisplayName = "03 - Get Book By ID")]
        [TestPriority(3)]
        public async Task GetBookById_ShouldContainHATEOASLinks() {

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            var needsCreate = false;
            if (_book == null) {
                needsCreate = true;
            }
            else {
                var getResponse = await _httpClient.GetAsync($"api/person/v1/{_book!.Id}");
                if (getResponse.StatusCode == HttpStatusCode.NotFound) needsCreate = true;
            }
            if (needsCreate) {
                var request = new BookDTO {
                    Title = "Harry Potter and The Sorcerer's Stone",
                    Author = "J. K. Rowling",
                    Price = 29.90m,
                    LaunchDate = new DateTime(2025 - 04 - 25)
                };

                var createResponse = await _httpClient.PostAsJsonAsync(
                    "/api/book/v1", request);

                createResponse.EnsureSuccessStatusCode();
                var created = await createResponse.Content.ReadAsStringAsync();
                _book = await createResponse.Content.ReadFromJsonAsync<BookDTO>();
            }

            var response = await _httpClient.GetAsync($"/api/book/v1/{_book!.Id}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            _book = await response.Content.ReadFromJsonAsync<BookDTO>();

            AssertLinkPattern(content, "collection");
            AssertLinkPattern(content, "self");
            AssertLinkPattern(content, "create");
            AssertLinkPattern(content, "update");
            AssertLinkPattern(content, "delete");
        }

        [Fact(DisplayName = "04 - Delete Book")]
        [TestPriority(4)]
        public async Task DeleteBook_ShouldReturnNoContent() {

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            var needsCreate = false;
            if (_book == null) {
                needsCreate = true;
            }
            else {
                var getResponse = await _httpClient.GetAsync($"api/person/v1/{_book!.Id}");
                if (getResponse.StatusCode == HttpStatusCode.NotFound) needsCreate = true;
            }
            if (needsCreate) {
                var request = new BookDTO {
                    Title = "Harry Potter and The Sorcerer's Stone",
                    Author = "J. K. Rowling",
                    Price = 29.90m,
                    LaunchDate = new DateTime(2025 - 04 - 25)
                };

                var createResponse = await _httpClient.PostAsJsonAsync(
                    "/api/book/v1", request);

                createResponse.EnsureSuccessStatusCode();
                var created = await createResponse.Content.ReadAsStringAsync();
                _book = await createResponse.Content.ReadFromJsonAsync<BookDTO>();
            }
            var response = await _httpClient.DeleteAsync($"/api/book/v1/{_book!.Id}");

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "05 - Find All Books")]
        [TestPriority(5)]
        public async Task FindAll_ShouldReturnLinksForEachBook() {
            // ---------------------------
            // Arrange
            // ---------------------------
            // In this test, there is no explicit Arrange step, because
            // we are directly calling the API without preparing additional
            // data or mocking dependencies. The system under test is expected
            // to already contain one or more people.

            // ---------------------------
            // Act
            // ---------------------------
            // Perform the HTTP GET request to retrieve all people.

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            var response = await _httpClient.GetAsync("api/book/v1");
            response.EnsureSuccessStatusCode(); // Ensures the response status code is 2xx.

            // Read the response content as a string.
            var content = await response.Content.ReadAsStringAsync();

            // ---------------------------
            // Assert
            // ---------------------------
            // Extract all "id" values from the response JSON using Regex.
            var idMatches = Regex.Matches(content, @"""id"":\s*(\d+)");
            idMatches.Count.Should().BeGreaterThan(0, "There should be at least one book");

            // Iterate through each book id found in the response.
            foreach (Match match in idMatches) {
                var id = match.Groups[1].Value;

                // Expected hypermedia relations (HATEOAS links).
                var expectedRels = new[] { "collection", "self", "create", "update", "delete" };

                foreach (var rel in expectedRels) {
                    // Build the expected regex pattern depending on the relation.
                    // For "self" and "delete", the link must contain the specific id.
                    // For others, the link points to the base endpoint.
                    var pattern = rel switch {
                        "self" or "delete" =>
                            $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/book/v1/{id}""", // to 'self' and 'delete'
                        _ =>
                            $@"""rel"":\s*""{rel}"".*?""href"":\s*""https?://.+/api/book/v1""" // to others
                    };

                    // Assert that the link with the correct "rel" and "href" exists.
                    Regex.IsMatch(content, pattern, RegexOptions.IgnoreCase)
                         .Should().BeTrue($"Link '{rel}' should exist for book {id}");

                    // Assert that each link also contains a "type" attribute.
                    var typePattern = $@"""rel"":\s*""{rel}"".*?""type"":\s*""[^""]+""";
                    Regex.IsMatch(content, typePattern)
                         .Should().BeTrue($"Link '{rel}' must have a type for book {id}");
                }
            }
        }
    }
}