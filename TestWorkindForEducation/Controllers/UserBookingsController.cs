using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;

namespace TestWorkindForEducation.WebAPI.Controllers.User;

[ApiController]
[Route("api/user/[controller]")]
public class UserBookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public UserBookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings(string userId)
    {
        var bookings = await _bookingService.GetUserBookingsAsync(userId);
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking == null) return NotFound();
        return Ok(booking);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto createBookingDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Проверяем доступность помещения (StorageUnit) на указанные даты
        var isAvailable = await _bookingService.IsStorageUnitAvailableAsync(
            createBookingDto.StorageUnitId,
            createBookingDto.StartDate,
            createBookingDto.EndDate
        );

        if (!isAvailable) return BadRequest("Помещение уже забронировано на выбранные даты.");

        var createdBooking = await _bookingService.CreateBookingAsync(createBookingDto, createBookingDto.UserId);
        return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.Id }, createdBooking);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelBooking(int id, string userId)
    {
        try
        {
            await _bookingService.CancelBookingAsync(id, userId);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}