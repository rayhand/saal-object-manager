using FluentValidation;

namespace OMS.Application.Objects.Commands.CreateObject;

public class CreateObjectTypeCommandValidator : AbstractValidator<CreateObjectTypeCommand>
{
    public CreateObjectTypeCommandValidator()
    {        
        RuleFor(v => v.Name)
            .MaximumLength(50)
            .NotEmpty();
    }
}
