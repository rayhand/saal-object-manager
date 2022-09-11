using OMS.Application.Common.Mappings;
using OMS.Domain.Entities;

namespace OMS.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class ObjectTypeDto : IMapFrom<ObjectType>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}