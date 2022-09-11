using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMS.Application.Common.Interfaces;

namespace OMS.Application.Objects.Queries.GetObjectDetails;

public record GetObjectDetailsQuery : IRequest<ObjectDto>
{
    public Guid Id { get; set; }
}

public class GetObjectDetailsQueryHandler : IRequestHandler<GetObjectDetailsQuery, ObjectDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetObjectDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ObjectDto> Handle(GetObjectDetailsQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Objects
            .Include(o => o.ObjectType)
            .Include(o => o.ObjectRelationshipObjects)
                .ThenInclude(p => p.RelatedObject)
                .ThenInclude(q => q.ObjectType)
            .AsNoTracking()
            .FirstAsync(o => o.Id == request.Id, cancellationToken: cancellationToken);


        return _mapper.Map<ObjectDto>(entity);
    }
}
