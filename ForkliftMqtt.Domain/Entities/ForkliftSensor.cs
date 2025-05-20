// ForkliftSensor.cs (领域实体)
using System.Text.Json;

namespace ForkliftMqtt.Domain.Entities
{
    public class ForkliftSensor
    {
        public string Id { get; private set; }
        public string ForkliftId { get; private set; }
        public SensorType Type { get; private set; }
        public string Location { get; private set; }
        public string Metadata { get; private set; }

        private ForkliftSensor() { }

        public ForkliftSensor(string id, string forkliftId, SensorType type, string location, string metadata = null)
        {
            Id = id;
            ForkliftId = forkliftId;
            Type = type;
            Location = location;
            Metadata = metadata ?? string.Empty;
        }

        // 添加工厂方法支持EF Core迁移
        public static ForkliftSensor Create(string id, string forkliftId, SensorType type, string location, string metadata)
        {
            return new ForkliftSensor
            {
                Id = id,
                ForkliftId = forkliftId,
                Type = type,
                Location = location,
                Metadata = metadata ?? string.Empty
            };
        }
    }

    public enum SensorType
    {
        Temperature,
        Humidity,
        Proximity,
        Pressure,
        Acceleration,
        Gps,
        Battery
    }
}