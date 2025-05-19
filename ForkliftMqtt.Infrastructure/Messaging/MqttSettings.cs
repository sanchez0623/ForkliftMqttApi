using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Infrastructure.Messaging
{
    public class MqttSettings
    {
        public string BrokerHost { get; set; }
        public int BrokerPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientId { get; set; }
        public string TopicPrefix { get; set; } = "forklift";
    }
}
