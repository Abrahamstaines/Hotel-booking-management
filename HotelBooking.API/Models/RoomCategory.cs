using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.API.Models;

public class RoomCategory
{
    public int Id { get; set; }

    public int HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty; // Standard, Deluxe, Suite

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    public int MaxOccupancy { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal BasePrice { get; set; }

    [MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
}
