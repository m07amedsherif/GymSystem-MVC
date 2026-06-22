using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositries.Interfaces;

namespace GymSystem.BLL.Services.Classes
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork unitOfWork;

        public MemberServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        async Task<IEnumerable<MemberViewModel>> IMemberServices.GetAllMembersAsync(CancellationToken ct)
        {
            var members = await unitOfWork.GetRepositry<Member>().GetAll(false, ct);
            if (!members.Any()) return [];
            var membersViewModel = members.Select(m => new MemberViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photo,
                Gender = m.Gender.ToString(),
            });
            return membersViewModel;
        }

        async Task<MemberViewModel> IMemberServices.GetMemberDetailsAsync(int id, CancellationToken ct)
        {
            var member = await unitOfWork.GetRepositry<Member>().GetById(id, ct);
            if (member == null) return null;
            var memberViewModel = new MemberViewModel()
            {
                //Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Gender = member.Gender.ToString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
            };
            var activeMembership = await unitOfWork.GetRepositry<Membership>().FirstOrDefaultAsunc(mb => mb.Id == member.Id && mb.EndDate > DateTime.Now, false, ct);
            if(activeMembership != null)
            {
                var Activeplan = await unitOfWork.GetRepositry<Plan>().GetById(activeMembership.PlanId, ct);
                memberViewModel.PlanName = Activeplan?.Name;
                memberViewModel.MembershipStartDate = activeMembership.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = activeMembership.EndDate.ToShortDateString();
            }
            return memberViewModel;
        }

        async Task<HealthRecordViewModel> IMemberServices.GetMemberHealthRecordAsync(int id, CancellationToken ct)
        {
            var record = await unitOfWork.GetRepositry<HealthRecord>().FirstOrDefaultAsunc(hr => hr.MemberId == id, false, ct);
            if (record == null) return null;
            return new HealthRecordViewModel()
            {
                Height = record.Height,
                Weight = record.Weight,
                BloodType = record.BloodType,
                Note = record.Note,
            };
        }

        async Task<MemberToUpdateViewModel> IMemberServices.GetMemberToUpdateAsync(int id, CancellationToken ct)
        {
            var member = await unitOfWork.GetRepositry<Member>().GetById(id, ct);
            if(member == null) return null;
            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Street = member.Address.Street,
                City = member.Address.City,
                BuildingNumber = member.Address.BuildingNumber,
                Photo = member.Photo
            };
        }

        async Task<bool> IMemberServices.CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            var emailExists = await unitOfWork.GetRepositry<Member>().AnyAsync(m => m.Email == model.Email, ct);
            var phoneExists = await unitOfWork.GetRepositry<Member>().AnyAsync(m => m.Phone == model.Phone, ct);
            if (emailExists || phoneExists) return false;

            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Gender = model.Gender,
                Phone = model.Phone,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City,
                },
                HealthRecord = new HealthRecord()
                {
                    Height = model.HealthRecordViewModel.Height,
                    Weight = model.HealthRecordViewModel.Weight,
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Note = model.HealthRecordViewModel.Note,
                }
            };
            unitOfWork.GetRepositry<Member>().Add(member);
            var result = await unitOfWork.GetRepositry<Member>().CompleteAsync();
            return result > 0;
        }

        async Task<bool> IMemberServices.DeleteMemberAsync(int id, CancellationToken ct)
        {
            var member = await unitOfWork.GetRepositry<Member>().GetById(id, ct);
            if (member is null) return false;

            var HasFutureSessions = await unitOfWork.GetRepositry<Booking>().AnyAsync(b => b.MemberId == id && b.Session.EndDate > DateTime.Now, ct);
            if (HasFutureSessions) return false;

            unitOfWork.GetRepositry<Member>().Delete(member);

            var result = await unitOfWork.GetRepositry<Member>().CompleteAsync();
            return result > 0;
        }

        async Task<bool> IMemberServices.UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            var member = await unitOfWork.GetRepositry<Member>().GetById(id, ct);
            if (member is null) return false;
            if(await unitOfWork.GetRepositry<Member>().AnyAsync(m => m.Email == model.Email && m.Id != id, ct) || 
                await unitOfWork.GetRepositry<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct))
            {
                return false;
            }

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.UpdatedAt = DateTime.Now;

            unitOfWork.GetRepositry<Member>().Update(member);

            var result = await unitOfWork.GetRepositry<Member>().CompleteAsync();
            return result > 0;
        }
    }
}
