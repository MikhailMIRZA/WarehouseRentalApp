using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IStorageUnitService
    {
        Task<List<StorageUnitDto>> GetAllStorageUnitsAsync();
        Task<StorageUnitDto?> GetStorageUnitByIdAsync(int id);
        Task<StorageUnitDto> CreateStorageUnitAsync(CreateStorageUnitDto createStorageUnitDto);
        Task<bool> UpdateStorageUnitAsync(int id, CreateStorageUnitDto updateStorageUnitDto);
        Task<bool> DeleteStorageUnitAsync(int id);
    }
}