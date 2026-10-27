using BusTrackingDL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Responses
{
    public class TripDto
    {
        public int TripId {  get; set; }
        public int RouteId { get; set; }
        public int DriverId { get; set; }
        public TimeSpan TripTime { get; set; }
        public string TripStatus { get; set; }
        public string TripDay { get; set; } = string.Empty;
    }
}
