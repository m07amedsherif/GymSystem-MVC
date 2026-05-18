using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymSystem.DAL.Entities
{
    public class Membership : BaseEntity 
    {
        // auto-set in config
        public DateTime EndDate { get; set; }
        // = StartDate + Plan.DurationDays (in service)

        // FKs to both sides of the relationship
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;

        [NotMapped]
        public string Status => DateTime.Now < EndDate ? "Active" : "Expired";
        
        [NotMapped]
        public bool IsActive => DateTime.Now < EndDate;
    }
}