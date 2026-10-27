using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingDL.Models.Entites
{
    public class StudentConfirmation:BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public int TripId {  get; set; }
        public Trip Trip { get; set; } = null!;
        public DateTime ConfirmedAt { get; set; } = DateTime.UtcNow;
        public string? Notes {  get; set; }
        public bool IsCancelled { get; set; } = false;

    }
}
