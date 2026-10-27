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
    public class StudentConfirmationConfig : IEntityTypeConfiguration<StudentConfirmation>
    {
        public void Configure(EntityTypeBuilder<StudentConfirmation> builder)
        {
            builder.ToTable("StudentConfirmation");
            builder.HasKey(s=>s.Id);
            builder.Property(s => s.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            builder.Property(a => a.Notes).HasMaxLength(500);
            builder.HasOne(s=>s.Student).WithMany(c=>c.Confirmations)
                .HasForeignKey(c=>c.StudentId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(sc => sc.Trip)
                .WithMany(t => t.StudentConfirmations)
                .HasForeignKey(sc => sc.TripId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
