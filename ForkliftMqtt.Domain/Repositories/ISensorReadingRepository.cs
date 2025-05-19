using ForkliftMqtt.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Domain.Repositories
{
    public interface ISensorReadingRepository
    {
        Task AddAsync(SensorReading reading);
        Task<IEnumerable<SensorReading>> GetBySensorIdAsync(string sensorId, DateTime? startTime = null, DateTime? endTime = null);
        Task<IEnumerable<SensorReading>> GetLatestReadingsAsync(string sensorId, int count);
        Task DeleteOlderThanAsync(DateTime cutoffDate);
    }
}
