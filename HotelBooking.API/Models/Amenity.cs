using System.ComponentModel.DataAnnotations;

namespace HotelBooking.API.Models;

public class Amenity
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Icon { get; set; } = string.Empty;

    public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
}

public class HotelAmenity
{
    public int HotelId { get; set; }
    public Hotel Hotel { get; set; } = null!;

    public int AmenityId { get; set; }
    public Amenity Amenity { get; set; } = null!;
}
