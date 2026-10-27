using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Requests
{
    public class CreateConfirmationDto
    {
        public int StudentId { get; set; }
        public int TripId { get; set; }
        public string? Notes { get; set; }

    }
}
