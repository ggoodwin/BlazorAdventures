using Application.Configurations;
using Application.Interfaces.Services;
using Application.Requests.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class SendGridService : IMailService
    {
        private readonly SendGridConfiguration _config;
        private readonly ILogger<SendGridService> _logger;

        public SendGridService(IOptions<SendGridConfiguration> config, ILogger<SendGridService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public async Task SendAsync(MailRequest request)
        {
            try
            {
                var client = new SendGridClient(_config.Key);
                var from = new EmailAddress(_config.From, _config.DisplayName);
                var subject = request.Subject;
                var to = new EmailAddress(request.To, request.ToName ?? request.To);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, request.Body, request.HtmlBody);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}
