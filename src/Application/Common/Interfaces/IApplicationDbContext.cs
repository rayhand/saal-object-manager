using OMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Object = OMS.Domain.Entities.Object;

namespace OMS.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Object> Objects { get; }
    DbSet<ObjectRelationship> ObjectRelationships { get; }
    DbSet<ObjectType> ObjectTypes { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
