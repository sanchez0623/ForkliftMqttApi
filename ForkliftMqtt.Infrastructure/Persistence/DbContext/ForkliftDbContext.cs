using ForkliftMqtt.Domain.Entities;
using ForkliftMqtt.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ForkliftMqtt.Infrastructure.Persistence.EntityConfigurations;

namespace ForkliftMqtt.Infrastructure.Persistence.DbContext
{
    public class ForkliftDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<ForkliftSensor> Sensors { get; set; }
        public DbSet<SensorReading> SensorReadings { get; set; }
        public DbSet<Forklift> Forklifts { get; set; }

        public ForkliftDbContext(DbContextOptions<ForkliftDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ForkliftSensorConfiguration());
            modelBuilder.ApplyConfiguration(new SensorReadingConfiguration());
            modelBuilder.ApplyConfiguration(new ForkliftConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
