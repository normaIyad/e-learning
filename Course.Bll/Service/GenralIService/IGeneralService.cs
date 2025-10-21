using System.Linq.Expressions;

namespace Course.Bll.Service.GenralIService
{
    public interface IGeneralService<TRequest, TResponse, TEntity>
    {
        Task AddAsync (TRequest entity);
        Task<int> DeleteAsync (int id);
        Task<IEnumerable<TResponse>> GetAllAsync (Expression<Func<TRequest, bool>>? expression = null, Expression<Func<TRequest, object>>?[] inclode = null, bool isTrck = true);
        Task<TResponse?> GetByIdAsync (int id);
        Task<int> UpdateAsync (int id, TRequest entity);
        Task<bool> Toogle (int id);
    }
}
