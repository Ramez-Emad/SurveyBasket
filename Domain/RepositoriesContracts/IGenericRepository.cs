using System.Linq.Expressions;

namespace Domain.RepositoriesContracts;
public interface IGenericRepository<TEntity>
    where TEntity : class
{

    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default,params Expression<Func<TEntity, object>>[] includes );
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default ,params Expression<Func<TEntity, object>>[] includes);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Delete(TEntity entity);
}