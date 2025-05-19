using ForkliftMqtt.Domain.Repositories;
using ForkliftMqtt.Domain.ValueObjects;
using ForkliftMqtt.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Infrastructure.Persistence.Repositories
{
    public class SensorReadingRepository : ISensorReadingRepository
    {
        private readonly ForkliftDbContext _dbContext;
        private readonly ILogger<SensorReadingRepository> _logger;

        public SensorReadingRepository(
            ForkliftDbContext dbContext,
            ILogger<SensorReadingRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(SensorReading reading)
        {
            await _dbContext.SensorReadings.AddAsync(reading);
            await _dbContext.SaveChangesAsync();
            _logger.LogDebug("Sensor reading for {SensorId} at {Timestamp} saved successfully",
                reading.SensorId, reading.Timestamp);
        }

        public async Task<IEnumerable<SensorReading>> GetBySensorIdAsync(
            string sensorId,
            DateTime? startTime = null,
            DateTime? endTime = null)
        {
            var query = _dbContext.SensorReadings
                .Where(r => r.SensorId == sensorId);

            if (startTime.HasValue)
            {
                query = query.Where(r => r.Timestamp >= startTime.Value);
            }

            if (endTime.HasValue)
            {
                query = query.Where(r => r.Timestamp <= endTime.Value);
            }

            return await query
                .OrderByDescending(r => r.Timestamp)
                .ToListAsync();
        }

        public async Task<IEnumerable<SensorReading>> GetLatestReadingsAsync(string sensorId, int count)
        {
            return await _dbContext.SensorReadings
                .Where(r => r.SensorId == sensorId)
                .OrderByDescending(r => r.Timestamp)
                .Take(count)
                .ToListAsync();
        }

        public async Task DeleteOlderThanAsync(DateTime cutoffDate)
        {
            var oldReadings = await _dbContext.SensorReadings
                .Where(r => r.Timestamp < cutoffDate)
                .ToListAsync();

            if (oldReadings.Any())
            {
                _dbContext.SensorReadings.RemoveRange(oldReadings);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("{Count} sensor readings older than {CutoffDate} deleted successfully",
                    oldReadings.Count, cutoffDate);
            }
        }
    }
}
