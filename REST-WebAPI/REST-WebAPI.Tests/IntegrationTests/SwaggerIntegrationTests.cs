using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using REST_WebAPI.Tests.IntegrationTests.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace REST_WebAPI.Tests.IntegrationTests {
    public class SwaggerIntegrationTests : IClassFixture<SqlServerFixture> {
        private readonly HttpClient _httpClient;

        public SwaggerIntegrationTests(SqlServerFixture sqlFixture) {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlFixture.ConnectionString);

            _httpClient = factory.CreateClient(
                new WebApplicationFactoryClientOptions {
                    BaseAddress = new Uri("http://localhost")
                }
            );
        }

        [Fact]
        public async Task Swagger_Json_ShouldReturnSwaggerJson() {
            var response = await _httpClient.GetAsync("/swagger/v1/swagger.json");
            
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNull();
            content.Should().Contain("/api/person/v1");
        }

        [Fact]
        public async Task SwaggerUI_ShouldReturnSwaggerUI() {
            var response = await _httpClient.GetAsync("/swagger-ui/index.html");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNull();
            content.Should().Contain("<div id=\"swagger-ui\">");
        }
    }
}
