using OMS.Application.Common.Exceptions;
using OMS.Application.Common.Interfaces;
using OMS.Domain.Entities;
using MediatR;
using Object = OMS.Domain.Entities.Object;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace OMS.Application.Objects.Commands.UpdateObject;

public record UpdateObjectCommand : IRequest
{
    public Guid Id { get; init; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid ObjectTypeId { get; set; }

    public ICollection<Guid> RelatedObjects { get; set; } = new List<Guid>();
}

public class UpdateObjectCommandHandler : IRequestHandler<UpdateObjectCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateObjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateObjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Objects
            .Include(o => o.ObjectType)
            .Include(o => o.ObjectRelationshipObjects)
                .ThenInclude(p => p.RelatedObject)
                .ThenInclude(q => q.ObjectType)
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Object), request.Id);
        }

        using var scope = new TransactionScope(
               TransactionScopeOption.Required,
               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
               TransactionScopeAsyncFlowOption.Enabled
           );

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.ObjectTypeId = request.ObjectTypeId;

       
        SyncRelationships(request, entity);

        await _context.SaveChangesAsync(cancellationToken);
        scope.Complete();
        return Unit.Value;
    }

    /// <summary>
    /// Sync Object Relationships
    /// </summary>
    /// <param name="request"></param>
    /// <param name="entity"></param>
    private void SyncRelationships(UpdateObjectCommand request, Object entity)
    {
        // - - Sync Relationships --
        // Delete removed relationships
        foreach (var relationship in entity.ObjectRelationshipObjects)
        {

            if (!request.RelatedObjects.Contains(relationship.RelatedObjectId))
            {
                _context.ObjectRelationships.Remove(relationship);
            }
        }

        // Add new relationships if not exists already
        foreach (var relatedObjectId in request.RelatedObjects)
        {
            if (!entity.ObjectRelationshipObjects.Any(o => o.RelatedObjectId == relatedObjectId))
            {
                _context.ObjectRelationships.Add(new ObjectRelationship
                {
                    Object = entity,
                    RelatedObjectId = relatedObjectId
                });
            };
        }
    }
}
