using System.Net.Http.Json;
using TestWorkindForEducation.Models;

namespace TestWorkindForEducation.Services
{
    public class StorageApiService
    {
        private readonly HttpClient _httpClient;

        public StorageApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:5096/");
        }

        // 1. ПОЛУЧЕНИЕ ВСЕХ ПОМЕЩЕНИЙ
        public async Task<List<FrontendStorageUnit>> GetStorageUnitsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<FrontendStorageUnit>>("api/admin/AdminStorageUnits");
                return response ?? GetTestStorageUnits();
            }
            catch
            {
                return GetTestStorageUnits();
            }
        }

        // 2. ДОБАВЛЕНИЕ ПОМЕЩЕНИЯ
        public async Task<bool> AddStorageUnitAsync(FrontendStorageUnit unit)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/admin/AdminStorageUnits", unit);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // 3. ОБНОВЛЕНИЕ ПОМЕЩЕНИЯ
        public async Task<bool> UpdateStorageUnitAsync(int id, FrontendStorageUnit unit)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/admin/AdminStorageUnits/{id}", unit);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // 4. УДАЛЕНИЕ ПОМЕЩЕНИЯ
        public async Task<bool> DeleteStorageUnitAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/admin/AdminStorageUnits/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // 5. ПОЛУЧЕНИЕ БРОНИРОВАНИЙ
        public async Task<List<FrontendBooking>> GetUserBookingsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<FrontendBooking>>("api/user/UserBookings/my");
                return response ?? GetTestBookings();
            }
            catch
            {
                return GetTestBookings();
            }
        }

        // ТЕСТОВЫЕ ДАННЫЕ (склады)
        private List<FrontendStorageUnit> GetTestStorageUnits()
        {
            return new List<FrontendStorageUnit>
            {
                new() { Id = 1, Name = "Бокс-1", Type = "Сухой", Price = 2500,
                        Description = "Небольшой сухой склад", Area = 30, IsAvailable = true },
                new() { Id = 2, Name = "Холод-2", Type = "Холодильный", Price = 4000,
                        Description = "Холодильная камера -18°C", Area = 25, IsAvailable = true }
            };
        }

        private List<FrontendBooking> GetTestBookings()
        {
            return new List<FrontendBooking>
            {
                new() { Id = 1, StorageUnitId = 1, StorageUnitName = "Бокс-1", StorageUnitType = "Сухой",
                        UserName = "Иван Иванов", TotalPrice = 12500,
                        StartDate = DateTime.Now.AddDays(7), EndDate = DateTime.Now.AddDays(10),
                        Status = "Confirmed" }
            };
        }
    }
}
