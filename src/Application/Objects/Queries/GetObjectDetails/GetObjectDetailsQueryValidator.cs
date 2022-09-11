using FluentValidation;
using OMS.Application.Objects.Queries.GetObjectDetails;
using OMS.Application.Objects.Services;

namespace OMS.Application.Objects.Commands.UpdateObject;

public class GetObjectDetailsQueryValidator : AbstractValidator<GetObjectDetailsQuery>
{
    private readonly IObjectService _objectService;

    public GetObjectDetailsQueryValidator(
        IObjectService objectService
        )
    {
        _objectService = objectService;

        RuleFor(v => v.Id)
           .Cascade(CascadeMode.Stop)
           .MustAsync(async (id, cancellation) =>
           {
               bool exists = await _objectService.IdExistsAsync(id, cancellation);
               return exists;
           }).WithMessage("Object Id:'{PropertyValue}' not found");
    }
}