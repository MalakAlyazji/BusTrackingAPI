using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Requests
{
    public class changePasswordDto
    {
        public string userName {  get; set; }
        public string oldPassword { get; set; }
        public string newPassword {  get; set; }
    }
}
