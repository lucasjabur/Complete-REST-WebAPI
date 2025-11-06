using EvolveDb;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using Serilog;

namespace REST_WebAPI.Configurations {
    public static class EvolveConfig {
        public static IServiceCollection AddEvolveConfiguration(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment()) {
                var connectionString = configuration["MSSQLServerSQLConnection:MSSQLServerSQLConnectionString"];

                if (string.IsNullOrEmpty(connectionString)) {
                    throw new ArgumentNullException("Connection string 'MSSQLServerSQLConnectionString' not found!");
                }
                
                try {
                    ExecuteMigrations(connectionString);
                } catch (Exception ex) {
                    Log.Error(ex, "An error ocurred while migrating the database.");
                }
            }
            return services;
        }

        public static void ExecuteMigrations(string connectionString) {
            using var evolveConnection = new SqlConnection(connectionString);
            var evolve = new Evolve(evolveConnection, msg => Log.Information(msg)) {
                Locations = new List<string> { "db/migrations", "db/dataset" },
                IsEraseDisabled = true
            };
            evolve.Migrate();
        }
    }
}
