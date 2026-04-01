using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models;

public class Room
{
    public int Id { get; set; }

    public int RoomCategoryId { get; set; }
    public RoomCategory RoomCategory { get; set; } = null!;

    [Required, MaxLength(10)]
    public string RoomNumber { get; set; } = string.Empty;

    public int Floor { get; set; }

    public bool IsAvailable { get; set; } = true;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
