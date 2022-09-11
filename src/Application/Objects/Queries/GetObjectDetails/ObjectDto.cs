using AutoMapper;
using OMS.Application.Common.Mappings;
using OMS.Application.Objects.Queries.GetObjectsWithPagination;
using OMS.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using Object = OMS.Domain.Entities.Object;

namespace OMS.Application.Objects.Queries.GetObjectDetails;

public class ObjectDto : IMapFrom<Object>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public ObjectTypeDto ObjectType { get; set; } = null!;

    public ICollection<ObjectBriefDto> RelatedObjects { get; set; } = new List<ObjectBriefDto>();

    public void Mapping(Profile profile) =>
        profile.CreateMap<Object, ObjectDto>()        
        .ForMember(dest => dest.RelatedObjects, opt => opt.MapFrom(s => s.ObjectRelationshipObjects
                                                                        .Select(o => o.RelatedObject)));

}
