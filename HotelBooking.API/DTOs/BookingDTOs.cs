using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.DTOs;

public class CreateBookingDto
{
    [Required]
    public int RoomId { get; set; }

    [Required]
    public DateTime CheckIn { get; set; }

    [Required]
    public DateTime CheckOut { get; set; }

    public string? PromotionCode { get; set; }
}

public class BookingResponseDto
{
    public int Id { get; set; }
    public string BookingReference { get; set; } = string.Empty;
    public string HotelName { get; set; } = string.Empty;
    public string RoomCategory { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? PromotionCode { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class PromotionValidateDto
{
    [Required]
    public string Code { get; set; } = string.Empty;
    public decimal OriginalPrice { get; set; }
}

public class PromotionResultDto
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalPrice { get; set; }
}
