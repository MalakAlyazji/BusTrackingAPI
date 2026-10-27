using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusTrackingDL.Models.Entites
{
    public class Driver:BaseEntity
    {
        public string DriverName {  get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive {  get; set; } = true;
        public string? BusNumber {  get; set; }
        public string PasswordHash {get; set; } = string.Empty;
        public ICollection <Trip> trips { get; set; } =new List<Trip> ();
    }
}
