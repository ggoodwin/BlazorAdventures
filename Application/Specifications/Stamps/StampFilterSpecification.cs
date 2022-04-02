using Application.Specifications.Base;
using Domain.Entities.TimeStamp;

namespace Application.Specifications.Stamps
{
    public class StampFilterSpecification : BlazorAdventuresSpecification<Stamp>
    {
        public StampFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.TheStamp.Equals(searchString);// || p.Client.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
