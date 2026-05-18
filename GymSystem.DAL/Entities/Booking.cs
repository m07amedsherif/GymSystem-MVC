namespace GymSystem.DAL.Entities
{
    public class Booking : BaseEntity
    {
        public bool IsAttended { get; set; } = false;
        // default false per spec §8

        // FKs to both sides
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
    }
}