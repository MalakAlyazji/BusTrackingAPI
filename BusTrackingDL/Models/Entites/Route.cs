using BusTrackingDL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingDL.Models.Entites
{
    public class Route:BaseEntity
    {
        public ICollection <Trip> trips { get; set; }= new List<Trip>();
        public enLocation StartingPoint {  get; set; } = enLocation.AlThumama;
        public enLocation EndingPoint { get; set; } = enLocation.University;
    }
}
