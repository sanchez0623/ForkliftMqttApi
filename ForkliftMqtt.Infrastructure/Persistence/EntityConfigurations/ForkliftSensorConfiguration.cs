using ForkliftMqtt.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ForkliftMqtt.Infrastructure.Persistence.EntityConfigurations
{
    public class ForkliftSensorConfiguration : IEntityTypeConfiguration<ForkliftSensor>
    {
        public void Configure(EntityTypeBuilder<ForkliftSensor> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.ForkliftId)
                .IsRequired();

            builder.Property(s => s.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(s => s.Location)
                .IsRequired();

            builder.Property(s => s.Metadata)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions()) ?? new Dictionary<string, object>()
                );

            builder.HasIndex(s => s.ForkliftId);
        }
    }
}
