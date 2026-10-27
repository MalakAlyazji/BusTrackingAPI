using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingDL.Models.Entites
{
    public class Admin:BaseEntity
    {
        public string UserName {  get; set; }=string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public ICollection<Trip> Trips { get; set; }=new List<Trip>();
        public bool IsActive {  get; set; }=true;
    }
}
