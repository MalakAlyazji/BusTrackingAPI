using BusTrackingDL.Models.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusTrackingDL.Models.Configration
{
    public class AdminConfig : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.ToTable("Admins");
            builder.HasKey(d=>d.Id);
            builder.Property(d => d.UserName).IsRequired().HasMaxLength(20);
            builder.Property(d => d.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(d => d.UserName).IsUnique();
        }
    }
}
