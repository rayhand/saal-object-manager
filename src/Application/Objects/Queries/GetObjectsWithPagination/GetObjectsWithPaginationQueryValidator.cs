using FluentValidation;

namespace OMS.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class GetObjectsWithPaginationQueryValidator : AbstractValidator<GetObjectsWithPaginationQuery>
{
    public GetObjectsWithPaginationQueryValidator()
    {
        //RuleFor(x => x.SearchQuery)
        //    .NotEmpty().WithMessage("Search term is required");

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}
