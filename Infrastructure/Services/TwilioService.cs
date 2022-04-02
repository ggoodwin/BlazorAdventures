using Application.Configurations;
using Application.Interfaces.Services;
using Application.Requests.SMS;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Infrastructure.Services
{
    public class TwilioService : ISmsService
    {
        private readonly SmsConfiguration _config;
        private readonly ILogger<TwilioService> _logger;

        public TwilioService(IOptions<SmsConfiguration> config, ILogger<TwilioService> logger)
        {
            _config = config.Value;
            _logger = logger;
        }

        public async Task<string> SendAsync(SmsRequest request)
        {
            TwilioClient.Init(_config.AccountId, _config.AuthToken);

            var message = await MessageResource.CreateAsync(
                new PhoneNumber(request.PhoneNumber),
                @from: new PhoneNumber(_config.SendNumber),
                body: request.Message
            );
            return message.Sid;
        }
    }
}
