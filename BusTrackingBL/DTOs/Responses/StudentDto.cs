using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingBL.DTOs.Responses
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
