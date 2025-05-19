using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Domain.Enums
{
    public enum ForkliftStatus
    {
        Idle,
        Working,
        Charging,
        Maintenance,
        Error
    }
}
