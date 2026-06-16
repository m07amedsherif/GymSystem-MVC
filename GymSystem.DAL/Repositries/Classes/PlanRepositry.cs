using GymSystem.DAL.Contexts;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositries.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymSystem.DAL.Repositries.Classes
{
    public class PlanRepositry : GenericRepositry<Plan>, IPlanRepositry
    {
        private GymDbContext context;
        public PlanRepositry(GymDbContext _context)
            : base(_context)
        {
            this.context = _context;
        }
    }
}
