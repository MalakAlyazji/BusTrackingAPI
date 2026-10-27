using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Requests
{
    public class AddDriverDto
    {
        [Required]
        [MinLength(2)]
        public string DriverName { get; set; } = string.Empty;
      
        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;
        public string? BusNumber { get; set; }
        [Required]
        [MinLength(8)]
        public string password { get; set; }
    }
}
