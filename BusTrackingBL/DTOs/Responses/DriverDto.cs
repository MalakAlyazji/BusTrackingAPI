using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Responses
{
    public class DriverDto
    {
        public int ID {  get; set; }
        public string DriverName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? BusNumber { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
