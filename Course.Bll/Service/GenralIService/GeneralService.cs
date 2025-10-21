using Course.DAL.Repositry;
using Mapster;
using System.Linq.Expressions;

namespace Course.Bll.Service.GenralIService
{
    public class GeneralService<TRequest, TResponse, TEntity>
        : IGeneralService<TRequest, TResponse, TEntity>
        where TEntity : class
    {
        private readonly IGenralRepositry<TEntity> repository;

        public GeneralService (IGenralRepositry<TEntity> repository)
        {
            this.repository=repository;
        }

        public async Task AddAsync (TRequest entity)
        {
            var entityT = entity.Adapt<TEntity>();
            await repository.AddAsync(entityT);
        }

        public async Task<int> DeleteAsync (int id)
        {
            var existingEntity = await repository.GetByIdAsync(id);
            return existingEntity==null ? 0 : await repository.DeleteAsync(existingEntity);
        }

        public async Task<IEnumerable<TResponse>> GetAllAsync (
            Expression<Func<TRequest, bool>>? expression = null,
            Expression<Func<TRequest, object>>[]? include = null,
            bool isTrack = true)
        {
            var exp = expression?.Adapt<Expression<Func<TEntity, bool>>>();
            var includes = include?.Select(x => x.Adapt<Expression<Func<TEntity, object>>>()).ToArray();
            var entities = await repository.GetAllAsync(exp, includes, isTrack);
            return entities.Adapt<IEnumerable<TResponse>>();
        }

        public async Task<TResponse?> GetByIdAsync (int id)
        {
            var entity = await repository.GetByIdAsync(id);
            return entity is null ? default : entity.Adapt<TResponse>();
        }

        public async Task<bool> Toogle (int id)
        {
            var existingEntity = await repository.GetByIdAsync(id);
            if (existingEntity==null) return false;

            var prop = typeof(TEntity).GetProperty("IsActive");
            if (prop==null||prop.PropertyType!=typeof(bool)) return false;

            var currentValue = (bool)prop.GetValue(existingEntity)!;
            prop.SetValue(existingEntity, !currentValue);

            var result = await repository.UpdateAsync(existingEntity);
            return result>0;
        }

        public async Task<int> UpdateAsync (int id, TRequest entity)
        {
            var existingEntity = await repository.GetByIdAsync(id);
            if (existingEntity==null) return 0;

            entity.Adapt(existingEntity);
            return await repository.UpdateAsync(existingEntity);
        }
    }
}
