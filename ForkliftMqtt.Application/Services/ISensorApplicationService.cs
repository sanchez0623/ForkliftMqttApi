using ForkliftMqtt.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Application.Services
{
    public interface ISensorApplicationService
    {
        Task<IEnumerable<ForkliftSensorDto>> GetAllSensorsAsync();
        Task<ForkliftSensorDto> GetSensorByIdAsync(string id);
        Task<bool> PublishSensorReadingAsync(SensorReadingDto readingDto);
        Task<bool> StartMonitoringSensorAsync(string sensorId);
        Task<bool> StopMonitoringSensorAsync(string sensorId);
    }
}
