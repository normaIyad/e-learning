using Course.DAL.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Course.DAL.Repositry.Class
{
    public class GenralRepositry<T> : IGenralRepositry<T> where T : class
    {
        private readonly ApplicationDbContext context;

        public GenralRepositry (ApplicationDbContext context)
        {
            this.context=context;
        }
        public async Task AddAsync (T entity, CancellationToken cancellationToken = default)
        {
            var result = await context.Set<T>().AddAsync(entity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

        }

        public async Task<int> DeleteAsync (T entity, CancellationToken cancellationToken = default)
        {
            var result = context.Set<T>().Remove(entity);
            return await context.SaveChangesAsync(cancellationToken);

        }

        public async Task<IEnumerable<T>> GetAllAsync (Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[] inclode = null, bool isTrck = true)
        {
            IQueryable<T> entity = context.Set<T>();

            if (expression!=null)
            {
                entity=entity.Where(expression);
            }
            if (inclode!=null)
            {
                foreach (var item in inclode)
                {
                    entity=entity.Include(item);
                }
            }
            if (!isTrck)
            {
                entity=entity.AsNoTracking();
            }
            var result = await entity.ToListAsync();
            return result;
        }

        public async Task<T> GetByIdAsync (int id)
        {
            var data = await context.Set<T>().FindAsync(id);
            return data;
        }

        public Task SaveChangesAsync (CancellationToken cancellationToken = default)
        {
            context.SaveChangesAsync(cancellationToken);
            return Task.CompletedTask;
        }

        public async Task<int> UpdateAsync (T entity, CancellationToken cancellationToken = default)
        {
            var result = context.Set<T>().Update(entity);
            return await context.SaveChangesAsync(cancellationToken);
        }
    }
}
