namespace OMS.Application.Objects.Services;
public interface IObjectService
{
    /// <summary>
    /// Checks for <paramref name="objectTypeId"/> existance in Object table
    /// </summary>
    /// <param name="objectTypeId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> IdExistsAsync(Guid objectId, CancellationToken cancellationToken);

    /// <summary>
    /// Checks for <paramref name="name"/> is not already present in Object table to avoid unique exceptions
    /// </summary>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Checks for <paramref name="name"/> is not already present for objects different from <paramref name="id"/> in Object table to avoid unique exceptions
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> NameExistsAsync(Guid id, string name, CancellationToken cancellationToken);
}
