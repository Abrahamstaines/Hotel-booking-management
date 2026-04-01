using Microsoft.EntityFrameworkCore;
using HotelBooking.API.Data;
using HotelBooking.API.DTOs;
using HotelBooking.API.Models;

namespace HotelBooking.API.Services;

public interface IBookingService
{
    Task<BookingResponseDto> CreateBookingAsync(string userId, CreateBookingDto dto);
    Task<List<BookingResponseDto>> GetUserBookingsAsync(string userId);
    Task<BookingResponseDto> CancelBookingAsync(int bookingId, string userId);
    Task<BookingResponseDto> RebookAsync(int bookingId, string userId, DateTime checkIn, DateTime checkOut);
    Task<PromotionResultDto> ValidatePromotionAsync(string code, decimal originalPrice);
}

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _db;
    private readonly IEmailService _emailService;

    public BookingService(ApplicationDbContext db, IEmailService emailService)
    {
        _db = db;
        _emailService = emailService;
    }

    public async Task<BookingResponseDto> CreateBookingAsync(string userId, CreateBookingDto dto)
    {
        if (dto.CheckIn >= dto.CheckOut)
            throw new InvalidOperationException("Check-out date must be after check-in date.");

        if (dto.CheckIn.Date < DateTime.UtcNow.Date)
            throw new InvalidOperationException("Check-in date cannot be in the past.");

        var room = await _db.Rooms
            .Include(r => r.RoomCategory).ThenInclude(rc => rc.Hotel)
            .Include(r => r.Bookings)
            .FirstOrDefaultAsync(r => r.Id == dto.RoomId);

        if (room == null)
            throw new InvalidOperationException("Room not found.");

        // Check availability
        var hasConflict = room.Bookings.Any(b =>
            b.Status == BookingStatus.Confirmed &&
            b.CheckIn < dto.CheckOut &&
            b.CheckOut > dto.CheckIn);

        if (hasConflict)
            throw new InvalidOperationException("Room is not available for the selected dates.");

        var nights = (dto.CheckOut - dto.CheckIn).Days;
        var totalPrice = room.RoomCategory.BasePrice * nights;
        decimal discountAmount = 0;

        // Apply promotion
        if (!string.IsNullOrEmpty(dto.PromotionCode))
        {
            var promo = await _db.Promotions.FirstOrDefaultAsync(p =>
                p.Code == dto.PromotionCode && p.IsActive &&
                p.ValidFrom <= DateTime.UtcNow && p.ValidTo >= DateTime.UtcNow);

            if (promo != null)
            {
                discountAmount = totalPrice * promo.DiscountPercent / 100;
                totalPrice -= discountAmount;
            }
        }

        var booking = new Booking
        {
            UserId = userId,
            RoomId = dto.RoomId,
            CheckIn = dto.CheckIn,
            CheckOut = dto.CheckOut,
            TotalPrice = totalPrice,
            DiscountAmount = discountAmount,
            PromotionCode = dto.PromotionCode,
            Status = BookingStatus.Confirmed,
            BookingReference = GenerateBookingReference()
        };

        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();

        // Send confirmation email
        var user = await _db.Users.FindAsync(userId);
        if (user != null)
        {
            await _emailService.SendBookingConfirmationAsync(
                user.Email!,
                user.FullName,
                booking.BookingReference,
                room.RoomCategory.Hotel.Name,
                room.RoomCategory.Name,
                room.RoomNumber,
                booking.CheckIn,
                booking.CheckOut,
                booking.TotalPrice
            );
        }

        return MapToDto(booking, room);
    }

    public async Task<List<BookingResponseDto>> GetUserBookingsAsync(string userId)
    {
        var bookings = await _db.Bookings
            .Include(b => b.Room).ThenInclude(r => r.RoomCategory).ThenInclude(rc => rc.Hotel)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return bookings.Select(b => MapToDto(b, b.Room)).ToList();
    }

    public async Task<BookingResponseDto> CancelBookingAsync(int bookingId, string userId)
    {
        var booking = await _db.Bookings
            .Include(b => b.Room).ThenInclude(r => r.RoomCategory).ThenInclude(rc => rc.Hotel)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

        if (booking == null)
            throw new InvalidOperationException("Booking not found.");

        if (booking.Status == BookingStatus.Cancelled)
            throw new InvalidOperationException("Booking is already cancelled.");

        booking.Status = BookingStatus.Cancelled;
        await _db.SaveChangesAsync();

        return MapToDto(booking, booking.Room);
    }

    public async Task<BookingResponseDto> RebookAsync(int bookingId, string userId, DateTime checkIn, DateTime checkOut)
    {
        var originalBooking = await _db.Bookings
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

        if (originalBooking == null)
            throw new InvalidOperationException("Original booking not found.");

        var newBookingDto = new CreateBookingDto
        {
            RoomId = originalBooking.RoomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            PromotionCode = originalBooking.PromotionCode
        };

        return await CreateBookingAsync(userId, newBookingDto);
    }

    public async Task<PromotionResultDto> ValidatePromotionAsync(string code, decimal originalPrice)
    {
        var promo = await _db.Promotions.FirstOrDefaultAsync(p =>
            p.Code == code.ToUpper() && p.IsActive &&
            p.ValidFrom <= DateTime.UtcNow && p.ValidTo >= DateTime.UtcNow);

        if (promo == null)
        {
            return new PromotionResultDto
            {
                IsValid = false,
                Message = "Invalid or expired promotion code."
            };
        }

        var discount = originalPrice * promo.DiscountPercent / 100;

        return new PromotionResultDto
        {
            IsValid = true,
            Message = promo.Description,
            DiscountPercent = promo.DiscountPercent,
            DiscountAmount = discount,
            FinalPrice = originalPrice - discount
        };
    }

    private static string GenerateBookingReference()
    {
        return $"HB-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..6].ToUpper()}";
    }

    private static BookingResponseDto MapToDto(Booking booking, Room room)
    {
        return new BookingResponseDto
        {
            Id = booking.Id,
            BookingReference = booking.BookingReference,
            HotelName = room.RoomCategory.Hotel.Name,
            RoomCategory = room.RoomCategory.Name,
            RoomNumber = room.RoomNumber,
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            TotalPrice = booking.TotalPrice,
            DiscountAmount = booking.DiscountAmount,
            PromotionCode = booking.PromotionCode,
            Status = booking.Status.ToString(),
            CreatedAt = booking.CreatedAt
        };
    }
}
