using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositries.Interfaces
{
    public interface IGenericRepositry<TEntity> where TEntity : BaseEntity, new()
    {
        Task<IEnumerable<TEntity?>> GetAll(bool isTracked, CancellationToken ct = default);
        Task<TEntity?> GetById(int id, CancellationToken ct = default);
        void Add(TEntity item);
        void Update(TEntity item);
        void Delete(TEntity item);
        Task<int> CompleteAsync();
        Task<TEntity?> FirstOrDefaultAsunc(Expression<Func<TEntity, bool>> predicate, bool isTracked = false, CancellationToken ct = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
    }
}
