using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Base;
using OnlineMarket.DataBase;

namespace StudentUptBackend.Database;

public class Repository<T> : IRepository<T> where T : class, IModel
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
        DbSet = _context.Set<T>();
    }

    private Task<int> Save()
    {
        return _context.SaveChangesAsync();
    }

    public DbSet<T> DbSet { get; }

    public async Task<T> AddAsync([NotNull] T entity)
    {
        entity.Id = Guid.NewGuid().ToString();
        entity.Created = entity.Updated = DateTime.UtcNow;

        entity = (await DbSet.AddAsync(entity)).Entity;
        await Save();

        return entity;
    }

    public async Task<IEnumerable<T>> GetAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<T?> GetAsync(string id)
    {
        return await DbSet.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<T?> AddOrUpdateAsync([NotNull] T entity)
    {
        var existing = await DbSet.FirstOrDefaultAsync(item => item.Id == entity.Id);

        return existing is null ? await AddAsync(entity) : await UpdateAsync(entity.Id, entity);
    }

    public async Task<T?> UpdateAsync(string id, object newEntity)
    {
        var entity = await GetAsync(id);
        if (entity is null) return null;

        CheckUpdateObject(entity, newEntity);

        await Save();

        return DbSet.First(e => e.Id == id);
    }

    public async Task<T?> DeleteAsync(string id)
    {
        var entity = await DbSet.FirstOrDefaultAsync(item => item.Id == id);

        if (entity is null) return null;

        entity = DbSet.Remove(entity).Entity;
        await Save();

        return entity;
    }

    public async Task<IEnumerable<T>> DeleteAsync(IEnumerable<string> ids)
    {
        var entities = await DbSet.Where(e => ids.Contains(e.Id)).ToListAsync();

        DbSet.RemoveRange(entities);
        await Save();

        return entities;
    }

    private static void CheckUpdateObject(T originalObj, object updateObj)
    {
        foreach (var property in updateObj.GetType().GetProperties())
        {
            var propValue = property.GetValue(updateObj, null);
            var originalProp = originalObj.GetType().GetProperty(property.Name);
            if (propValue is not null && originalProp is not null) originalProp.SetValue(originalObj, propValue);
        }
    }
}
