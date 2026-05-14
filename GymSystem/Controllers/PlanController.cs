using GymSystem.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly GymDbContext dbContext = new GymDbContext();
        public IActionResult Index()
        {
            var plans = dbContext.Plans.ToList();
            return View(plans);
        }

        public IActionResult Details(int id)
        {
            var plan = dbContext.Plans.FirstOrDefault(p => p.Id == id);
            if (plan == null) RedirectToAction(nameof(Index));
            return View(plan);
        }
    }
}
