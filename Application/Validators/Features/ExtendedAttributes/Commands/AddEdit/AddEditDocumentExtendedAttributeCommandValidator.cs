using Domain.Entities.ExtendedAttributes;
using Domain.Entities.Misc;

namespace Application.Validators.Features.ExtendedAttributes.Commands.AddEdit
{
    public class AddEditDocumentExtendedAttributeCommandValidator : AddEditExtendedAttributeCommandValidator<int, int, Document, DocumentExtendedAttribute>
    {
        public AddEditDocumentExtendedAttributeCommandValidator()
        {
            // you can override the validation rules here
        }
    }
}
