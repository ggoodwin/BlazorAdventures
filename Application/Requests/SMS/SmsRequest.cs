namespace Application.Requests.SMS
{
    public class SmsRequest
    {
        public string? PhoneNumber { get; set; }
        public string? Code { get; set; }
        public string? Message { get; set; }
    }
}
