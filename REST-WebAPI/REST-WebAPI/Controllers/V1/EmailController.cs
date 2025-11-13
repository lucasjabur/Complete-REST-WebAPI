using Microsoft.AspNetCore.Mvc;
using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Services;
using System.Text.Json;

namespace REST_WebAPI.Controllers.V1 {
    
    [ApiController]
    [Route("api/[controller]/v1")]
    public class EmailController : ControllerBase {
        
        private readonly IEmailServices _emailServices;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailServices emailServices, ILogger<EmailController> logger) {
            _emailServices = emailServices;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult SendEmail([FromBody] EmailRequestDTO emailRequest) {
            try {
                _logger.LogInformation("Sending email to '{to}' with subject '{subject}'", emailRequest.To, emailRequest.Subject);
                _emailServices.SendSimpleEmail(emailRequest.To, emailRequest.Subject, emailRequest.Body);
                return Ok("Email sent successfully.");
            } catch (Exception ex) {
                _logger.LogError(ex, "Error occurred while sending email to {to}", emailRequest.To);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while sending the email.");
            }
        }

        [HttpPost("with-attachment")]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200, Type = typeof(string))]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SendEmailWithAttachment([FromForm] string emailRequest, [FromForm] FileUploadDTO attachment) {
            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            EmailRequestDTO emailRequestDto = JsonSerializer.Deserialize<EmailRequestDTO>(emailRequest, options);

            if (emailRequestDto == null) {
                _logger.LogWarning("Invalid email request data provided.");
                return BadRequest("Invalid email request data provided.");
            }

            if (attachment?.File == null || attachment.File.Length == 0) {
                _logger.LogWarning("No attachment provided for email to {to}", emailRequestDto.To);
                return BadRequest("Attachment is null or empty");
            }
            _logger.LogInformation("Sending email with attachment to '{to}' with subject '{subject}'", emailRequestDto.To, emailRequestDto.Subject);
            await _emailServices.SendEmailWithAttachment(emailRequestDto, attachment.File);

            return Ok("Email with attachment sent successfully.");
        }
    }
}
