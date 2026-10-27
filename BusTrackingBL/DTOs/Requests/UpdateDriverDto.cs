using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Requests
{
    public class UpdateDriverDto
    {
        public string DriverName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? BusNumber { get; set; }
    }
}
