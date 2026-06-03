using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Infrastructure.Repositories
{
    public class StorageUnitRepository : IStorageUnitRepository
    {
        private readonly ApplicationDbContext _context;

        public StorageUnitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<StorageUnit?> GetByIdAsync(int id) =>
            await _context.StorageUnits.FindAsync(id);

        public async Task<List<StorageUnit>> GetAllAsync() =>
            await _context.StorageUnits.ToListAsync();

        public async Task<StorageUnit> AddAsync(StorageUnit unit)
        {
            _context.StorageUnits.Add(unit);
            await _context.SaveChangesAsync();
            return unit;
        }

        public async Task UpdateAsync(StorageUnit unit)
        {
            _context.StorageUnits.Update(unit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var unit = await GetByIdAsync(id);
            if (unit != null)
            {
                _context.StorageUnits.Remove(unit);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.StorageUnits.AnyAsync(r => r.Id == id);
    }
}