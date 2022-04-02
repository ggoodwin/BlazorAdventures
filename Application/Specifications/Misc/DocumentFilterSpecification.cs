using Application.Specifications.Base;
using Domain.Entities.Misc;

namespace Application.Specifications.Misc
{
    public class DocumentFilterSpecification : BlazorAdventuresSpecification<Document>
    {
        public DocumentFilterSpecification(string? searchString, string userId)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.Title.Contains(searchString) || p.Description.Contains(searchString)) && (p.IsPublic == true || (p.IsPublic == false && p.CreatedBy == userId));
            }
            else
            {
                Criteria = p => (p.IsPublic == true || (p.IsPublic == false && p.CreatedBy == userId));
            }
        }
    }
}
