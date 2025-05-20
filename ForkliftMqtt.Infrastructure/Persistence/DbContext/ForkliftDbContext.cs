using ForkliftMqtt.Domain.Entities;
using ForkliftMqtt.Domain.Enums;
using ForkliftMqtt.Domain.ValueObjects;
using ForkliftMqtt.Infrastructure.Persistence.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

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

            // 添加种子数据
            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // 种子数据 - 叉车
            modelBuilder.Entity<Forklift>().HasData(
                new
                {
                    Id = "FL-001",
                    Name = "叉车一号",
                    Model = "XC-2023",
                    Status = ForkliftStatus.Idle,
                    LastMaintenanceDate = new DateTime(2025, 5, 20)//DateTime.UtcNow
                },
                new
                {
                    Id = "FL-002",
                    Name = "叉车二号",
                    Model = "XC-2023",
                    Status = ForkliftStatus.Working,
                    LastMaintenanceDate = new DateTime(2025, 5, 20).AddDays(-30)//DateTime.UtcNow.AddDays(-30)
                }
            );

            // 种子数据 - 传感器
            modelBuilder.Entity<ForkliftSensor>().HasData(
                new
                {
                    Id = "SENS-001",
                    ForkliftId = "FL-001",
                    Type = SensorType.Temperature,
                    Location = "前部",
                    Metadata = "{\"range\":\"0-100\",\"alertThreshold\":80}"
                },
                new
                {
                    Id = "SENS-002",
                    ForkliftId = "FL-001",
                    Type = SensorType.Humidity,
                    Location = "中部",
                    Metadata = "{\"range\":\"0-100\",\"alertThreshold\":90}"
                },
                new
                {
                    Id = "SENS-003",
                    ForkliftId = "FL-002",
                    Type = SensorType.Battery,
                    Location = "电池仓",
                    Metadata = "{\"capacity\":\"200Ah\",\"alertThreshold\":20}"
                }
            );

            // 您可以添加更多种子数据
        }
    }
}
