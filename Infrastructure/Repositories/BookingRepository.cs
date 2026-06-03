using Domain.Entities;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.StorageUnit)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<Booking>> GetUserBookingsAsync(string userId)
        {
            return await _context.Bookings
                .Include(b => b.StorageUnit)
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            return await _context.Bookings
                .Include(b => b.StorageUnit)
                .ToListAsync();
        }

        public async Task<Booking> AddAsync(Booking booking)
        {
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsStorageUnitAvailableAsync(int storageUnitId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (startDate >= endDate) return false;
            try
            {
                var hasConflict = await _context.Bookings
                    .Where(b => b.StorageUnitId == storageUnitId)
                    .Where(b => b.Status != BookingStatus.Cancelled)
                    .Where(b => b.Status != BookingStatus.Completed)
                    .AnyAsync(b => startDate < b.EndDate && endDate > b.StartDate);
                return !hasConflict;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка проверки доступности помещения {storageUnitId}: {ex.Message}");
                return false;
            }
        }

        // Оставшийся метод GetRoomBookingsAsync можно удалить или переименовать при необходимости
        public async Task<List<Booking>> GetStorageUnitBookingsAsync(int storageUnitId)
        {
            return await _context.Bookings
                .Where(b => b.StorageUnitId == storageUnitId)
                .ToListAsync();
        }
    }
}