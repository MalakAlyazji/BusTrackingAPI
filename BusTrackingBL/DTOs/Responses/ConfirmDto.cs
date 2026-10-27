using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Responses
{
    public class ConfirmDto
    {
        public int ConfirmationId { get; set; }
        public int StudentId { get; set; }
        public int TripId { get; set; }
        public string? Notes { get; set; }
        public DateTime ConfirmedAt { get; set; } = DateTime.UtcNow;
    }
}
