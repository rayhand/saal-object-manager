namespace OMS.Application.Objects.Services;
public interface IObjectTypesService
{
    /// <summary>
    /// Checks for objectTypeId existance in ObjectTypes table
    /// </summary>
    /// <param name="objectTypeId"></param>
    /// <returns></returns>
    Task<bool> IdExistsAsync(Guid objectTypeId, CancellationToken cancellationToken);
}
