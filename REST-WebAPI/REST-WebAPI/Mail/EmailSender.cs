using MimeKit;
using REST_WebAPI.Mail.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace REST_WebAPI.Mail {
    public class EmailSender {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailSender> _logger;
        private readonly List<MailboxAddress> _recipients = new();
        private string _to;
        private string _subject;
        private string _body;
        private string? _attachment;

        public EmailSender(EmailSettings settings, ILogger<EmailSender> logger) {
            _settings = settings;
            _logger = logger;
        }

        public EmailSender To(string to) {
            _to = to;
            _recipients.Clear();
            _recipients.AddRange(ParseRecipients(to));
            return this;
        }

        public EmailSender WithSubject(string subject) {
            _subject = subject;
            return this;
        }

        public EmailSender WithMessage(string body) {
            _body = body;
            return this;
        }

        public EmailSender Attach(string attachmentPath) {
            if (File.Exists(attachmentPath)) {
                _attachment = attachmentPath;
                return this;
            
            } else {
                _logger.LogWarning("Attachment file not found: {AttachmentPath}", attachmentPath);
            }
            return this;
        }

        //public async void Send() {
        //    var message = new MimeMessage();

        //    message.From.Add(new MailboxAddress(_settings.From, _settings.Username));
        //    message.To.AddRange(_recipients);
        //    message.Subject = _subject ?? _settings.Subject ?? "No Subject";

        //    var builder = new BodyBuilder {
        //        TextBody = _body ?? _settings.Message ?? ""
        //    };

        //    if (!string.IsNullOrEmpty(_attachment)) {
        //        var filename = Path.GetFileName(_attachment);
        //        builder.Attachments.Add(filename, File.ReadAllBytes(_attachment));
        //    }

        //    message.Body = builder.ToMessageBody();

        //    try {
        //        using var client = new SmtpClient();

        //        _logger.LogInformation($"Connecting to {_settings.Host}:{_settings.Port}");
        //        _logger.LogInformation($"Username: {_settings.Username}");
        //        _logger.LogInformation($"Password length: {_settings.Password?.Length ?? 0}");
        //        _logger.LogInformation($"SmtpAuth enabled: {_settings.Properties?.SmtpAuth ?? false}");

        //        await client.ConnectAsync(
        //            _settings.Host,
        //            _settings.Port,
        //            SecureSocketOptions.StartTls
        //        );

        //        _logger.LogInformation("Connected successfully. Authenticating...");

        //        await client.AuthenticateAsync(_settings.Username, _settings.Password);
        //        await client.SendAsync(message);
        //        await client.DisconnectAsync(true);

        //        _logger.LogInformation("Email successfully sent to {Recipients}", string.Join(";", _recipients));
        //    }
        //    catch (Exception ex) {
        //        _logger.LogError(ex, "Failed to send email to {Recipients}", string.Join(";", _recipients));
        //        throw;
        //    }
        //    finally {
        //        Reset();
        //    }
        //}

        public async Task SendAsync() {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.From, _settings.Username));
            message.To.AddRange(_recipients);
            message.Subject = _subject ?? _settings.Subject ?? "No Subject";

            var builder = new BodyBuilder {
                TextBody = _body ?? _settings.Message ?? ""
            };

            if (!string.IsNullOrEmpty(_attachment)) {
                var filename = Path.GetFileName(_attachment);
                builder.Attachments.Add(filename, File.ReadAllBytes(_attachment));
            }

            message.Body = builder.ToMessageBody();

            try {
                using var client = new SmtpClient();

                // ✅ Configuração fixa para Gmail
                _logger.LogInformation("Sending email to '{To}' with subject '{Subject}'", _to, _subject);

                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.Username, _settings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email successfully sent to {Recipients}", string.Join(";", _recipients));
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Failed to send email to {Recipients}", string.Join(";", _recipients));
                throw;
            }
            finally {
                Reset();
            }
        }

        private IEnumerable<MailboxAddress> ParseRecipients(string to) {
            var tosWithoutSpace = to.Replace(" ", string.Empty);
            var recipients = tosWithoutSpace.Split(';', StringSplitOptions.RemoveEmptyEntries);

            var list = new List<MailboxAddress>();
            foreach (var address in recipients) {
                try {
                    var mailbox = MailboxAddress.Parse(address);
                    list.Add(mailbox);
                }
                catch (Exception ex) {
                    _logger.LogWarning(ex, "Invalid email address: {Recipient}", address);
                }
            }
            return list;
        }
        private void Reset() {
            _to = null;
            _subject = null;
            _body = null;
            _attachment = null;
            _recipients.Clear();
        }
    }
}
