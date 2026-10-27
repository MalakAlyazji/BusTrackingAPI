using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Requests
{
    public class UpdateTripDto
    {
        [Range(1, int.MaxValue)]
        public int DriverId { get; set; }
        [Range(1, int.MaxValue)]
        public int RouteId { get; set; }
        public TimeSpan TripTime { get; set; }
        public DayOfWeek TripDay { get; set; }
    }
}
