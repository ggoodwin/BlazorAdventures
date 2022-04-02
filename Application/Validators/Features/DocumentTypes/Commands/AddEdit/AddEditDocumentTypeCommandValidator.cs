using Application.Features.DocumentTypes.Commands.AddEdit;
using FluentValidation;

namespace Application.Validators.Features.DocumentTypes.Commands.AddEdit
{
    public class AddEditDocumentTypeCommandValidator : AbstractValidator<AddEditDocumentTypeCommand>
    {
        public AddEditDocumentTypeCommandValidator()
        {
            RuleFor(request => request.Name)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Name is required!");
            RuleFor(request => request.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => "Description is required!");
        }
    }
}
