using System.Linq.Expressions;

namespace Course.DAL.Repositry
{
    public interface IGenralRepositry<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync (Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[] inclode = null, bool isTrck = true);
        Task<T> GetByIdAsync (int id);
        Task AddAsync (T entity, CancellationToken cancellationToken = default);
        Task<int> UpdateAsync (T entity, CancellationToken cancellationToken = default);
        Task<int> DeleteAsync (T entity, CancellationToken cancellationToken = default);
        Task SaveChangesAsync (CancellationToken cancellationToken = default);
        // Task GetAllAsync<TEntity> (Expression<Func<TEntity, bool>>? exp, Expression<Func<T, object>>[] inclode, bool isTrck);
    }
}
