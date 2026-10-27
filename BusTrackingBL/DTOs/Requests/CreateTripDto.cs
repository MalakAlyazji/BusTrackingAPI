using BusTrackingDL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Requests
{
    public class CreateTripDto
    {
        public int? AdminId { get; set; }
        public int RouteId { get; set; }
        public int DriverId { get; set; }
        public TimeSpan TripTime { get; set; }
        public DayOfWeek TripDay { get; set; }
    }
}
