namespace OMS.Domain.Entities;
public class Object: BaseAuditableEntity
{  
    public string Name { get; set; } = null!;
    public string? Description { get; set; }   
    public Guid ObjectTypeId { get; set; }  
    public virtual ObjectType ObjectType { get; set; } = null!;
    public virtual ICollection<ObjectRelationship> ObjectRelationshipObjects { get; set; }
    public virtual ICollection<ObjectRelationship> ObjectRelationshipRelatedObjects { get; set; }

    public Object()
    {
        ObjectRelationshipObjects = new HashSet<ObjectRelationship>();
        ObjectRelationshipRelatedObjects = new HashSet<ObjectRelationship>();
    }

}
 