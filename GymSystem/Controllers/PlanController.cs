using GymSystem.DAL.Contexts;
using Microsoft.AspNetCore.Mvc;
using GymSystem.DAL.Repositries.Interfaces;
using GymSystem.DAL.Repositries.Classes;

namespace GymSystem.Controllers
{
    public class PlanController : Controller
    {
        private IPlanRepositry planRepositry;

        public PlanController(IPlanRepositry _planRepositry)
        {
            this.planRepositry = _planRepositry;
        }

        public async Task<IActionResult> Index()
        {
            var plans = await planRepositry.GetAll();
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
