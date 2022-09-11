using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OMS.Application.Common.Interfaces;
using OMS.Application.Common.Mappings;
using OMS.Application.Common.Models;
using OMS.Application.Objects.Queries.GetObjectsWithPagination;

namespace OMS.Application.ObjectTypes.Queries.GetObjectsTypesWithPagination;

public record GetObjectTypesWithPaginationQuery : IRequest<PaginatedList<ObjectTypeDto>>
{
    public string SearchQuery { get; set; } = string.Empty;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetObjectTypeWithPaginationQueryHandler : IRequestHandler<GetObjectTypesWithPaginationQuery, PaginatedList<ObjectTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetObjectTypeWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ObjectTypeDto>> Handle(GetObjectTypesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        // TODO: Consider using Specification Pattern if possible

        return await _context.ObjectTypes
            .Where(x => string.IsNullOrEmpty(request.SearchQuery) || x.Name.Contains(request.SearchQuery))
            .OrderBy(x => x.Name)
            .AsNoTracking()
            .ProjectTo<ObjectTypeDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
