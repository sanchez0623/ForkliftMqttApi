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
    public class ForkliftRepository : IForkliftRepository
    {
        private readonly ForkliftDbContext _dbContext;
        private readonly ILogger<ForkliftRepository> _logger;

        public ForkliftRepository(
            ForkliftDbContext dbContext,
            ILogger<ForkliftRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Forklift?> GetByIdAsync(string id)
        {
            return await _dbContext.Forklifts
                .Include(f => f.Sensors)  // 如果Forklift实体中有Sensors导航属性
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<Forklift>> GetAllAsync()
        {
            return await _dbContext.Forklifts.ToListAsync();
        }

        public async Task AddAsync(Forklift forklift)
        {
            await _dbContext.Forklifts.AddAsync(forklift);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Forklift {ForkliftId} added successfully", forklift.Id);
        }

        public async Task UpdateAsync(Forklift forklift)
        {
            var existingForklift = await _dbContext.Forklifts.FindAsync(forklift.Id);
            if (existingForklift == null)
            {
                throw new DomainException($"Forklift with ID {forklift.Id} not found");
            }

            _dbContext.Entry(existingForklift).CurrentValues.SetValues(forklift);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Forklift {ForkliftId} updated successfully", forklift.Id);
        }

        public async Task DeleteAsync(string id)
        {
            var forklift = await _dbContext.Forklifts.FindAsync(id);
            if (forklift == null)
            {
                throw new DomainException($"Forklift with ID {id} not found");
            }

            _dbContext.Forklifts.Remove(forklift);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Forklift {ForkliftId} deleted successfully", id);
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _dbContext.Forklifts.AnyAsync(f => f.Id == id);
        }
    }
}
