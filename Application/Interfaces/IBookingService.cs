using Application.DTOs;


namespace Application.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDto?> GetBookingByIdAsync(int id);
        Task<List<BookingDto>> GetUserBookingsAsync(string userId);
        Task<List<BookingDto>> GetAllBookingsAsync();
        Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto, string userId);
        Task CancelBookingAsync(int id, string userId);
        Task<bool> IsStorageUnitAvailableAsync(int storageUnitId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}