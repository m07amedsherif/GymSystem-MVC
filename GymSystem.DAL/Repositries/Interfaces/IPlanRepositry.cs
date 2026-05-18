using GymSystem.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Repositries.Interfaces
{
    public interface IPlanRepositry
    {
        Task<IEnumerable<Plan?>> GetAll();
        Task<Plan?> GetById(int id);
        void Add(Plan plan);
        void Update(Plan plan);
        void Delete(Plan plan);
        Task<int> CompleteAsync();
    }
}
