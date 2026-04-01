using Microsoft.AspNetCore.Mvc;
using HotelBooking.API.DTOs;
using HotelBooking.API.Services;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public RoomsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDto>> GetRoom(int id)
    {
        var room = await _hotelService.GetRoomAsync(id);
        if (room == null) return NotFound(new { message = "Room not found." });
        return Ok(room);
    }
}
