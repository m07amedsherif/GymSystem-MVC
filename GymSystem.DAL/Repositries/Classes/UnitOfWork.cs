using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositries.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext dbContext;
        private readonly Dictionary<string, object> _Repos = [];
        public UnitOfWork(GymDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> CompeleteAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public IGenericRepositry<TEntity> GetRepositry<TEntity>() where TEntity : BaseEntity, new()
        {
            var typename = typeof(TEntity).Name;
            if (_Repos.TryGetValue(typename, out object oldRepositry))
            {
                return (IGenericRepositry<TEntity>) oldRepositry;
            }
            var newRepo = new GenericRepositry<TEntity>(dbContext);
            _Repos[typename] = newRepo;
            return newRepo;
        }
    }
}
