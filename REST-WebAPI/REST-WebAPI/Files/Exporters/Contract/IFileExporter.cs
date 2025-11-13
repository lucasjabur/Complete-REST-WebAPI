using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Data.DTO.V1;

namespace REST_WebAPI.Files.Exporters.Contract {
    public interface IFileExporter {
        FileContentResult ExportFile(List<PersonDTO> people);
    }
}
