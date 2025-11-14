using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Hypermedia.Utils;
using REST_WebAPI.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace REST_WebAPI.Tests.IntegrationTests.Person.JSON {
    [TestCaseOrderer(
        TestConfig.TestCaseOrdererFullName, TestConfig.TestCaseOrdererAssembly)]
    public class PersonControllerJsonTests : IClassFixture<SqlServerFixture> {
        private readonly HttpClient _httpClient;
        private static PersonDTO? _person;
        private static TokenDTO? _token;

        public PersonControllerJsonTests(SqlServerFixture sqlFixture) {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions {
                    BaseAddress = new Uri("http://localhost")
                }
            );
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

        [Fact(DisplayName = "01 - Create Person")]
        [TestPriority(1)]
        public async Task CreatePerson_ShouldReturnCreatedPerson() {
            // Arrange

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            var request = new PersonDTO {
                FirstName = "Linus",
                LastName = "Torvalds",
                Address = "Helsinki - Finland",
                Gender = "Male",
                Enabled = true
            };

            // Act
            var response = await _httpClient
                .PostAsJsonAsync("api/person/v1", request);

            // Assert
            response.EnsureSuccessStatusCode();

            var created = await response.Content
                .ReadFromJsonAsync<PersonDTO>();

            created.Should().NotBeNull();
            created.Id.Should().BeGreaterThan(0);
            created.FirstName.Should().Be("Linus");
            created.LastName.Should().Be("Torvalds");
            created.Address.Should().Be("Helsinki - Finland");
            created.Enabled.Should().BeTrue();

            _person = created;
        }

        [Fact(DisplayName = "02 - Update Person")]
        [TestPriority(2)]
        public async Task UpdatePerson_ShouldReturnUpdatedPerson() {

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            var needsCreate = false;
            if (_person == null) {
                needsCreate = true;
            }
            else {
                var getResponse = await _httpClient.GetAsync($"api/person/v1/{_person.Id}");
                if (getResponse.StatusCode == HttpStatusCode.NotFound) needsCreate = true;
            }

            if (needsCreate) {
                var request = new PersonDTO {
                    FirstName = "Linus",
                    LastName = "Torvalds",
                    Address = "Helsinki - Finland",
                    Gender = "Male",
                    Enabled = true
                };

                var createResponse = await _httpClient.PostAsJsonAsync("api/person/v1", request);
                createResponse.EnsureSuccessStatusCode();
                var created = await createResponse.Content.ReadFromJsonAsync<PersonDTO>();
                created.Should().NotBeNull();
                _person = created;
            }

            // Arrange
            _person.LastName = "Benedict Torvalds";

            // Act
            var response = await _httpClient
                .PutAsJsonAsync("api/person/v1", _person);

            // Assert
            response.EnsureSuccessStatusCode();

            var updated = await response.Content
                .ReadFromJsonAsync<PersonDTO>();

            updated.Should().NotBeNull();
            updated.Id.Should().BeGreaterThan(0);
            updated.FirstName.Should().Be("Linus");
            updated.LastName.Should().Be("Benedict Torvalds");
            updated.Address.Should().Be("Helsinki - Finland");
            updated.Enabled.Should().BeTrue();

            _person = updated;
        }

        [Fact(DisplayName = "03 - Disable Person By ID")]
        [TestPriority(3)]
        public async Task DisablePersonById_ShouldReturnDisabledPerson() {

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            if (_person == null) {
                var request = new PersonDTO {
                    FirstName = "Linus",
                    LastName = "Benedict Torvalds",
                    Address = "Helsinki - Finland",
                    Gender = "Male",
                    Enabled = true
                };

                var createResponse = await _httpClient.PostAsJsonAsync("api/person/v1", request);
                createResponse.EnsureSuccessStatusCode();
                var created = await createResponse.Content.ReadFromJsonAsync<PersonDTO>();
                created.Should().NotBeNull();
                _person = created;
            }

            // Arrange & Act
            var response = await _httpClient
                .PatchAsync($"api/person/v1/{_person.Id}", null);

            // Assert
            response.EnsureSuccessStatusCode();

            var disabled = await response.Content
                .ReadFromJsonAsync<PersonDTO>();

            disabled.Should().NotBeNull();
            disabled.Id.Should().BeGreaterThan(0);
            disabled.FirstName.Should().Be("Linus");
            disabled.LastName.Should().Be("Benedict Torvalds");
            disabled.Address.Should().Be("Helsinki - Finland");
            disabled.Enabled.Should().BeFalse();

            _person = disabled;
        }

        [Fact(DisplayName = "04 - Get Person By ID")]
        [TestPriority(4)]
        public async Task GetPersonById_ShouldReturnPerson() {

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            var needsCreate = false;
            if (_person == null) {
                needsCreate = true;
            }
            else {
                var getResponse = await _httpClient.GetAsync($"api/person/v1/{_person.Id}");
                if (getResponse.StatusCode == HttpStatusCode.NotFound) needsCreate = true;
            }

            if (needsCreate) {
                var request = new PersonDTO {
                    FirstName = "Linus",
                    LastName = "Benedict Torvalds",
                    Address = "Helsinki - Finland",
                    Gender = "Male",
                    Enabled = false
                };

                var createResponse = await _httpClient.PostAsJsonAsync("api/person/v1", request);
                createResponse.EnsureSuccessStatusCode();
                var created = await createResponse.Content.ReadFromJsonAsync<PersonDTO>();
                created.Should().NotBeNull();
                _person = created;
            }

            // Arrange & Act
            var response = await _httpClient
                .GetAsync($"api/person/v1/{_person.Id}");

            // Assert
            response.EnsureSuccessStatusCode();

            var found = await response.Content
                .ReadFromJsonAsync<PersonDTO>();

            found.Should().NotBeNull();
            found.Id.Should().Be(_person.Id);
            found.FirstName.Should().Be("Linus");
            found.LastName.Should().Be("Benedict Torvalds");
            found.Address.Should().Be("Helsinki - Finland");
            found.Enabled.Should().BeFalse();
        }

        [Fact(DisplayName = "05 - Delete Person By ID")]
        [TestPriority(5)]
        public async Task DeletePersonById_ShouldReturnNoContent() {

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            if (_person == null) {
                var request = new PersonDTO {
                    FirstName = "Linus",
                    LastName = "Benedict Torvalds",
                    Address = "Helsinki - Finland",
                    Gender = "Male",
                    Enabled = true
                };

                var createResponse = await _httpClient.PostAsJsonAsync("api/person/v1", request);
                createResponse.EnsureSuccessStatusCode();
                var created = await createResponse.Content.ReadFromJsonAsync<PersonDTO>();
                created.Should().NotBeNull();
                _person = created;
            }

            // Arrange & Act
            var response = await _httpClient
                .DeleteAsync($"api/person/v1/{_person.Id}");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact(DisplayName = "06 - Find all Person")]
        [TestPriority(6)]
        public async Task FindAllPerson_ShouldReturnListOfPerson() {

            _httpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", _token?.AccessToken);

            // Arrange & Act
            var response = await _httpClient
                .GetAsync("api/person/v1/asc/10/1");

            // Assert
            response.EnsureSuccessStatusCode();

            var page = await response.Content
                .ReadFromJsonAsync<PagedSearchDTO<PersonDTO>>();

            page.Should().NotBeNull();
            page.CurrentPage.Should().Be(1);

            var list = page?.List;

            list.Should().NotBeNull();
            list.Count.Should().BeGreaterThan(0);

            var first = list.First(p => p.FirstName == "Ab");
            first.LastName.Should().Be("Cartmael");
            first.Address.Should().Be("7th Floor");
            first.Enabled.Should().BeFalse();
            first.Gender.Should().Be("Male");

            var third = list.First(p => p.FirstName == "Abbye");
            third.LastName.Should().Be("Kensit");
            third.Address.Should().Be("PO Box 81864");
            third.Enabled.Should().BeFalse();
            third.Gender.Should().Be("Female");

            page.CurrentPage.Should().BeGreaterThan(0);
            page.TotalResults.Should().BeGreaterThan(0);
            page.PageSize.Should().BeGreaterThan(0);
            page.SortDirections.Should().NotBeNull();

            page.CurrentPage.Should().BeGreaterThan(0);
            page.TotalResults.Should().BeGreaterThan(0);
            page.PageSize.Should().BeGreaterThan(0);
            page.SortDirections.Should().NotBeNull();
        }
    }
}