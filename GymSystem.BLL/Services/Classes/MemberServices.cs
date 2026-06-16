using GymSystem.BLL.Services.Interfaces;
using GymSystem.BLL.ViewModels.MembersViewModels;
using GymSystem.DAL.Entities;
using GymSystem.DAL.Repositries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Services.Classes
{
    internal class MemberServices : IMemberServices
    {
        private readonly IGenericRepositry<Member> memberRepositry;
        private readonly IGenericRepositry<Membership> membershipRepositry;
        private readonly IGenericRepositry<Plan> planRepositry;
        private readonly IGenericRepositry<HealthRecord> healthRecordRepositry;
        private readonly IGenericRepositry<Booking> bookingRepositry;

        public MemberServices(IGenericRepositry<Member> _memberRepositry, IGenericRepositry<Membership> _membershipRepositry, 
            IGenericRepositry<Plan> _planRepositry, IGenericRepositry<HealthRecord> _healthRecordRepositry, IGenericRepositry<Booking> _bookingRepositry)
        {
            memberRepositry = _memberRepositry;
            membershipRepositry = _membershipRepositry;
            planRepositry = _planRepositry;
            healthRecordRepositry = _healthRecordRepositry;
            bookingRepositry = _bookingRepositry;
        }

        async Task<IEnumerable<MemberViewModel>> IMemberServices.GetAllMembersAsync(CancellationToken ct)
        {
            var members = await memberRepositry.GetAll(false, ct);
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
            var member = await memberRepositry.GetById(id, ct);
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
            var activeMembership = await membershipRepositry.FirstOrDefaultAsunc(mb => mb.Id == member.Id && mb.EndDate > DateTime.Now, false, ct);
            if(activeMembership != null)
            {
                var Activeplan = await planRepositry.GetById(activeMembership.PlanId, ct);
                memberViewModel.PlanName = Activeplan?.Name;
                memberViewModel.MembershipStartDate = activeMembership.CreatedAt.ToShortDateString();
                memberViewModel.MembershipEndDate = activeMembership.EndDate.ToShortDateString();
            }
            return memberViewModel;
        }

        async Task<HealthRecordViewModel> IMemberServices.GetMemberHealthRecordAsync(int id, CancellationToken ct)
        {
            var record = await healthRecordRepositry.FirstOrDefaultAsunc(hr => hr.MemberId == id, false, ct);
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
            var member = await memberRepositry.GetById(id, ct);
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
            var emailExists = await memberRepositry.AnyAsync(m => m.Email == model.Email, ct);
            var phoneExists = await memberRepositry.AnyAsync(m => m.Phone == model.Phone, ct);
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
            memberRepositry.Add(member);
            var result = await memberRepositry.CompleteAsync();
            return result > 0;
        }

        async Task<bool> IMemberServices.DeleteMemberAsync(int id, CancellationToken ct)
        {
            var member = await memberRepositry.GetById(id, ct);
            if (member is null) return false;

            var HasFutureSessions = await bookingRepositry.AnyAsync(b => b.MemberId == id && b.Session.EndDate > DateTime.Now, ct);
            if (HasFutureSessions) return false;

            memberRepositry.Delete(member);

            var result = await memberRepositry.CompleteAsync();
            return result > 0;
        }

        async Task<bool> IMemberServices.UpdateMemberDetailsAsync(int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            var member = await memberRepositry.GetById(id, ct);
            if (member is null) return false;
            if(await memberRepositry.AnyAsync(m => m.Email == model.Email && m.Id != id, ct) || 
                await memberRepositry.AnyAsync(m => m.Phone == model.Phone && m.Id != id, ct))
            {
                return false;
            }

            member.Email = model.Email;
            member.Phone = model.Phone;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.City = model.City;
            member.Address.Street = model.Street;
            member.UpdatedAt = DateTime.Now;

            memberRepositry.Update(member);

            var result = await memberRepositry.CompleteAsync();
            return result > 0;
        }
    }
}
