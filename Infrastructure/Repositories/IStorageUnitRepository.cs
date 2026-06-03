using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public interface IStorageUnitRepository
    {
        Task<StorageUnit?> GetByIdAsync(int id);
        Task<List<StorageUnit>> GetAllAsync();
        Task<StorageUnit> AddAsync(StorageUnit unit);
        Task UpdateAsync(StorageUnit unit);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}