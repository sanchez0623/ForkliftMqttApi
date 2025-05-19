using ForkliftMqtt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForkliftMqtt.Domain.Repositories
{
    public interface IForkliftRepository
    {
        Task<Forklift?> GetByIdAsync(string id);
        Task<IEnumerable<Forklift>> GetAllAsync();
        Task AddAsync(Forklift forklift);
        Task UpdateAsync(Forklift forklift);
        Task DeleteAsync(string id);
        Task<bool> ExistsAsync(string id);
    }
}
