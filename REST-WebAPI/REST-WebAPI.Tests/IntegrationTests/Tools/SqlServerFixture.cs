using REST_WebAPI.Configurations;
using Testcontainers.MsSql;

namespace REST_WebAPI.Tests.IntegrationTests.Tools {
    public class SqlServerFixture : IAsyncLifetime {

        public MsSqlContainer Container { get; }
        public string ConnectionString => Container.GetConnectionString();

        public SqlServerFixture() {
            Container = new MsSqlBuilder().WithPassword("Lu19c@sja2002bur").Build();
        }

        public async Task InitializeAsync() {
            await Container.StartAsync();
            EvolveConfig.ExecuteMigrations(ConnectionString);
        }

        public async Task DisposeAsync() {
            await Container.DisposeAsync();
        }

    }
}
