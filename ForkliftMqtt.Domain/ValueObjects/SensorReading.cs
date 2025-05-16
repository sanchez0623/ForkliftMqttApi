using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// SensorReading.cs (值对象)
namespace ForkliftMqtt.Domain.ValueObjects
{
    public class SensorReading
    {
        public string SensorId { get; }
        public DateTime Timestamp { get; }
        public double Value { get; }
        public string Unit { get; }
        public Dictionary<string, object> Attributes { get; }

        public SensorReading(string sensorId, double value, string unit, Dictionary<string, object> attributes = null)
        {
            SensorId = sensorId;
            Timestamp = DateTime.UtcNow;
            Value = value;
            Unit = unit;
            Attributes = attributes ?? new Dictionary<string, object>();
        }
    }
}
