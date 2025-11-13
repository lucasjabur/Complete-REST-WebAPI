using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Files.Exporters.Contract;
using REST_WebAPI.Files.Importers.Factory;
using System.Globalization;
using System.Text;

namespace REST_WebAPI.Files.Exporters.Implementations {
    public class CsvExporter : IFileExporter {
        public FileContentResult ExportFile(List<PersonDTO> people) {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen:true);

            using var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = true,
            });

            csv.WriteRecords(people);
            writer.Flush();

            var fileBytes = memoryStream.ToArray();

            return new FileContentResult(fileBytes, MediaTypes.ApplicationCsv) {
                FileDownloadName = $"people_exported_{DateTime.UtcNow:yyyyMMddHHmmss}.csv"
            };
        }
    }
}