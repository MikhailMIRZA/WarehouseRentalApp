using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int id);
        Task<List<Booking>> GetUserBookingsAsync(string userId);
        Task<List<Booking>> GetAllAsync();
        Task<Booking> AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
        Task<bool> IsStorageUnitAvailableAsync(int storageUnitId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}