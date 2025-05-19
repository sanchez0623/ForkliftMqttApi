using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Application.DTOs
{
    public class ForkliftSensorDto
    {
        public string Id { get; set; }
        public string ForkliftId { get; set; }
        public string SensorType { get; set; }
        public string Location { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }
}
