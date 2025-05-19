using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Application.Commands
{
    public class PublishSensorReadingCommand : IRequest<bool>
    {
        public string SensorId { get; set; }
        public double Value { get; set; }
        public string Unit { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
    }
}
