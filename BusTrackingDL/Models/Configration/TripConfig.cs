using BusTrackingDL.Models.Entites;
using BusTrackingDL.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingDL.Models.Configration
{
    public class TripConfig : IEntityTypeConfiguration<Trip>
    {
        public void Configure(EntityTypeBuilder<Trip> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.TripStatus).HasConversion<string>().IsRequired();
            builder.HasOne(a=>a.Admin).WithMany(s=>s.Trips).HasForeignKey(a=>a.AdminId).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(a => a.DriverInfo).WithMany(s => s.trips).HasForeignKey(a => a.DriverId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(a => a.RouteInfo).WithMany(s => s.trips).HasForeignKey(a => a.RouteId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(t => new{ t.DriverId, t.TripDay, t.TripTime}) .IsUnique();
        }
    }
}
