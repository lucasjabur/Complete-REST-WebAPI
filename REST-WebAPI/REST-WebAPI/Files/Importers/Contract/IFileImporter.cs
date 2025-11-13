using REST_WebAPI.Data.DTO.V1;

namespace REST_WebAPI.Files.Importers.Contract {
    public interface IFileImporter {
        Task<List<PersonDTO>> ImportFileAsync(Stream fileStream);  
    }
}
