using Microsoft.EntityFrameworkCore;
using OMS.Application.Common.Interfaces;

namespace OMS.Application.Objects.Services;

/// <summary>
/// IObjectService implementation for EF Core
/// </summary>
public class ObjectService: IObjectService
{
    private readonly IApplicationDbContext _context;

    public ObjectService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IdExistsAsync(Guid objectId, CancellationToken cancellationToken)
        => await _context.Objects.AsNoTracking().AnyAsync(o => o.Id == objectId, cancellationToken);

    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken)
        => await _context.Objects.AsNoTracking().AnyAsync(o => o.Name == name, cancellationToken);

    public async Task<bool> NameExistsAsync(Guid id, string name, CancellationToken cancellationToken)
       => await _context.Objects.AsNoTracking().AnyAsync(o => o.Name == name && o.Id != id, cancellationToken);
}
