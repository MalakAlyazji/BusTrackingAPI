using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Requests
{
    public class CreateDriverDto
    {
        public string DriverName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? BusNumber { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}