using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Domain.Exceptions
{
    public class SensorNotFoundException : DomainException
    {
        public SensorNotFoundException() { }
        public SensorNotFoundException(string message) : base(message) { }
        public SensorNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
