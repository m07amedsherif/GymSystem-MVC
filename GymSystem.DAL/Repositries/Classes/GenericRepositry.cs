using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymSystem.DAL.Repositries.Classes
{
    public class GenericRepositry<TEntity> : IGenericRepositry<TEntity>
        where TEntity : BaseEntity, new()
    {

        private readonly GymDbContext dbContext;
        public GenericRepositry(GymDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }
        public void Add(TEntity item)
        {
            dbContext.Set<TEntity>().Add(item);
        }

        public async Task<int> CompleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public void Delete(TEntity item)
        {
            var existingItem = dbContext.Set<TEntity>().FirstOrDefault(m => m.Id == item.Id);
            if (existingItem != null)
            {
                dbContext.Set<TEntity>().Remove(existingItem);
            }
        }

        public async Task<IEnumerable<TEntity?>> GetAll(bool isTracked, CancellationToken ct = default)
        {
            var entities = isTracked ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking();
            return await entities.ToListAsync(ct);
        }

        public async Task<TEntity?> GetById(int id, CancellationToken ct = default)
        {
            return await dbContext.Set<TEntity>().FirstOrDefaultAsync(m => m.Id == id, ct);
        }

        public void Update(TEntity item)
        {
            dbContext.Set<TEntity>().Update(item);
        }

        public async Task<TEntity?> FirstOrDefaultAsunc(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default)
        {
            var item = isTracked ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking();
            return await item.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await dbContext.Set<TEntity>().AnyAsync(predicate, ct);
        }
    }
}
