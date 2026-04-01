using Microsoft.EntityFrameworkCore;
using HotelBooking.API.Data;
using HotelBooking.API.DTOs;

namespace HotelBooking.API.Services;

public interface IHotelService
{
    Task<List<HotelListDto>> SearchHotelsAsync(HotelSearchParams searchParams);
    Task<HotelDetailDto?> GetHotelDetailAsync(int id);
    Task<List<RoomCategoryDto>> GetAvailableRoomsAsync(int hotelId, DateTime? checkIn, DateTime? checkOut);
    Task<RoomDto?> GetRoomAsync(int roomId);
}

public class HotelService : IHotelService
{
    private readonly ApplicationDbContext _db;

    public HotelService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<List<HotelListDto>> SearchHotelsAsync(HotelSearchParams p)
    {
        var query = _db.Hotels
            .Include(h => h.RoomCategories)
            .Include(h => h.HotelAmenities).ThenInclude(ha => ha.Amenity)
            .AsQueryable();

        if (!string.IsNullOrEmpty(p.City))
            query = query.Where(h => h.City.ToLower().Contains(p.City.ToLower()));

        if (p.StarRating.HasValue)
            query = query.Where(h => h.StarRating >= p.StarRating.Value);

        if (p.MinPrice.HasValue)
            query = query.Where(h => h.RoomCategories.Any(rc => rc.BasePrice >= p.MinPrice.Value));

        if (p.MaxPrice.HasValue)
            query = query.Where(h => h.RoomCategories.Any(rc => rc.BasePrice <= p.MaxPrice.Value));

        if (!string.IsNullOrEmpty(p.Amenity))
            query = query.Where(h => h.HotelAmenities.Any(ha => ha.Amenity.Name.ToLower().Contains(p.Amenity.ToLower())));

        if (p.Guests.HasValue)
            query = query.Where(h => h.RoomCategories.Any(rc => rc.MaxOccupancy >= p.Guests.Value));

        var hotels = await query.ToListAsync();

        return hotels.Select(h => new HotelListDto
        {
            Id = h.Id,
            Name = h.Name,
            City = h.City,
            StarRating = h.StarRating,
            ImageUrl = h.ImageUrl,
            StartingPrice = h.RoomCategories.Any() ? h.RoomCategories.Min(rc => rc.BasePrice) : 0,
            Amenities = h.HotelAmenities.Select(ha => ha.Amenity.Name).ToList()
        }).ToList();
    }

    public async Task<HotelDetailDto?> GetHotelDetailAsync(int id)
    {
        var hotel = await _db.Hotels
            .Include(h => h.RoomCategories).ThenInclude(rc => rc.Rooms)
            .Include(h => h.HotelAmenities).ThenInclude(ha => ha.Amenity)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hotel == null) return null;

        return new HotelDetailDto
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Description = hotel.Description,
            City = hotel.City,
            Address = hotel.Address,
            StarRating = hotel.StarRating,
            ImageUrl = hotel.ImageUrl,
            ContactEmail = hotel.ContactEmail,
            ContactPhone = hotel.ContactPhone,
            Amenities = hotel.HotelAmenities.Select(ha => new AmenityDto
            {
                Name = ha.Amenity.Name,
                Icon = ha.Amenity.Icon
            }).ToList(),
            RoomCategories = hotel.RoomCategories.Select(rc => new RoomCategoryDto
            {
                Id = rc.Id,
                Name = rc.Name,
                Description = rc.Description,
                MaxOccupancy = rc.MaxOccupancy,
                BasePrice = rc.BasePrice,
                ImageUrl = rc.ImageUrl,
                AvailableRooms = rc.Rooms.Count(r => r.IsAvailable)
            }).ToList()
        };
    }

    public async Task<List<RoomCategoryDto>> GetAvailableRoomsAsync(int hotelId, DateTime? checkIn, DateTime? checkOut)
    {
        var query = _db.RoomCategories
            .Include(rc => rc.Rooms).ThenInclude(r => r.Bookings)
            .Where(rc => rc.HotelId == hotelId);

        var categories = await query.ToListAsync();

        return categories.Select(rc => new RoomCategoryDto
        {
            Id = rc.Id,
            Name = rc.Name,
            Description = rc.Description,
            MaxOccupancy = rc.MaxOccupancy,
            BasePrice = rc.BasePrice,
            ImageUrl = rc.ImageUrl,
            AvailableRooms = rc.Rooms.Count(r => IsRoomAvailable(r, checkIn, checkOut))
        }).Where(rc => rc.AvailableRooms > 0).ToList();
    }

    private static bool IsRoomAvailable(Models.Room room, DateTime? checkIn, DateTime? checkOut)
    {
        if (!room.IsAvailable) return false;
        if (!checkIn.HasValue || !checkOut.HasValue) return true;

        return !room.Bookings.Any(b =>
            b.Status == Models.BookingStatus.Confirmed &&
            b.CheckIn < checkOut.Value &&
            b.CheckOut > checkIn.Value);
    }

    public async Task<RoomDto?> GetRoomAsync(int roomId)
    {
        var room = await _db.Rooms
            .Include(r => r.RoomCategory).ThenInclude(rc => rc.Hotel)
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room == null) return null;

        return new RoomDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            Floor = room.Floor,
            IsAvailable = room.IsAvailable,
            CategoryName = room.RoomCategory.Name,
            Price = room.RoomCategory.BasePrice,
            HotelName = room.RoomCategory.Hotel.Name
        };
    }
}
