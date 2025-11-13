using REST_WebAPI.Data.DTO.V1;

namespace REST_WebAPI.Services {
    public interface IEmailServices {
        void SendSimpleEmail(string to, string subject, string body);
        Task SendEmailWithAttachment(EmailRequestDTO emailRequest, IFormFile attachment);

    }
}
