using System.Transactions;
using MediatR;
using OMS.Application.Common.Interfaces;
using OMS.Domain.Entities;
using Object = OMS.Domain.Entities.Object;

namespace OMS.Application.Objects.Commands.CreateObject;

public record CreateObjectCommand : IRequest<Guid>
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid ObjectTypeId { get; set; }
    public ICollection<Guid> RelatedObjects { get; set; } = new List<Guid>();
}

public class CreateObjectCommandHandler : IRequestHandler<CreateObjectCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateObjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateObjectCommand request, CancellationToken cancellationToken)
    {
        using var scope = new TransactionScope(
               TransactionScopeOption.Required,
               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
               TransactionScopeAsyncFlowOption.Enabled
           );

        var entity = new Object 
        {
            Name = request.Name,
            Description = request.Description,
            ObjectTypeId = request.ObjectTypeId
        };
        _context.Objects.Add(entity);
        
        // Add Object Relationships
        foreach (var item in request.RelatedObjects)
        {
            _context.ObjectRelationships.Add(new ObjectRelationship
            {
                Object = entity,
                RelatedObjectId = item
            }); 
        }
        await _context.SaveChangesAsync(cancellationToken);

        scope.Complete();
        return entity.Id;
    }
}
