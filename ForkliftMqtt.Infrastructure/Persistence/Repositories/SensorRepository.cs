using ForkliftMqtt.Domain.Entities;
using ForkliftMqtt.Domain.Exceptions;
using ForkliftMqtt.Domain.Repositories;
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
    public class SensorRepository : ISensorRepository
    {
        private readonly ForkliftDbContext _dbContext;
        private readonly ILogger<SensorRepository> _logger;

        public SensorRepository(
            ForkliftDbContext dbContext,
            ILogger<SensorRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ForkliftSensor?> GetByIdAsync(string id)
        {
            return await _dbContext.Sensors
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<ForkliftSensor>> GetAllAsync()
        {
            return await _dbContext.Sensors
                .ToListAsync();
        }

        public async Task<IEnumerable<ForkliftSensor>> GetByForkliftIdAsync(string forkliftId)
        {
            return await _dbContext.Sensors
                .Where(s => s.ForkliftId == forkliftId)
                .ToListAsync();
        }

        public async Task AddAsync(ForkliftSensor sensor)
        {
            await _dbContext.Sensors.AddAsync(sensor);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Sensor {SensorId} added successfully", sensor.Id);
        }

        public async Task UpdateAsync(ForkliftSensor sensor)
        {
            var existingSensor = await _dbContext.Sensors.FindAsync(sensor.Id);
            if (existingSensor == null)
            {
                throw new SensorNotFoundException($"Sensor with ID {sensor.Id} not found");
            }

            _dbContext.Entry(existingSensor).CurrentValues.SetValues(sensor);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Sensor {SensorId} updated successfully", sensor.Id);
        }

        public async Task DeleteAsync(string id)
        {
            var sensor = await _dbContext.Sensors.FindAsync(id);
            if (sensor == null)
            {
                throw new SensorNotFoundException($"Sensor with ID {id} not found");
            }

            _dbContext.Sensors.Remove(sensor);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Sensor {SensorId} deleted successfully", id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _dbContext.Sensors.AnyAsync(s => s.Id == id);
        }
    }
}
