using System.ComponentModel.DataAnnotations.Schema;

namespace OMS.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
}
