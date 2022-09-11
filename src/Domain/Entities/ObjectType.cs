namespace OMS.Domain.Entities;
public class ObjectType : BaseAuditableEntity
{   
    public string Name { get; set; } = null!;
    
    public virtual ICollection<Object> Objects { get; set; } = new HashSet<Object>();
}
