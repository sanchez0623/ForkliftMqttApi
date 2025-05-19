using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Application.DTOs
{
    public class SensorReadingDto
    {
        public string SensorId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}
