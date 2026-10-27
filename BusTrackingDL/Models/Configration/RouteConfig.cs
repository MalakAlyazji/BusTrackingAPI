using BusTrackingDL.Models.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusTrackingDL.Models.Configration
{
    public class RouteConfig:IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("Routes");
            builder.HasKey(d => d.Id);

            builder.Property(d => d.StartingPoint)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.EndingPoint)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
