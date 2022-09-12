using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OMS.Infrastructure.Common;

public static class DbSetExtensions
{
    /// <summary>
    /// Use this with caution, transaction is recommended, “Read before write” can violate data integrity without being put inside a transaction control.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="dbSet"></param>
    /// <param name="entity"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static EntityEntry<T>? AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>>? predicate = null) where T : class, new()
    {
        var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
        return !exists ? dbSet.Add(entity) : null;
    }
}
