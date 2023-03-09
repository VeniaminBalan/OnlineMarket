using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Base;

namespace OnlineMarket.Utils.Repository;

public interface IRepository<T> where T : class, IModel // CRUD Repository
{
    public DbSet<T> DbSet { get; set; }
    //Create
    public Task<ActionResult<T>> Add(T request);
    //Read
    public Task<ActionResult<IEnumerable<T>>> Get();
    public Task<ActionResult<T>> Get(string id);
    //Update
    public Task<ActionResult<T>> Update(string id, object request);
    //Delete
    public Task<ActionResult<T>> Delete(string id);
}