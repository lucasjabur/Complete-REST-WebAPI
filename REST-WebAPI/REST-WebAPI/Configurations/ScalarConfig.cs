using Scalar.AspNetCore;

namespace REST_WebAPI.Configurations {
    public static class ScalarConfig {
        private static readonly string AppName =
            "ASP.NET 2026 REST API's from 0 to Azure and GCP with .NET 10, Docker e Kubernetes";

        public static WebApplication UseScalarConfiguration(this WebApplication app) {
            app.MapScalarApiReference("/scalar", c => {
                c.WithTitle(AppName).WithOpenApiRoutePattern("/swagger/v1/swagger.json");
            });
            return app;
        }
    }
}
