using REST_WebAPI.Files.Exporters.Contract;
using REST_WebAPI.Files.Exporters.Implementations;
using REST_WebAPI.Files.Importers.Factory;

namespace REST_WebAPI.Files.Exporters.Factory {
    public class FileExporterFactory {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FileExporterFactory> _logger;

        public FileExporterFactory(IServiceProvider serviceProvider, ILogger<FileExporterFactory> logger) {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IFileExporter GetExporter(string acceptHeader) {
            if (string.Equals(acceptHeader, MediaTypes.ApplicationXlsx, StringComparison.OrdinalIgnoreCase)) {
                _logger.LogInformation($"Selected Excel file exporter for media type: '{acceptHeader}'");
                return _serviceProvider.GetRequiredService<XlsxExporter>();
            }
            else if (string.Equals(acceptHeader, MediaTypes.ApplicationCsv, StringComparison.OrdinalIgnoreCase)) {

                _logger.LogInformation($"Selected CSV file exporter for media type: '{acceptHeader}'");
                return _serviceProvider.GetRequiredService<CsvExporter>();
            }
            else {
                _logger.LogError($"Unsupported media type {acceptHeader}");
                return _serviceProvider.GetRequiredService<CsvExporter>();
            }
        }
    }
}
