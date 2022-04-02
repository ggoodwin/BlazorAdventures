using Application.Requests.SMS;

namespace Application.Interfaces.Services
{
    public interface ISmsService
    {
        Task<string> SendAsync(SmsRequest request);
    }
}
