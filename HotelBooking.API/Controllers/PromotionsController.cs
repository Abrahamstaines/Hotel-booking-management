using Microsoft.AspNetCore.Mvc;
using HotelBooking.API.DTOs;
using HotelBooking.API.Services;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromotionsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public PromotionsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("validate")]
    public async Task<ActionResult<PromotionResultDto>> Validate([FromBody] PromotionValidateDto dto)
    {
        var result = await _bookingService.ValidatePromotionAsync(dto.Code, dto.OriginalPrice);
        return Ok(result);
    }
}
