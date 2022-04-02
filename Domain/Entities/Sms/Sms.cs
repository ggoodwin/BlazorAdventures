using Domain.Contracts;

namespace Domain.Entities.Sms
{
    public class Sms : AuditableEntity<int>
    {
        public string? Sid { get; set; }
        public string? Code { get; set; }
        public string? UserId { get; set; }
        public bool Verified { get; set; } = false;
    }
}
