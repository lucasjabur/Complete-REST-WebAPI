using REST_WebAPI.Data.DTO.V1;
using REST_WebAPI.Mail;

namespace REST_WebAPI.Services.Implementations {
    public class EmailServicesImpl : IEmailServices {

        private readonly EmailSender _emailSender;
        private readonly ILogger<EmailServicesImpl> _logger;

        public EmailServicesImpl(EmailSender emailSender, ILogger<EmailServicesImpl> logger) {
            _emailSender = emailSender;
            _logger = logger;
        }

        public async void SendSimpleEmail(string to, string subject, string body) {
            await _emailSender.To(to).WithSubject(subject).WithMessage(body).SendAsync();
        }

        public async Task SendEmailWithAttachment(EmailRequestDTO emailRequest, IFormFile attachment) {
            if (attachment == null || attachment.Length == 0) {
                _logger.LogWarning("Attachment is null or empty.");
                throw new ArgumentException("Attachment is null or empty", nameof(attachment));
            }

            string tempFilePath = Path.Combine(Path.GetTempPath(), attachment.FileName);

            try {
                await using (var stream = new FileStream(tempFilePath, FileMode.Create)) {
                    await attachment.CopyToAsync(stream);
                }
                await _emailSender.To(emailRequest.To)
                        .WithSubject(emailRequest.Subject)
                        .WithMessage(emailRequest.Body)
                        .Attach(tempFilePath)
                        .SendAsync();
                           
            } catch (Exception ex) {
                _logger.LogError(ex, "Error sending email with attachment.");
                throw;
            
            } finally {
                if (File.Exists(tempFilePath)) {
                    File.Delete(tempFilePath);
                }
            }
        }
    }
}
