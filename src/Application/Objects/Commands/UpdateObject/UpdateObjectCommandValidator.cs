using FluentValidation;
using OMS.Application.Objects.Services;

namespace OMS.Application.Objects.Commands.UpdateObject;

public class UpdateObjectCommandValidator : AbstractValidator<UpdateObjectCommand>
{
    private readonly IObjectService _objectService;
    private readonly IObjectTypesService _objectTypesService;

    public UpdateObjectCommandValidator(
        IObjectService objectService,
        IObjectTypesService objectTypesService)
    {
        _objectService = objectService;
        _objectTypesService = objectTypesService;
        
        RuleFor(v => v.Name)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(500);

        RuleFor(v => v)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (cmd, cancellation) => 
                     !await _objectService.NameExistsAsync(cmd.Id, cmd.Name, cancellation))
                     .WithMessage(v => $"Name '{v.Name}' must be unique");


        RuleFor(v => v.ObjectTypeId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (id, cancellation) => 
                        await _objectTypesService.IdExistsAsync(id, cancellation))
            .WithMessage("ObjectTypeId '{PropertyValue}' not registered");

    }
}