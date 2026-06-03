using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StorageUnitService : IStorageUnitService
    {
        private readonly IStorageUnitRepository _storageUnitRepository;

        public StorageUnitService(IStorageUnitRepository storageUnitRepository)
        {
            _storageUnitRepository = storageUnitRepository;
        }

        public async Task<StorageUnitDto?> GetStorageUnitByIdAsync(int id)
        {
            var unit = await _storageUnitRepository.GetByIdAsync(id);
            return unit != null ? MapToDto(unit) : null;
        }

        public async Task<List<StorageUnitDto>> GetAllStorageUnitsAsync()
        {
            var units = await _storageUnitRepository.GetAllAsync();
            return units.Select(MapToDto).ToList();
        }

        public async Task<StorageUnitDto> CreateStorageUnitAsync(CreateStorageUnitDto createDto)
        {
            var unit = new StorageUnit
            {
                Name = createDto.Name,
                Type = createDto.Type,
                Price = createDto.Price,
                Description = createDto.Description,
                Area = createDto.Area,
                IsAvailable = true
            };

            var created = await _storageUnitRepository.AddAsync(unit);
            return MapToDto(created);
        }

        public async Task<bool> UpdateStorageUnitAsync(int id, CreateStorageUnitDto updateDto)
        {
            var unit = await _storageUnitRepository.GetByIdAsync(id);
            if (unit == null) throw new ArgumentException("Помещение не найдено");

            unit.Name = updateDto.Name;
            unit.Type = updateDto.Type;
            unit.Price = updateDto.Price;
            unit.Description = updateDto.Description;
            unit.Area = updateDto.Area;
            unit.IsAvailable = updateDto.IsAvailable;

            await _storageUnitRepository.UpdateAsync(unit);
            return true;
        }

        public async Task<bool> DeleteStorageUnitAsync(int id)
        {
            if (!await _storageUnitRepository.ExistsAsync(id))
                return false;

            await _storageUnitRepository.DeleteAsync(id);
            return true;
        }

        private static StorageUnitDto MapToDto(StorageUnit unit) => new()
        {
            Id = unit.Id,
            Name = unit.Name,
            Type = unit.Type,
            Price = unit.Price,
            Description = unit.Description,
            Area = unit.Area,
            IsAvailable = unit.IsAvailable
        };
    }
}