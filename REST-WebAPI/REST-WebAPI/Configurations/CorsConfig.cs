using Microsoft.Extensions.Configuration;

namespace REST_WebAPI.Configurations {
    public static class CorsConfig {
        private static string[] GetAllowedOrigins(
            IConfiguration configuration) {
            return configuration.GetSection("Cors:Origins")
                .Get<string[]>() ?? Array.Empty<string>();
        }

        public static void AddCorsConfiguration(
            this IServiceCollection services,
            IConfiguration configuration) {
            var origins = GetAllowedOrigins(configuration);

            services.AddCors(options => {
                options.AddPolicy("LocalPolicy",
                    policy => policy.WithOrigins(
                            "http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());

                options.AddPolicy("MultipleOriginPolicy",
                    policy => policy.WithOrigins(
                            "http://localhost:3000",
                            "http://localhost:8080",
                            "http://localhost:5063",
                            "https://localhost:7161",
                            "https://erudio.com.br"
                            )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());

                options.AddPolicy("DefaultPolicy",
                    policy => policy.WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });
        }

        public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app,
            IConfiguration configuration) {

            var origins = GetAllowedOrigins(configuration);

            app.Use(async (context, next) => {
                var origin = context.Request.Headers["Origin"].ToString();
                var aborted = context.RequestAborted;

                if (!string.IsNullOrEmpty(origin) &&
                    !origins.Contains(origin, StringComparer.OrdinalIgnoreCase)) {
                    if (!aborted.IsCancellationRequested && !context.Response.HasStarted) {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("CORS origin not allowed.", aborted);
                    }
                    return;
                }

                if (aborted.IsCancellationRequested) return;

                try {
                    await next();
                } catch (OperationCanceledException) when (aborted.IsCancellationRequested) {

                }
            });

            app.UseCors("DefaultPolicy");
            return app;
            // app.UseCors();
        }
    }
}
