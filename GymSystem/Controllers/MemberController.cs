using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class MemberController : Controller
    {
        public IMemberServices MemberServices { get; }

        public MemberController(IMemberServices memberServices)
        {
            MemberServices = memberServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await MemberServices.GetAllMembersAsync(ct);
            return View(members);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) {
                return View(nameof(Create), model);
            }
            var result = await MemberServices.CreateMemberAsync(model, ct);
            if (result)
            {
                TempData["Success"] = "Member created successfuly";
            }
            else
            {
                TempData["Fail"] = "Failed to create member";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await MemberServices.GetMemberDetailsAsync(id, ct);
            if (member == null)
            {
                TempData["Fail"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpGet]
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var record = await MemberServices.GetMemberHealthRecordAsync(id, ct);
            if (record == null)
            {
                TempData["Fail"] = "Health record not found";
                return RedirectToAction(nameof(Index));
            }
            return View(record);
        }
    }
}
