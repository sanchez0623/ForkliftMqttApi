using ForkliftMqtt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Infrastructure.Persistence.EntityConfigurations
{
    public class ForkliftConfiguration : IEntityTypeConfiguration<Forklift>
    {
        public void Configure(EntityTypeBuilder<Forklift> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Model)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(f => f.Status)
                .IsRequired()
                .HasConversion<string>();

            // 一个叉车可以有多个传感器
            builder.HasMany<ForkliftSensor>()
                .WithOne()
                .HasForeignKey(s => s.ForkliftId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
