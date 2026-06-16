using Microsoft.AspNetCore.Mvc;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositries.Interfaces;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private readonly IGenericRepositry<Plan> planRepositry;

        public PlanController(IGenericRepositry<Plan> _planRepositry)
        {
            this.planRepositry = _planRepositry;
        }

        public async Task<IActionResult> Index(CancellationToken token)
        {
            var plans = await planRepositry.GetAll(false, token);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            var plan = await planRepositry.GetById(id);
            if (plan == null) RedirectToAction(nameof(Index));
            return View(plan);
        }
    }
}
