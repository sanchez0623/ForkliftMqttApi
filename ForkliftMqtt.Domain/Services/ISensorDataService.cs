using ForkliftMqtt.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 领域服务接口
namespace ForkliftMqtt.Domain.Services
{
    public interface ISensorDataService
    {
        Task PublishSensorReadingAsync(SensorReading reading);
        Task SubscribeToSensorAsync(string sensorId, Action<SensorReading> callback);
        Task UnsubscribeFromSensorAsync(string sensorId);
    }
}