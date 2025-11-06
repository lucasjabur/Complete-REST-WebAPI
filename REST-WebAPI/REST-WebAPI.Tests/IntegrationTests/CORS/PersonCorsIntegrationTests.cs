using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Json;

namespace REST_WebAPI.Tests.IntegrationTests.CORS {

    [TestCaseOrderer("REST_WebAPI.Tests.IntegrationTests.Tools.PriorityOrderer", "REST_WebAPI.Tests")]
    public class PersonCorsIntegrationTests : IClassFixture<SqlServerFixture> {
        private readonly HttpClient _httpClient;
        private static PersonDTO _person;

        public PersonCorsIntegrationTests(SqlServerFixture sqlFixture) {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions {
                    BaseAddress = new Uri("http://localhost")
                }
            );
        }

        private void AddOriginHeader(string origin) {
            _httpClient.DefaultRequestHeaders.Remove("Origin");
            _httpClient.DefaultRequestHeaders.Add("Origin", origin);
        }

        [TestPriority(1)]
        [Fact(DisplayName = "01 - Create Person With Allowed Origin")]
        public async Task CreatePerson_WithAllowedOrigin_ShouldReturnCreated() {
            AddOriginHeader("https://erudio.com.br");
            var request = new PersonDTO {
                FirstName = "Richard",
                LastName = "Stallman",
                Address = "New York City - New York - USA",
                Gender = "Male"
            };

            var response = await _httpClient.PostAsJsonAsync("/api/person/v1", request);

            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<PersonDTO>();
            created.Should().NotBeNull();
            created.Id.Should().BeGreaterThan(0);

            _person = created;
        }

        [TestPriority(2)]
        [Fact(DisplayName = "02 - Create Person With Disallowed Origin")]
        public async Task CreatePerson_WithDisallowedOrigin_ShouldReturnForbidden() {
            AddOriginHeader("https://semeru.com.br");
            var request = new PersonDTO {
                FirstName = "Richard",
                LastName = "Stallman",
                Address = "New York City - New York - USA",
                Gender = "Male"
            };

            var response = await _httpClient.PostAsJsonAsync("/api/person/v1", request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("CORS origin not allowed!");
        }

        [TestPriority(3)]
        [Fact(DisplayName = "03 - Get Person By Id With Allowed Origin")]
        public async Task FindPersonById_WithAllowedOrigin_ShouldReturnOk() {
            AddOriginHeader("https://erudio.com.br");

            var response = await _httpClient.GetAsync($"api/person/v1/{_person.Id}");

            response.EnsureSuccessStatusCode();

            var found = await response.Content.ReadFromJsonAsync<PersonDTO>();
            found.Should().NotBeNull();
            found.Id.Should().Be(_person.Id);
            found.FirstName.Should().Be("Richard");
            found.LastName.Should().Be("Stallman");
            found.Address.Should().Be("New York City - New York - USA");
        }

        [TestPriority(4)]
        [Fact(DisplayName = "04 - Get Person By Id With Disallowed Origin")]
        public async Task FindPersonById_WithDisallowedOrigin_ShouldReturnForbidden() {
            AddOriginHeader("https://semeru.com.br");

            var response = await _httpClient.GetAsync($"api/person/v1/{_person.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Be("CORS origin not allowed!");
        }
    }
}
