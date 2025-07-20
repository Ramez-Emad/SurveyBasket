using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Linq.Expressions;

namespace Persistence.Repositories;
public class GenericRepository<TEntity>(ApplicationDbContext _dbContext) : IGenericRepository<TEntity>
    where TEntity : class
{

    #region GetALl
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _dbContext.Set<TEntity>().ToListAsync(cancellationToken);


    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
        => await _dbContext.Set<TEntity>()
            .Select(selector)
            .ToListAsync(cancellationToken);


    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(ISpecifications<TEntity> specification, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator<TEntity>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification);
        return await query
            .Select(selector)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator<TEntity>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), specification);

        return await query.ToListAsync(cancellationToken);
    }


    public async Task<int> CountAsync(ISpecifications<TEntity> specifications) => await SpecificationEvaluator<TEntity>.GetQuery(
        _dbContext.Set<TEntity>(), specifications).CountAsync();

    #endregion

    #region Get

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
       => await _dbContext.Set<TEntity>().FindAsync(id);

    public async Task<TEntity?> GetAsync(ISpecifications<TEntity> spec, CancellationToken cancellationToken = default)
    {
        var query = SpecificationEvaluator<TEntity>.GetQuery(_dbContext.Set<TEntity>().AsQueryable(), spec);
        return await query.FirstOrDefaultAsync(cancellationToken);
    }
    #endregion

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);

    public void Delete(TEntity entity) =>
          _dbContext.Set<TEntity>().Remove(entity);

    public void Update(TEntity entity) =>
          _dbContext.Set<TEntity>().Update(entity);



}
