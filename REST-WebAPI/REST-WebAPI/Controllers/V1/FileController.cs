using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Services;

namespace REST_WebAPI.Controllers.V1 {

    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v1")]
    public class FileController : ControllerBase {

        private IFileServices _fileServices;
        private readonly ILogger<FileController> _logger;

        public FileController(IFileServices fileServices, ILogger<FileController> logger) {
            _fileServices = fileServices;
            _logger = logger;
        }

        [HttpGet("download-file/{fileName}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(204)]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [Produces("application/octet-stream")]
        public IActionResult DownloadFile([FromRoute] string fileName) {
            var buffer = _fileServices.GetFile(fileName);
            if (buffer == null || buffer.Length == 0) {
                return NoContent();
            }
            var contentType = $"application/{Path.GetExtension(fileName).TrimStart('.')}";
            return File(buffer, contentType, fileName);
        }

        [HttpPost("upload-file")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(FileDetailDTO))]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadDTO input) {
            var fileDetail = await _fileServices.SaveFileToDisk(input.File);
            _logger.LogInformation($"File '{input.File.FileName}' uploaded successfully.");
            return Ok(fileDetail);
        }

        [HttpPost("upload-multiple-files")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(200, Type = typeof(List<FileDetailDTO>))]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> UploadMultipleFiles([FromForm] MultipleFilesUploadDTO input) {
            var filesDetail = await _fileServices.SaveFilesToDisk(input.Files);
            return Ok(filesDetail);
        }
    }
}
