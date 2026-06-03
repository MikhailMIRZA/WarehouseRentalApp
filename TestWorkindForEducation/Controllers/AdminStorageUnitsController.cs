using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static Application.DTOs.CreateStorageUnitDto;

namespace TestWorkindForEducation.WebAPI.Controllers.Admin;

[ApiController]
[Route("api/admin/[controller]")]
public class AdminStorageUnitsController : ControllerBase
{
    private readonly IStorageUnitService _storageUnitService;

    public AdminStorageUnitsController(IStorageUnitService storageUnitService)
    {
        _storageUnitService = storageUnitService;
    }

    // GET: api/admin/adminstorageunits
    [HttpGet]
    public async Task<IActionResult> GetAllStorageUnits()
    {
        var units = await _storageUnitService.GetAllStorageUnitsAsync();
        return Ok(units);
    }

    // GET api/admin/adminstorageunits/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStorageUnit(int id)
    {
        var unit = await _storageUnitService.GetStorageUnitByIdAsync(id);
        if (unit == null) return NotFound();
        return Ok(unit);
    }

    // POST api/admin/adminstorageunits
    [HttpPost]
    public async Task<IActionResult> CreateStorageUnit([FromBody] CreateStorageUnitDto createStorageUnitDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var createdUnit = await _storageUnitService.CreateStorageUnitAsync(createStorageUnitDto);
            return CreatedAtAction(nameof(GetStorageUnit), new { id = createdUnit.Id }, createdUnit);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT api/admin/adminstorageunits/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStorageUnit(int id, [FromBody] CreateStorageUnitDto updateDto)
    {
        try
        {
            var isUpdated = await _storageUnitService.UpdateStorageUnitAsync(id, updateDto);
            if (!isUpdated) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE api/admin/adminstorageunits/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStorageUnit(int id)
    {
        var isDeleted = await _storageUnitService.DeleteStorageUnitAsync(id);
        if (!isDeleted) return NotFound();
        return NoContent();
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStorageUnitStatus(int id, [FromBody] UpdateStorageUnitStatusDto statusDto)
    {
        try
        {
            var unit = await _storageUnitService.GetStorageUnitByIdAsync(id);
            if (unit == null) return NotFound();

            var updateDto = new CreateStorageUnitDto
            {
                Name = unit.Name,
                Type = unit.Type,
                Price = unit.Price,
                Description = unit.Description,
                Area = unit.Area,
                IsAvailable = statusDto.IsAvailable
            };

            var isUpdated = await _storageUnitService.UpdateStorageUnitAsync(id, updateDto);
            if (!isUpdated) return NotFound();

            return Ok(new
            {
                message = "Статус помещения успешно обновлен",
                storageUnitId = id,
                isAvailable = statusDto.IsAvailable
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}