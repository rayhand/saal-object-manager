using AutoMapper;
using AutoMapper.QueryableExtensions;
using OMS.Application.Common.Interfaces;
using OMS.Application.Common.Mappings;
using OMS.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OMS.Application.TodoItems.Queries.GetTodoItemsWithPagination;

public record GetObjectsWithPaginationQuery : IRequest<PaginatedList<ObjectBriefDto>>
{
    public string SearchQuery { get; set; } = string.Empty;
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetObjectsWithPaginationQueryHandler : IRequestHandler<GetObjectsWithPaginationQuery, PaginatedList<ObjectBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetObjectsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<ObjectBriefDto>> Handle(GetObjectsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        // TODO: Consider using Specification Pattern if possible

        return await _context.Objects
            .Where(x => string.IsNullOrEmpty(request.SearchQuery) || (x.Name.Contains(request.SearchQuery) || x.Description.Contains(request.SearchQuery) ))
            .OrderBy(x => x.Name)
            .AsNoTracking()
            .ProjectTo<ObjectBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
