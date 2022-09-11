using FluentValidation;
using OMS.Application.Objects.Services;

namespace OMS.Application.Objects.Commands.CreateObject;

public class CreateObjectCommandValidator : AbstractValidator<CreateObjectCommand>
{
    private readonly IObjectService _objectService;
    private readonly IObjectTypesService _objectTypesService;

    public CreateObjectCommandValidator(
        IObjectService objectService,
        IObjectTypesService objectTypesService )
    {
        _objectService = objectService;
        _objectTypesService = objectTypesService;
                
        
        RuleFor(v => v.Name)
            .MaximumLength(100)
            .NotEmpty();

        RuleFor(v => v.Description)
            .MaximumLength(500);

        RuleFor(v => v.Name)
            .Cascade(CascadeMode.Stop)
            .MustAsync(async (name, cancellation) =>
            {
                bool exists = await _objectService.NameExistsAsync(name, cancellation);
                return !exists;
            }).WithMessage("Name '{PropertyValue}' must be unique");


        RuleFor(v => v.ObjectTypeId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MustAsync(async (id, cancellation) =>
            {
                bool exists = await _objectTypesService.IdExistsAsync(id, cancellation);
                return exists;
            }).WithMessage("ObjectTypeId Id:'{PropertyValue}' not registered");

    }
}
