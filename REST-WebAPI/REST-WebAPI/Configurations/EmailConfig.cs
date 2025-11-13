using REST_WebAPI.Mail.Settings;

namespace REST_WebAPI.Configurations {
    public static class EmailConfig {
        public static IServiceCollection AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration) {
            var section = configuration.GetSection("Email");
            var configs = section.Get<EmailSettings>();

            if (configs == null) {
                throw new InvalidOperationException("Email settings are not configured properly.");
            }

            // configs.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME") ?? configs.Username;
            // configs.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD") ?? configs.Password;

            configs.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME", EnvironmentVariableTarget.User)
                    ?? Environment.GetEnvironmentVariable("EMAIL_USERNAME", EnvironmentVariableTarget.Machine)
                    ?? Environment.GetEnvironmentVariable("EMAIL_USERNAME", EnvironmentVariableTarget.Process)
                    ?? configs.Username;

            configs.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD", EnvironmentVariableTarget.User)
                            ?? Environment.GetEnvironmentVariable("EMAIL_PASSWORD", EnvironmentVariableTarget.Machine)
                            ?? Environment.GetEnvironmentVariable("EMAIL_PASSWORD", EnvironmentVariableTarget.Process)
                            ?? configs.Password;

            configs.Password = configs.Password?.Replace(" ", "").Trim();

            Console.WriteLine($"EMAIL_USERNAME loaded: {!string.IsNullOrEmpty(configs.Username)}");
            Console.WriteLine($"EMAIL_PASSWORD loaded: {!string.IsNullOrEmpty(configs.Password)}");
            Console.WriteLine($"EMAIL_PASSWORD length: {configs.Password?.Length ?? 0}");

            services.AddSingleton(configs);
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            return services;
        }
    }
}
