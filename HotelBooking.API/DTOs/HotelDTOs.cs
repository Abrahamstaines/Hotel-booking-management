namespace HotelBooking.API.DTOs;

public class HotelListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public int StarRating { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public decimal StartingPrice { get; set; }
    public List<string> Amenities { get; set; } = new();
}

public class HotelDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int StarRating { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public List<AmenityDto> Amenities { get; set; } = new();
    public List<RoomCategoryDto> RoomCategories { get; set; } = new();
}

public class AmenityDto
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}

public class RoomCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int MaxOccupancy { get; set; }
    public decimal BasePrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int AvailableRooms { get; set; }
}

public class RoomDto
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public bool IsAvailable { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string HotelName { get; set; } = string.Empty;
}

public class HotelSearchParams
{
    public string? City { get; set; }
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? StarRating { get; set; }
    public int? Guests { get; set; }
    public string? Amenity { get; set; }
}
