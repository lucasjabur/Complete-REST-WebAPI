using Microsoft.VisualBasic;
using REST_WebAPI.Data.DTO.V1;

namespace REST_WebAPI.Services.Implementations {
    public class FileServicesImpl : IFileServices {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;

        private static readonly HashSet<string> _allowedExtensions = new() {
            ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx", ".mp3", ".txt"
        };

        public FileServicesImpl(IHttpContextAccessor context) {
            _context = context;
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadDir");
            if (!Directory.Exists(_basePath)) {
                Directory.CreateDirectory(_basePath);
            }
        }

        public byte[] GetFile(string fileName) {
            var filePath = Path.Combine(_basePath, fileName);
            if (!File.Exists(filePath)) {
                return null;
            }
            return File.ReadAllBytes(filePath);
        }

        public async Task<FileDetailDTO> SaveFileToDisk(IFormFile file) {
            if (file == null || file.Length == 0) {
                throw new Exception("File is null or empty");
            }

            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(fileExtension)) {
                throw new Exception("File type is not allowed");
            }

            var docName = Path.GetFileName(file.FileName);
            var destination = Path.Combine(_basePath, docName);

            var baseUrl = $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host}";
            var fileDetail = new FileDetailDTO {
                DocumentName = docName,
                DocType = file.ContentType,
                DocUrl = $"{baseUrl}/api/file/v1/download-file/{docName}"
            };

            using var stream = new FileStream(destination, FileMode.Create);
            await file.CopyToAsync(stream);
            return fileDetail;
        }

        public async Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files) {
            var results = new List<FileDetailDTO>();
            foreach (var file in files) {
                var fileDetail = await SaveFileToDisk(file);
                if (!string.IsNullOrEmpty(fileDetail.DocumentName)) {
                    results.Add(fileDetail);
                }
            }
            return results;
        }
    }
}
