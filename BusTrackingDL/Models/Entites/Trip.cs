using BusTrackingDL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BusTrackingDL.Models.Entites
{
    public class Trip: BaseEntity
    {
        public Admin? Admin { get; set; }
        public int? AdminId {  get; set; }

        public ICollection<StudentConfirmation> StudentConfirmations {  get; set; }
        = new List<StudentConfirmation>();

        public int RouteId { get; set; }
        public Route RouteInfo { get; set; } = null!;
        public Driver DriverInfo { get; set; } = null!;
        public int DriverId { get; set; }
        public TimeSpan TripTime { get; set; }
        public enStatus TripStatus { get; set; } = enStatus.Scheduled;
        public DateTime? DriverArrivedAt { get; set; }
        public DateTime? ActualDepartureTime { get; set; }
        public DayOfWeek TripDay { get; set; }

    }
}
