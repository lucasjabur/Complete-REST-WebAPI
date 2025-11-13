using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore.Metadata;
using REST_WebAPI.Files.Importers.Contract;
using REST_WebAPI.Files.Importers.Implementations;

namespace REST_WebAPI.Files.Importers.Factory {
    public class FileImporterFactory {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FileImporterFactory> _logger;

        public FileImporterFactory(IServiceProvider serviceProvider, ILogger<FileImporterFactory> logger) {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IFileImporter GetImporter(string fileName) {
            if (fileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)) {
                _logger.LogInformation($"Selected CSV file importer for file: '{fileName}'");
                 return _serviceProvider.GetRequiredService<CsvImporter>();

            } else if (fileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase)) {
                _logger.LogInformation($"Selected Excel file importer for file: '{fileName}'");
                return _serviceProvider.GetRequiredService<XlsxImporter>();

            } else {
                _logger.LogError("Unsupported file format: {FileName}", fileName);
                throw new NotSupportedException($"Unsupported file format: {fileName}");
            }
        }
    }
}
