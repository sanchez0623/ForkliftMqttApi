using ForkliftMqtt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Domain.Repositories
{
    public interface ISensorRepository
    {
        Task<ForkliftSensor?> GetByIdAsync(string id);
        Task<IEnumerable<ForkliftSensor>> GetAllAsync();
        Task<IEnumerable<ForkliftSensor>> GetByForkliftIdAsync(string forkliftId);
        Task AddAsync(ForkliftSensor sensor);
        Task UpdateAsync(ForkliftSensor sensor);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}
