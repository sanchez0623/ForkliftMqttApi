using ForkliftMqtt.Domain.ValueObjects;
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
    public class SensorReadingConfiguration : IEntityTypeConfiguration<SensorReading>
    {
        public void Configure(EntityTypeBuilder<SensorReading> builder)
        {
            builder.HasKey(r => new { r.SensorId, r.Timestamp });

            builder.Property(r => r.Value)
                .IsRequired();

            builder.Property(r => r.Unit)
                .IsRequired();

            builder.Property(r => r.Attributes)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                    v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions()) ?? new Dictionary<string, object>()
                );

            builder.HasIndex(r => r.SensorId);
            builder.HasIndex(r => r.Timestamp);
        }
    }
}
