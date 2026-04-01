using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBooking.API.DTOs;
using HotelBooking.API.Services;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost]
    public async Task<ActionResult<BookingResponseDto>> Create([FromBody] CreateBookingDto dto)
    {
        try
        {
            var result = await _bookingService.CreateBookingAsync(GetUserId(), dto);
            return CreatedAtAction(nameof(GetUserBookings), result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<BookingResponseDto>>> GetUserBookings()
    {
        var bookings = await _bookingService.GetUserBookingsAsync(GetUserId());
        return Ok(bookings);
    }

    [HttpPut("{id}/cancel")]
    public async Task<ActionResult<BookingResponseDto>> Cancel(int id)
    {
        try
        {
            var result = await _bookingService.CancelBookingAsync(id, GetUserId());
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/rebook")]
    public async Task<ActionResult<BookingResponseDto>> Rebook(int id, [FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut)
    {
        try
        {
            var result = await _bookingService.RebookAsync(id, GetUserId(), checkIn, checkOut);
            return CreatedAtAction(nameof(GetUserBookings), result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
