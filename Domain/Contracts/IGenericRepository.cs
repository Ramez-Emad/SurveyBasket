using System.Linq.Expressions;

namespace Domain.Contracts;
public interface IGenericRepository<TEntity>
    where TEntity : class
{

    #region Get All
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity> specification, CancellationToken cancellationToken = default);

    Task<IEnumerable<TResult>> GetAllAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TResult>> GetAllAsync<TResult>(
        ISpecifications<TEntity> specification,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default);

    Task<int> CountAsync(ISpecifications<TEntity> specifications);


    #endregion

    #region Get
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    public Task<TEntity?> GetAsync(ISpecifications<TEntity> spec, CancellationToken cancellationToken = default);

    #endregion



    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);

}