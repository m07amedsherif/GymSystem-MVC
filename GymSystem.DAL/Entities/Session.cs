using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.DAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
        public Trainer Trainer { get; set; } = null!;
        public int TrainerId { get; set; }
        public Category Category { get; set; } = null!;
        public int CategoryId { get; set; } 
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}
