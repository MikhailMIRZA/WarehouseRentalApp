using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Infrastructure.Repositories;

namespace Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IStorageUnitRepository _storageUnitRepository;
        private readonly HttpClient _httpClient;

        public BookingService(IBookingRepository bookingRepository, IStorageUnitRepository storageUnitRepository, HttpClient httpClient)
        {
            _bookingRepository = bookingRepository;
            _storageUnitRepository = storageUnitRepository;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5096/");
        }

        public async Task<BookingDto?> GetBookingByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            return booking != null ? MapToDto(booking) : null;
        }

        public async Task<List<BookingDto>> GetUserBookingsAsync(string userId)
        {
            var bookings = await _bookingRepository.GetUserBookingsAsync(userId);
            return bookings.Select(MapToDto).ToList();
        }

        public async Task<List<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            return bookings.Select(MapToDto).ToList();
        }

        public async Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto, string userId)
        {
            try
            {
                Console.WriteLine("=== НАЧАЛО СОЗДАНИЯ БРОНИРОВАНИЯ ===");
                Console.WriteLine($"StorageUnitId: {createBookingDto.StorageUnitId}, User: {createBookingDto.UserName}");

                var storageUnit = await _storageUnitRepository.GetByIdAsync(createBookingDto.StorageUnitId);
                if (storageUnit == null) throw new ArgumentException("Помещение не найдено");

                if (!storageUnit.IsAvailable) throw new InvalidOperationException("Помещение не доступно");

                if (!await IsStorageUnitAvailableAsync(createBookingDto.StorageUnitId, createBookingDto.StartDate, createBookingDto.EndDate))
                    throw new InvalidOperationException("Помещение уже забронировано на указанные даты");

                if (createBookingDto.StartDate >= createBookingDto.EndDate)
                    throw new ArgumentException("Дата окончания должна быть после даты начала");

                var booking = new Booking
                {
                    StorageUnitId = createBookingDto.StorageUnitId,
                    UserId = userId,
                    UserName = createBookingDto.UserName,
                    UserEmail = createBookingDto.UserEmail,
                    StartDate = createBookingDto.StartDate,
                    EndDate = createBookingDto.EndDate,
                    CreatedAt = DateTimeOffset.UtcNow,
                    Status = BookingStatus.Confirmed
                };

                var createdBooking = await _bookingRepository.AddAsync(booking);
                Console.WriteLine($"Бронирование сохранено. ID: {createdBooking.Id}");

                await UpdateStorageUnitStatusAsync(createdBooking.StorageUnitId, false);

                return MapToDto(createdBooking);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА в CreateBookingAsync: {ex.Message}");
                throw;
            }
        }

        public async Task CancelBookingAsync(int id, string userId)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null) throw new ArgumentException("Бронь не найдена");
            if (booking.UserId != userId) throw new UnauthorizedAccessException("Доступ запрещен");

            booking.Status = BookingStatus.Cancelled;
            await _bookingRepository.UpdateAsync(booking);

            await UpdateStorageUnitStatusAsync(booking.StorageUnitId, true);
        }

        public async Task<bool> IsStorageUnitAvailableAsync(int storageUnitId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return await _bookingRepository.IsStorageUnitAvailableAsync(storageUnitId, startDate, endDate);
        }

        private async Task UpdateStorageUnitStatusAsync(int storageUnitId, bool isAvailable)
        {
            try
            {
                Console.WriteLine($"Обновление статуса помещения {storageUnitId} на {(isAvailable ? "Свободно" : "Занято")}");

                var response = await _httpClient.PatchAsJsonAsync(
                    $"api/admin/AdminStorageUnits/{storageUnitId}/status",
                    new { isAvailable });

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Статус помещения {storageUnitId} обновлен через API");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API ошибка: {error}");
                    await UpdateStorageUnitStatusDirectlyAsync(storageUnitId, isAvailable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HTTP ошибка: {ex.Message}");
                await UpdateStorageUnitStatusDirectlyAsync(storageUnitId, isAvailable);
            }
        }

        private async Task UpdateStorageUnitStatusDirectlyAsync(int storageUnitId, bool isAvailable)
        {
            try
            {
                var unit = await _storageUnitRepository.GetByIdAsync(storageUnitId);
                if (unit != null)
                {
                    unit.IsAvailable = isAvailable;
                    Console.WriteLine($"Статус помещения {storageUnitId} обновлен напрямую");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка прямого обновления: {ex.Message}");
            }
        }

        private static BookingDto MapToDto(Booking booking) => new BookingDto
        {
            Id = booking.Id,
            StorageUnitId = booking.StorageUnitId,
            StorageUnitName = booking.StorageUnit?.Name ?? string.Empty,
            UserId = booking.UserId,
            UserName = booking.UserName,
            UserEmail = booking.UserEmail,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            CreatedAt = booking.CreatedAt,
            Status = booking.Status,
            StorageUnit = booking.StorageUnit != null ? new StorageUnitDto
            {
                Id = booking.StorageUnit.Id,
                Name = booking.StorageUnit.Name,
                Type = booking.StorageUnit.Type,
                Price = booking.StorageUnit.Price,
                Description = booking.StorageUnit.Description,
                Area = booking.StorageUnit.Area,
                IsAvailable = booking.StorageUnit.IsAvailable
            } : null
        };
    }
}