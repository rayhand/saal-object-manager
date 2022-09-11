using FluentValidation;
using OMS.Application.ObjectTypes.Queries.GetObjectsTypesWithPagination;

namespace OMS.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class GetObjectTypesWithPaginationQueryValidator : AbstractValidator<GetObjectTypesWithPaginationQuery>
{
    public GetObjectTypesWithPaginationQueryValidator()
    {

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
