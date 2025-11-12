using System.ComponentModel.DataAnnotations;

namespace REST_WebAPI.Data.DTO.V1 {
    public class MultipleFilesUploadDTO {

        [Required]
        public List<IFormFile> Files { get; set; }
    }
}
