using OMS.Application.Common.Mappings;
using OMS.Domain.Entities;

namespace OMS.Application.Objects.Queries.GetObjectsWithPagination;

public class ObjectTypeDto : IMapFrom<ObjectType>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}