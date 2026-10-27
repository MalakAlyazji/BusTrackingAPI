using BusTrackingDL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Responses
{
    public class RouteDto
    {
        public int RouteId { get; set; }
        public string StartingPoint { get; set; } = string.Empty;
        public string EndingPoint { get; set; } = string.Empty;
    }
}
