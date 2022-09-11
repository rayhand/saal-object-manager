using System.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMS.Application.Common.Exceptions;
using OMS.Application.Common.Interfaces;
using Object = OMS.Domain.Entities.Object;

namespace OMS.Application.Objects.Commands.DeleteObject;

public record DeleteObjectCommand(Guid Id) : IRequest;

public class DeleteObjectCommandHandler : IRequestHandler<DeleteObjectCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteObjectCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteObjectCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Objects
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Object), request.Id);
        }

        using var scope = new TransactionScope(
               TransactionScopeOption.Required,
               new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
               TransactionScopeAsyncFlowOption.Enabled
           );

        // Do not relay in cascade deletes, delete relationships first
        var relationShips = await _context.ObjectRelationships
                                .Where(o => o.ObjectId == entity.Id || o.RelatedObjectId == entity.Id)
                                .ToListAsync(cancellationToken: cancellationToken);
       
        _context.ObjectRelationships.RemoveRange(relationShips);
        _context.Objects.Remove(entity);
        
        //entity.AddDomainEvent(new ObjectDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
        scope.Complete();
        return Unit.Value;
    }
}
