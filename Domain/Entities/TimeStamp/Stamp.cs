using Domain.Contracts;

namespace Domain.Entities.TimeStamp
{
    public class Stamp : AuditableEntity<int>
    {
        public DateTime TheStamp { get; set; }
    }
}
