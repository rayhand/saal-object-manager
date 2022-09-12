using MediatR;
using OMS.Application.Common.Interfaces;
using OMS.Domain.Entities;

namespace OMS.Application.Objects.Commands.CreateObject;

public record CreateObjectTypeCommand : IRequest<Guid>
{
    public string Name { get; set; } = null!;  
}

public class CreateObjectTypeCommandHandler : IRequestHandler<CreateObjectTypeCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateObjectTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateObjectTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = new ObjectType 
        {
            Name = request.Name            
        };
        _context.ObjectTypes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
       
        return entity.Id;
    }
}
