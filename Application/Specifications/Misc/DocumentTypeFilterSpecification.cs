using Application.Specifications.Base;
using Domain.Entities.Misc;

namespace Application.Specifications.Misc
{
    public class DocumentTypeFilterSpecification : BlazorAdventuresSpecification<DocumentType>
    {
        public DocumentTypeFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
