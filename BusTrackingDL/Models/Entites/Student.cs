using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingDL.Models.Entites
{
    public class Student: BaseEntity
    {
        public string StudentName { get; set; } = string.Empty;
        public string Phone { get; set; }=string.Empty;
        public ICollection<StudentConfirmation> Confirmations { get; set; }=new List<StudentConfirmation>();
    }
}
