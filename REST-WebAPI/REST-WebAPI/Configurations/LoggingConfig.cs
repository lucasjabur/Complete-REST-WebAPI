using Serilog;

namespace REST_WebAPI.Configurations {
    public static class LoggingConfig {
        public static void AddSerilogLogging(this WebApplicationBuilder builder) {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog();
        }
    }
}
