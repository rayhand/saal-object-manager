namespace OMS.Domain.Entities;
public  class ObjectRelationship
{
    public Guid ObjectId { get; set; }
    public Guid RelatedObjectId { get; set; }    
    public virtual Object Object { get; set; } = null!;
    public virtual Object RelatedObject { get; set; } = null!;
}
  