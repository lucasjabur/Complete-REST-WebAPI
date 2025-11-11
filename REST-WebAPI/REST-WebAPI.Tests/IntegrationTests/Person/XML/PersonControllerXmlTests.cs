using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Tests.IntegrationTests.Tools;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace REST_WebAPI.Tests.IntegrationTests.Person.XML {
    [TestCaseOrderer(
        "REST_WebAPI.Tests.IntegrationTests.Tools.PriorityOrderer",
        "REST-WebAPI.Tests")]
    public class PersonControllerXmlTests : IClassFixture<SqlServerFixture> {
        private readonly HttpClient _httpClient;
        private static PersonDTO? _person;

        public PersonControllerXmlTests(SqlServerFixture sqlFixture) {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions {
                    BaseAddress = new Uri("http://localhost")
                }
            );

            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/xml"));
        }

        [Fact(DisplayName = "01 - Create Person")]
        [TestPriority(1)]
        public async Task CreatePerson_ShouldReturnCreatedPerson() {
            // Arrange
            var request = new PersonDTO {
                FirstName = "Galileo",
                LastName = "Galilei",
                Address = "Pisa - Italy",
                Gender = "Male",
                Enabled = true
            };

            // Act
            var response = await _httpClient
                .PostAsync("api/person/v1", XmlHelper.SerializeToXml(request));

            // Assert
            response.EnsureSuccessStatusCode();

            var created = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            created.Should().NotBeNull();
            created.Id.Should().BeGreaterThan(0);
            created.FirstName.Should().Be("Galileo");
            created.LastName.Should().Be("Galilei");
            created.Address.Should().Be("Pisa - Italy");
            created.Enabled.Should().BeTrue();

            _person = created;
        }

        [Fact(DisplayName = "02 - Update Person")]
        [TestPriority(2)]
        public async Task UpdatePerson_ShouldReturnUpdatedPerson() {
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
                    FirstName = "Galileo",
                    LastName = "Galilei",
                    Address = "Pisa - Italy",
                    Gender = "Male",
                    Enabled = true
                };

                var createResponse = await _httpClient.PostAsync("api/person/v1", XmlHelper.SerializeToXml(request));
                createResponse.EnsureSuccessStatusCode();
                var created = await XmlHelper.ReadFromXmlAsync<PersonDTO>(createResponse);
                created.Should().NotBeNull();
                _person = created;
            }
            // Arrange
            _person.LastName = "di Vincenzo Bonaiuti de' Galilei";

            // Act
            var response = await _httpClient
                .PutAsync("api/person/v1", XmlHelper.SerializeToXml(_person));

            // Assert
            response.EnsureSuccessStatusCode();

            var updated = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            updated.Should().NotBeNull();
            updated.Id.Should().BeGreaterThan(0);
            updated.FirstName.Should().Be("Galileo");
            updated.LastName.Should().Be("di Vincenzo Bonaiuti de' Galilei");
            updated.Address.Should().Be("Pisa - Italy");
            updated.Enabled.Should().BeTrue();

            _person = updated;
        }

        [Fact(DisplayName = "03 - Disable Person By ID")]
        [TestPriority(3)]
        public async Task DisablePersonById_ShouldReturnDisabledPerson() {
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
                    FirstName = "Galileo",
                    LastName = "di Vincenzo Bonaiuti de' Galilei",
                    Address = "Pisa - Italy",
                    Gender = "Male",
                    Enabled = true
                };

                var createResponse = await _httpClient.PostAsync("api/person/v1", XmlHelper.SerializeToXml(request));
                createResponse.EnsureSuccessStatusCode();
                var created = await XmlHelper.ReadFromXmlAsync<PersonDTO>(createResponse);
                created.Should().NotBeNull();
                _person = created;
            }

            // Arrange & Act
            var response = await _httpClient
                .PatchAsync($"api/person/v1/{_person.Id}", null);

            // Assert
            response.EnsureSuccessStatusCode();

            var disabled = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            disabled.Should().NotBeNull();
            disabled.Id.Should().BeGreaterThan(0);
            disabled.FirstName.Should().Be("Galileo");
            disabled.LastName.Should().Be("di Vincenzo Bonaiuti de' Galilei");
            disabled.Address.Should().Be("Pisa - Italy");
            disabled.Enabled.Should().BeFalse();

            _person = disabled;
        }

        [Fact(DisplayName = "04 - Get Person By ID")]
        [TestPriority(4)]
        public async Task GetPersonById_ShouldReturnPerson() {
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
                    FirstName = "Galileo",
                    LastName = "di Vincenzo Bonaiuti de' Galilei",
                    Address = "Pisa - Italy",
                    Gender = "Male",
                    Enabled = false
                };

                var createResponse = await _httpClient.PostAsync("api/person/v1", XmlHelper.SerializeToXml(request));
                createResponse.EnsureSuccessStatusCode();
                var created = await XmlHelper.ReadFromXmlAsync<PersonDTO>(createResponse);
                created.Should().NotBeNull();
                _person = created;
            }

            // Arrange & Act
            var response = await _httpClient
                .GetAsync($"api/person/v1/{_person.Id}");

            // Assert
            response.EnsureSuccessStatusCode();

            var found = await XmlHelper.ReadFromXmlAsync<PersonDTO>(response);

            found.Should().NotBeNull();
            found.Id.Should().Be(_person.Id);
            found.FirstName.Should().Be("Galileo");
            found.LastName.Should().Be("di Vincenzo Bonaiuti de' Galilei");
            found.Address.Should().Be("Pisa - Italy");
            found.Enabled.Should().BeFalse();
        }

        [Fact(DisplayName = "05 - Delete Person By ID")]
        [TestPriority(5)]
        public async Task DeletePersonById_ShouldReturnNoContent() {
            if (_person == null) {
                var request = new PersonDTO {
                    FirstName = "Galileo",
                    LastName = "Galilei",
                    Address = "Pisa - Italy",
                    Gender = "Male",
                    Enabled = true
                };

                var createResponse = await _httpClient.PostAsync("api/person/v1", XmlHelper.SerializeToXml(request));
                createResponse.EnsureSuccessStatusCode();
                var created = await XmlHelper.ReadFromXmlAsync<PersonDTO>(createResponse);
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
            // Arrange & Act
            var response = await _httpClient
                .GetAsync("api/person/v1");

            // Assert
            response.EnsureSuccessStatusCode();

            var list = await XmlHelper.ReadFromXmlAsync<List<PersonDTO>>(response);

            list.Should().NotBeNull();
            list.Count.Should().BeGreaterThan(0);

            var first = list.First(p => p.FirstName == "Ayrton");
            first.LastName.Should().Be("Senna");
            first.Address.Should().Be("São Paulo - Brasil");
            first.Enabled.Should().BeTrue();
            first.Gender.Should().Be("Male");

            var sixth = list.First(p => p.FirstName == "Ada");
            sixth.LastName.Should().Be("Lovelace");
            sixth.Address.Should().Be("London - England");
            sixth.Enabled.Should().BeTrue();
            sixth.Gender.Should().Be("Female");
        }
    }
}
