using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.API.Models;

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    Completed
}

public class Booking
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;

    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    [Required, MaxLength(20)]
    public string BookingReference { get; set; } = string.Empty;

    public string? PromotionCode { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal DiscountAmount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
