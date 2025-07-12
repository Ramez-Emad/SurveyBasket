using Domain.RepositoriesContracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories;
public class GenericRepository<TEntity>(ApplicationDbContext _dbContext) : IGenericRepository<TEntity>
    where TEntity : class
{
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default , params Expression<Func<TEntity, object>>[] includes)
    {

        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        foreach (var include in includes)
            query = query.Include(include);

        return await query.ToListAsync();

    }


    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

    public void Delete(TEntity entity) =>
          _dbContext.Set<TEntity>().Remove(entity);

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default , params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        foreach (var include in includes)
            query = query.Include(include);

        return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id ,cancellationToken);
    }

    public void Update(TEntity entity) =>
          _dbContext.Set<TEntity>().Update(entity);

}
