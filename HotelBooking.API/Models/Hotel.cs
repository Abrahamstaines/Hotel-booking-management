using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models;

public class Hotel
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string City { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string Address { get; set; } = string.Empty;

    [Range(1, 5)]
    public int StarRating { get; set; }

    [MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [MaxLength(100)]
    public string ContactEmail { get; set; } = string.Empty;

    [MaxLength(20)]
    public string ContactPhone { get; set; } = string.Empty;

    public ICollection<RoomCategory> RoomCategories { get; set; } = new List<RoomCategory>();
    public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
}
