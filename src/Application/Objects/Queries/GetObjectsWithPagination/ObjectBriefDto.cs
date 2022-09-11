using OMS.Application.Common.Mappings;
using OMS.Application.Objects.Queries.GetObjectsWithPagination;
using Object = OMS.Domain.Entities.Object;

namespace OMS.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public class ObjectBriefDto : IMapFrom<Object>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ObjectTypeDto ObjectType { get; set; } = null!;
}
