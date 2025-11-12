using REST_WebAPI.Data.DTO.V1;

namespace REST_WebAPI.Services {
    public interface IFileServices {
        byte[] GetFile(string fileName);
        Task<FileDetailDTO> SaveFileToDisk(IFormFile file);
        Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files);
    }
}
