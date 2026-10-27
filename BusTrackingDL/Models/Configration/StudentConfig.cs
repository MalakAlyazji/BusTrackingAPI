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
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable(nameof(Student));
            builder.HasKey(x => x.Id);
            builder.Property(s => s.StudentName)
              .IsRequired()
              .HasMaxLength(100);
            builder.Property(s => s.Phone)
                .IsRequired()
                .HasMaxLength(20);
            builder.HasIndex(s => s.Phone)
                .IsUnique();
        }
    }
}
