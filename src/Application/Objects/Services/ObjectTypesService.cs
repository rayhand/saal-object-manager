using Microsoft.EntityFrameworkCore;
using OMS.Application.Common.Interfaces;

namespace OMS.Application.Objects.Services;
public class ObjectTypesService : IObjectTypesService
{
    private readonly IApplicationDbContext _context;

    public ObjectTypesService(IApplicationDbContext context)
    {
        _context = context;
    }
   
    public async Task<bool> IdExistsAsync(Guid objectTypeId, CancellationToken cancellationToken) 
        => await _context.ObjectTypes.AsNoTracking().AnyAsync(o => o.Id == objectTypeId, cancellationToken);
   
}
