using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositries.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepositry<TEntity> GetRepositry<TEntity>() where TEntity:BaseEntity, new();
        public Task<int> CompeleteAsync();
    }
}
