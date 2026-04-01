using Microsoft.AspNetCore.Mvc;
using HotelBooking.API.DTOs;
using HotelBooking.API.Services;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController : ControllerBase
{
    private readonly IHotelService _hotelService;

    public HotelsController(IHotelService hotelService)
    {
        _hotelService = hotelService;
    }

    [HttpGet]
    public async Task<ActionResult<List<HotelListDto>>> Search([FromQuery] HotelSearchParams searchParams)
    {
        var hotels = await _hotelService.SearchHotelsAsync(searchParams);
        return Ok(hotels);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HotelDetailDto>> GetDetail(int id)
    {
        var hotel = await _hotelService.GetHotelDetailAsync(id);
        if (hotel == null) return NotFound(new { message = "Hotel not found." });
        return Ok(hotel);
    }

    [HttpGet("{id}/rooms")]
    public async Task<ActionResult<List<RoomCategoryDto>>> GetAvailableRooms(
        int id, [FromQuery] DateTime? checkIn, [FromQuery] DateTime? checkOut)
    {
        var rooms = await _hotelService.GetAvailableRoomsAsync(id, checkIn, checkOut);
        return Ok(rooms);
    }
}
