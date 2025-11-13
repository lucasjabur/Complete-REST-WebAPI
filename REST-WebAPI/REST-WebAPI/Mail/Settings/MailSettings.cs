using DocumentFormat.OpenXml.Bibliography;

namespace REST_WebAPI.Mail.Settings {
    public class MailSettings {
        public bool SmtpAuth { get; set; }
        public bool StartTlsEnable { get; set; }
        public bool StartTlsRequired { get; set; }
    }
}