namespace Application.Requests.Mail
{
    public class MailRequest
    {
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? HtmlBody { get; set; }
        public string? To { get; set; }
        public string? ToName { get; set; }
    }
}
