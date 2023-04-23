using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Base;

namespace StudentUptBackend.Database;

public interface IRepository<T> where T : class, IModel
{
    DbSet<T> DbSet { get; }
    Task<T> AddAsync([NotNull] T entity);
    Task<T?> GetAsync(string id);
    Task<IEnumerable<T>> GetAsync();
    Task<T?> AddOrUpdateAsync([NotNull] T entity);
    Task<T?> UpdateAsync(string id ,object entity);
    Task<T?> DeleteAsync(string id);
    Task<IEnumerable<T>> DeleteAsync(IEnumerable<string> ids);
}
