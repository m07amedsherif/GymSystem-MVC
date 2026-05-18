using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Repositries.Classes
{
    public class PlanRepositry : IPlanRepositry
    {
        private GymDbContext context;
        public PlanRepositry(GymDbContext _context)
        {
            this.context = _context;
        }

        public void Add(Plan plan)
        {
            context.Plans.Add(plan);
        }

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Delete(Plan plan)
        {
            var existingPlan = context.Plans.FirstOrDefault(p => p.Id == plan.Id);
            if (existingPlan != null) {
                context.Plans.Remove(existingPlan);
            }
        }

        public async Task<IEnumerable<Plan?>> GetAll()
        {
            return await context.Plans.ToListAsync();
        }

        public async Task<Plan?> GetById(int id)
        {
            return await context.Plans.FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(Plan plan)
        {
            context.Plans.Update(plan);
        }
    }
}
