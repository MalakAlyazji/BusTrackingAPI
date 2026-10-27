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
    public class DriverConfig : IEntityTypeConfiguration<Driver>
    {
        public void Configure(EntityTypeBuilder<Driver> builder)
        {
            builder.ToTable("Drivers");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.BusNumber).HasMaxLength(20);
            builder.Property(d => d.DriverName).IsRequired().HasMaxLength(25);
            builder.Property(d => d.PasswordHash).IsRequired().HasMaxLength(255);
            builder.Property(d => d.Phone).IsRequired().HasMaxLength(20);
            builder.HasIndex(d => d.Phone).IsUnique();
        }
    }
}
