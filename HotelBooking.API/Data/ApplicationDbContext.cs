using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HotelBooking.API.Models;

namespace HotelBooking.API.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Hotel> Hotels => Set<Hotel>();
    public DbSet<RoomCategory> RoomCategories => Set<RoomCategory>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<HotelAmenity> HotelAmenities => Set<HotelAmenity>();
    public DbSet<Promotion> Promotions => Set<Promotion>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // HotelAmenity composite key
        builder.Entity<HotelAmenity>()
            .HasKey(ha => new { ha.HotelId, ha.AmenityId });

        builder.Entity<HotelAmenity>()
            .HasOne(ha => ha.Hotel)
            .WithMany(h => h.HotelAmenities)
            .HasForeignKey(ha => ha.HotelId);

        builder.Entity<HotelAmenity>()
            .HasOne(ha => ha.Amenity)
            .WithMany(a => a.HotelAmenities)
            .HasForeignKey(ha => ha.AmenityId);

        // Hotel -> RoomCategories
        builder.Entity<RoomCategory>()
            .HasOne(rc => rc.Hotel)
            .WithMany(h => h.RoomCategories)
            .HasForeignKey(rc => rc.HotelId);

        // RoomCategory -> Rooms
        builder.Entity<Room>()
            .HasOne(r => r.RoomCategory)
            .WithMany(rc => rc.Rooms)
            .HasForeignKey(r => r.RoomCategoryId);

        // Booking relationships
        builder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId);

        builder.Entity<Booking>()
            .HasOne(b => b.Room)
            .WithMany(r => r.Bookings)
            .HasForeignKey(b => b.RoomId);

        // Unique constraint on promotion code
        builder.Entity<Promotion>()
            .HasIndex(p => p.Code)
            .IsUnique();

        // Unique constraint on booking reference
        builder.Entity<Booking>()
            .HasIndex(b => b.BookingReference)
            .IsUnique();

        // Seed data
        SeedData(builder);
    }

    private static void SeedData(ModelBuilder builder)
    {
        // Amenities
        builder.Entity<Amenity>().HasData(
            new Amenity { Id = 1, Name = "Free WiFi", Icon = "wifi" },
            new Amenity { Id = 2, Name = "Swimming Pool", Icon = "pool" },
            new Amenity { Id = 3, Name = "Gym", Icon = "fitness_center" },
            new Amenity { Id = 4, Name = "Parking", Icon = "local_parking" },
            new Amenity { Id = 5, Name = "Restaurant", Icon = "restaurant" },
            new Amenity { Id = 6, Name = "Spa", Icon = "spa" },
            new Amenity { Id = 7, Name = "Room Service", Icon = "room_service" },
            new Amenity { Id = 8, Name = "Bar", Icon = "local_bar" },
            new Amenity { Id = 9, Name = "Airport Shuttle", Icon = "airport_shuttle" },
            new Amenity { Id = 10, Name = "Pet Friendly", Icon = "pets" }
        );

        // Hotels
        builder.Entity<Hotel>().HasData(
            new Hotel { Id = 1, Name = "Grand Palace Hotel", Description = "Luxury 5-star hotel in the heart of New York City with stunning skyline views.", City = "New York", Address = "123 Fifth Avenue, Manhattan, NY 10001", StarRating = 5, ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945", ContactEmail = "info@grandpalace.com", ContactPhone = "+1-212-555-0100" },
            new Hotel { Id = 2, Name = "Seaside Resort & Spa", Description = "Beautiful beachfront resort with world-class spa facilities and ocean views.", City = "Miami", Address = "456 Ocean Drive, Miami Beach, FL 33139", StarRating = 4, ImageUrl = "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4", ContactEmail = "reservations@seasideresort.com", ContactPhone = "+1-305-555-0200" },
            new Hotel { Id = 3, Name = "Mountain View Lodge", Description = "Cozy mountain retreat surrounded by nature, perfect for adventure seekers.", City = "Denver", Address = "789 Alpine Road, Denver, CO 80201", StarRating = 3, ImageUrl = "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa", ContactEmail = "stay@mountainviewlodge.com", ContactPhone = "+1-303-555-0300" },
            new Hotel { Id = 4, Name = "The Urban Boutique", Description = "Trendy boutique hotel in downtown San Francisco with modern design.", City = "San Francisco", Address = "321 Market Street, San Francisco, CA 94105", StarRating = 4, ImageUrl = "https://images.unsplash.com/photo-1564501049412-61c2a3083791", ContactEmail = "hello@urbanboutique.com", ContactPhone = "+1-415-555-0400" },
            new Hotel { Id = 5, Name = "Historic Garden Inn", Description = "Charming historic inn with beautiful gardens in the heart of Charleston.", City = "Charleston", Address = "567 King Street, Charleston, SC 29401", StarRating = 3, ImageUrl = "https://images.unsplash.com/photo-1445019980597-93fa8acb246c", ContactEmail = "info@historicgardeninn.com", ContactPhone = "+1-843-555-0500" },
            new Hotel { Id = 6, Name = "Skyline Tower Hotel", Description = "Modern high-rise hotel with panoramic city views and rooftop bar.", City = "Chicago", Address = "890 Michigan Avenue, Chicago, IL 60611", StarRating = 5, ImageUrl = "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb", ContactEmail = "concierge@skylinetower.com", ContactPhone = "+1-312-555-0600" },
            new Hotel { Id = 7, Name = "Desert Oasis Resort", Description = "Luxurious desert retreat with golf course and infinity pool.", City = "Phoenix", Address = "234 Cactus Lane, Scottsdale, AZ 85251", StarRating = 4, ImageUrl = "https://images.unsplash.com/photo-1571896349842-33c89424de2d", ContactEmail = "reservations@desertoasis.com", ContactPhone = "+1-480-555-0700" },
            new Hotel { Id = 8, Name = "Lakefront Suites", Description = "Peaceful lakeside hotel perfect for families and romantic getaways.", City = "Seattle", Address = "678 Lakeshore Blvd, Seattle, WA 98101", StarRating = 3, ImageUrl = "https://images.unsplash.com/photo-1582719508461-905c673771fd", ContactEmail = "info@lakefrontsuites.com", ContactPhone = "+1-206-555-0800" },
            // Indian Hotels
            new Hotel { Id = 9, Name = "The Royal Heritage", Description = "Luxurious heritage hotel in the heart of Ahmedabad with traditional Gujarati architecture and modern amenities.", City = "Ahmedabad", Address = "12 CG Road, Navrangpura, Ahmedabad, Gujarat 380009", StarRating = 5, ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945", ContactEmail = "info@royalheritage.in", ContactPhone = "+91-79-2655-0100" },
            new Hotel { Id = 10, Name = "Taj Gateway Mumbai", Description = "Iconic waterfront hotel overlooking the Arabian Sea with world-class dining.", City = "Mumbai", Address = "Apollo Bunder, Colaba, Mumbai, Maharashtra 400001", StarRating = 5, ImageUrl = "https://images.unsplash.com/photo-1542314831-068cd1dbfeeb", ContactEmail = "reservations@tajmumbai.in", ContactPhone = "+91-22-6665-0200" },
            new Hotel { Id = 11, Name = "Delhi Imperial Palace", Description = "Elegant 5-star hotel near India Gate with Mughal-inspired interiors.", City = "Delhi", Address = "1 Janpath, Connaught Place, New Delhi 110001", StarRating = 5, ImageUrl = "https://images.unsplash.com/photo-1564501049412-61c2a3083791", ContactEmail = "stay@delhiimperial.in", ContactPhone = "+91-11-2334-0300" },
            new Hotel { Id = 12, Name = "Pink City Resort", Description = "Beautiful resort near Hawa Mahal with rooftop pool and Rajasthani cuisine.", City = "Jaipur", Address = "45 MI Road, Jaipur, Rajasthan 302001", StarRating = 4, ImageUrl = "https://images.unsplash.com/photo-1520250497591-112f2f40a3f4", ContactEmail = "info@pinkcityresort.in", ContactPhone = "+91-141-2360-0400" },
            new Hotel { Id = 13, Name = "Goa Beach Paradise", Description = "Tropical beachfront resort on Calangute Beach with infinity pool and spa.", City = "Goa", Address = "Calangute Beach Road, Bardez, Goa 403516", StarRating = 4, ImageUrl = "https://images.unsplash.com/photo-1571896349842-33c89424de2d", ContactEmail = "hello@goabeachparadise.in", ContactPhone = "+91-832-2276-0500" },
            new Hotel { Id = 14, Name = "Bangalore Tech Suites", Description = "Modern business hotel in the IT corridor with co-working spaces and rooftop bar.", City = "Bangalore", Address = "100 Outer Ring Road, Whitefield, Bangalore, Karnataka 560066", StarRating = 4, ImageUrl = "https://images.unsplash.com/photo-1551882547-ff40c63fe5fa", ContactEmail = "stay@blrtechsuites.in", ContactPhone = "+91-80-4455-0600" }
        );

        // Room Categories
        builder.Entity<RoomCategory>().HasData(
            // Grand Palace Hotel
            new RoomCategory { Id = 1, HotelId = 1, Name = "Standard Room", Description = "Comfortable room with city views", MaxOccupancy = 2, BasePrice = 299.99m, ImageUrl = "https://images.unsplash.com/photo-1631049307264-da0ec9d70304" },
            new RoomCategory { Id = 2, HotelId = 1, Name = "Deluxe Suite", Description = "Spacious suite with living area and skyline views", MaxOccupancy = 3, BasePrice = 549.99m, ImageUrl = "https://images.unsplash.com/photo-1590490360182-c33d955a75e6" },
            new RoomCategory { Id = 3, HotelId = 1, Name = "Presidential Suite", Description = "Luxurious top-floor suite with panoramic views", MaxOccupancy = 4, BasePrice = 1299.99m, ImageUrl = "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b" },
            // Seaside Resort
            new RoomCategory { Id = 4, HotelId = 2, Name = "Ocean View Room", Description = "Room with direct ocean views and balcony", MaxOccupancy = 2, BasePrice = 249.99m, ImageUrl = "https://images.unsplash.com/photo-1602002418082-a4443e081dd1" },
            new RoomCategory { Id = 5, HotelId = 2, Name = "Beach Suite", Description = "Beachfront suite with private terrace", MaxOccupancy = 3, BasePrice = 449.99m, ImageUrl = "https://images.unsplash.com/photo-1591088398332-8a7791972843" },
            // Mountain View Lodge
            new RoomCategory { Id = 6, HotelId = 3, Name = "Standard Cabin", Description = "Cozy cabin-style room with mountain views", MaxOccupancy = 2, BasePrice = 149.99m, ImageUrl = "https://images.unsplash.com/photo-1595576508898-0ad5c879a061" },
            new RoomCategory { Id = 7, HotelId = 3, Name = "Family Cabin", Description = "Spacious cabin suitable for families", MaxOccupancy = 5, BasePrice = 249.99m, ImageUrl = "https://images.unsplash.com/photo-1566665797739-1674de7a421a" },
            // Urban Boutique
            new RoomCategory { Id = 8, HotelId = 4, Name = "Studio Room", Description = "Stylish studio with modern amenities", MaxOccupancy = 2, BasePrice = 199.99m, ImageUrl = "https://images.unsplash.com/photo-1618773928121-c32242e63f39" },
            new RoomCategory { Id = 9, HotelId = 4, Name = "Loft Suite", Description = "Two-level loft with city views", MaxOccupancy = 3, BasePrice = 399.99m, ImageUrl = "https://images.unsplash.com/photo-1590490360182-c33d955a75e6" },
            // Historic Garden Inn
            new RoomCategory { Id = 10, HotelId = 5, Name = "Garden Room", Description = "Charming room overlooking the gardens", MaxOccupancy = 2, BasePrice = 179.99m, ImageUrl = "https://images.unsplash.com/photo-1566073771259-6a8506099945" },
            // Skyline Tower
            new RoomCategory { Id = 11, HotelId = 6, Name = "City View Room", Description = "Modern room with floor-to-ceiling windows", MaxOccupancy = 2, BasePrice = 279.99m, ImageUrl = "https://images.unsplash.com/photo-1631049307264-da0ec9d70304" },
            new RoomCategory { Id = 12, HotelId = 6, Name = "Executive Suite", Description = "Premium suite with separate office area", MaxOccupancy = 2, BasePrice = 599.99m, ImageUrl = "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b" },
            // Desert Oasis
            new RoomCategory { Id = 13, HotelId = 7, Name = "Desert View Room", Description = "Room with stunning desert landscape views", MaxOccupancy = 2, BasePrice = 219.99m, ImageUrl = "https://images.unsplash.com/photo-1602002418082-a4443e081dd1" },
            new RoomCategory { Id = 14, HotelId = 7, Name = "Pool Villa", Description = "Private villa with its own pool", MaxOccupancy = 4, BasePrice = 699.99m, ImageUrl = "https://images.unsplash.com/photo-1591088398332-8a7791972843" },
            // Lakefront Suites
            new RoomCategory { Id = 15, HotelId = 8, Name = "Lake View Room", Description = "Peaceful room with lake views", MaxOccupancy = 2, BasePrice = 169.99m, ImageUrl = "https://images.unsplash.com/photo-1595576508898-0ad5c879a061" },
            new RoomCategory { Id = 16, HotelId = 8, Name = "Family Suite", Description = "Spacious suite ideal for families", MaxOccupancy = 5, BasePrice = 329.99m, ImageUrl = "https://images.unsplash.com/photo-1566665797739-1674de7a421a" },
            // The Royal Heritage (Ahmedabad)
            new RoomCategory { Id = 17, HotelId = 9, Name = "Heritage Room", Description = "Traditional room with carved wooden furniture and courtyard views", MaxOccupancy = 2, BasePrice = 149.99m, ImageUrl = "https://images.unsplash.com/photo-1631049307264-da0ec9d70304" },
            new RoomCategory { Id = 18, HotelId = 9, Name = "Maharaja Suite", Description = "Opulent suite with private balcony and royal decor", MaxOccupancy = 3, BasePrice = 349.99m, ImageUrl = "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b" },
            // Taj Gateway Mumbai
            new RoomCategory { Id = 19, HotelId = 10, Name = "Sea View Room", Description = "Elegant room with panoramic Arabian Sea views", MaxOccupancy = 2, BasePrice = 299.99m, ImageUrl = "https://images.unsplash.com/photo-1602002418082-a4443e081dd1" },
            new RoomCategory { Id = 20, HotelId = 10, Name = "Presidential Suite", Description = "Ultra-luxury suite with butler service and private dining", MaxOccupancy = 4, BasePrice = 899.99m, ImageUrl = "https://images.unsplash.com/photo-1590490360182-c33d955a75e6" },
            // Delhi Imperial Palace
            new RoomCategory { Id = 21, HotelId = 11, Name = "Imperial Room", Description = "Spacious room with Mughal-inspired art and city views", MaxOccupancy = 2, BasePrice = 249.99m, ImageUrl = "https://images.unsplash.com/photo-1618773928121-c32242e63f39" },
            new RoomCategory { Id = 22, HotelId = 11, Name = "Royal Suite", Description = "Grand suite with separate living area and India Gate views", MaxOccupancy = 3, BasePrice = 599.99m, ImageUrl = "https://images.unsplash.com/photo-1582719478250-c89cae4dc85b" },
            // Pink City Resort (Jaipur)
            new RoomCategory { Id = 23, HotelId = 12, Name = "Rajputana Room", Description = "Vibrant room with traditional Rajasthani textiles", MaxOccupancy = 2, BasePrice = 129.99m, ImageUrl = "https://images.unsplash.com/photo-1595576508898-0ad5c879a061" },
            new RoomCategory { Id = 24, HotelId = 12, Name = "Palace Suite", Description = "Luxurious suite with rooftop access and fort views", MaxOccupancy = 4, BasePrice = 299.99m, ImageUrl = "https://images.unsplash.com/photo-1591088398332-8a7791972843" },
            // Goa Beach Paradise
            new RoomCategory { Id = 25, HotelId = 13, Name = "Beach Cottage", Description = "Cozy cottage steps from the beach with hammock", MaxOccupancy = 2, BasePrice = 179.99m, ImageUrl = "https://images.unsplash.com/photo-1602002418082-a4443e081dd1" },
            new RoomCategory { Id = 26, HotelId = 13, Name = "Pool Villa", Description = "Private villa with plunge pool and ocean views", MaxOccupancy = 4, BasePrice = 449.99m, ImageUrl = "https://images.unsplash.com/photo-1591088398332-8a7791972843" },
            // Bangalore Tech Suites
            new RoomCategory { Id = 27, HotelId = 14, Name = "Business Room", Description = "Modern room with high-speed WiFi and work desk", MaxOccupancy = 2, BasePrice = 159.99m, ImageUrl = "https://images.unsplash.com/photo-1631049307264-da0ec9d70304" },
            new RoomCategory { Id = 28, HotelId = 14, Name = "Executive Suite", Description = "Premium suite with lounge access and meeting room", MaxOccupancy = 3, BasePrice = 349.99m, ImageUrl = "https://images.unsplash.com/photo-1590490360182-c33d955a75e6" }
        );

        // Rooms (3 rooms per category for first few hotels)
        var rooms = new List<Room>();
        int roomId = 1;
        for (int catId = 1; catId <= 28; catId++)
        {
            int roomCount = catId <= 5 ? 3 : 2;
            for (int r = 1; r <= roomCount; r++)
            {
                rooms.Add(new Room
                {
                    Id = roomId,
                    RoomCategoryId = catId,
                    RoomNumber = $"{catId}{r:D2}",
                    Floor = (catId % 5) + 1,
                    IsAvailable = true
                });
                roomId++;
            }
        }
        builder.Entity<Room>().HasData(rooms);

        // Hotel Amenities
        builder.Entity<HotelAmenity>().HasData(
            // Grand Palace - all amenities
            new HotelAmenity { HotelId = 1, AmenityId = 1 }, new HotelAmenity { HotelId = 1, AmenityId = 2 },
            new HotelAmenity { HotelId = 1, AmenityId = 3 }, new HotelAmenity { HotelId = 1, AmenityId = 4 },
            new HotelAmenity { HotelId = 1, AmenityId = 5 }, new HotelAmenity { HotelId = 1, AmenityId = 6 },
            new HotelAmenity { HotelId = 1, AmenityId = 7 }, new HotelAmenity { HotelId = 1, AmenityId = 8 },
            // Seaside Resort
            new HotelAmenity { HotelId = 2, AmenityId = 1 }, new HotelAmenity { HotelId = 2, AmenityId = 2 },
            new HotelAmenity { HotelId = 2, AmenityId = 5 }, new HotelAmenity { HotelId = 2, AmenityId = 6 },
            new HotelAmenity { HotelId = 2, AmenityId = 7 }, new HotelAmenity { HotelId = 2, AmenityId = 8 },
            // Mountain View Lodge
            new HotelAmenity { HotelId = 3, AmenityId = 1 }, new HotelAmenity { HotelId = 3, AmenityId = 4 },
            new HotelAmenity { HotelId = 3, AmenityId = 10 },
            // Urban Boutique
            new HotelAmenity { HotelId = 4, AmenityId = 1 }, new HotelAmenity { HotelId = 4, AmenityId = 3 },
            new HotelAmenity { HotelId = 4, AmenityId = 5 }, new HotelAmenity { HotelId = 4, AmenityId = 8 },
            // Historic Garden Inn
            new HotelAmenity { HotelId = 5, AmenityId = 1 }, new HotelAmenity { HotelId = 5, AmenityId = 4 },
            new HotelAmenity { HotelId = 5, AmenityId = 5 },
            // Skyline Tower
            new HotelAmenity { HotelId = 6, AmenityId = 1 }, new HotelAmenity { HotelId = 6, AmenityId = 2 },
            new HotelAmenity { HotelId = 6, AmenityId = 3 }, new HotelAmenity { HotelId = 6, AmenityId = 5 },
            new HotelAmenity { HotelId = 6, AmenityId = 7 }, new HotelAmenity { HotelId = 6, AmenityId = 8 },
            new HotelAmenity { HotelId = 6, AmenityId = 9 },
            // Desert Oasis
            new HotelAmenity { HotelId = 7, AmenityId = 1 }, new HotelAmenity { HotelId = 7, AmenityId = 2 },
            new HotelAmenity { HotelId = 7, AmenityId = 3 }, new HotelAmenity { HotelId = 7, AmenityId = 6 },
            new HotelAmenity { HotelId = 7, AmenityId = 9 },
            // Lakefront Suites
            new HotelAmenity { HotelId = 8, AmenityId = 1 }, new HotelAmenity { HotelId = 8, AmenityId = 4 },
            new HotelAmenity { HotelId = 8, AmenityId = 10 },
            // The Royal Heritage (Ahmedabad) - WiFi, Pool, Gym, Parking, Restaurant, Spa, Room Service
            new HotelAmenity { HotelId = 9, AmenityId = 1 }, new HotelAmenity { HotelId = 9, AmenityId = 2 },
            new HotelAmenity { HotelId = 9, AmenityId = 3 }, new HotelAmenity { HotelId = 9, AmenityId = 4 },
            new HotelAmenity { HotelId = 9, AmenityId = 5 }, new HotelAmenity { HotelId = 9, AmenityId = 6 },
            new HotelAmenity { HotelId = 9, AmenityId = 7 },
            // Taj Gateway Mumbai - WiFi, Pool, Gym, Restaurant, Spa, Room Service, Bar, Airport Shuttle
            new HotelAmenity { HotelId = 10, AmenityId = 1 }, new HotelAmenity { HotelId = 10, AmenityId = 2 },
            new HotelAmenity { HotelId = 10, AmenityId = 3 }, new HotelAmenity { HotelId = 10, AmenityId = 5 },
            new HotelAmenity { HotelId = 10, AmenityId = 6 }, new HotelAmenity { HotelId = 10, AmenityId = 7 },
            new HotelAmenity { HotelId = 10, AmenityId = 8 }, new HotelAmenity { HotelId = 10, AmenityId = 9 },
            // Delhi Imperial Palace - WiFi, Pool, Gym, Parking, Restaurant, Spa, Room Service, Bar
            new HotelAmenity { HotelId = 11, AmenityId = 1 }, new HotelAmenity { HotelId = 11, AmenityId = 2 },
            new HotelAmenity { HotelId = 11, AmenityId = 3 }, new HotelAmenity { HotelId = 11, AmenityId = 4 },
            new HotelAmenity { HotelId = 11, AmenityId = 5 }, new HotelAmenity { HotelId = 11, AmenityId = 6 },
            new HotelAmenity { HotelId = 11, AmenityId = 7 }, new HotelAmenity { HotelId = 11, AmenityId = 8 },
            // Pink City Resort (Jaipur) - WiFi, Pool, Restaurant, Spa, Parking
            new HotelAmenity { HotelId = 12, AmenityId = 1 }, new HotelAmenity { HotelId = 12, AmenityId = 2 },
            new HotelAmenity { HotelId = 12, AmenityId = 5 }, new HotelAmenity { HotelId = 12, AmenityId = 6 },
            new HotelAmenity { HotelId = 12, AmenityId = 4 },
            // Goa Beach Paradise - WiFi, Pool, Restaurant, Bar, Spa, Pet Friendly
            new HotelAmenity { HotelId = 13, AmenityId = 1 }, new HotelAmenity { HotelId = 13, AmenityId = 2 },
            new HotelAmenity { HotelId = 13, AmenityId = 5 }, new HotelAmenity { HotelId = 13, AmenityId = 8 },
            new HotelAmenity { HotelId = 13, AmenityId = 6 }, new HotelAmenity { HotelId = 13, AmenityId = 10 },
            // Bangalore Tech Suites - WiFi, Gym, Restaurant, Bar, Airport Shuttle, Parking
            new HotelAmenity { HotelId = 14, AmenityId = 1 }, new HotelAmenity { HotelId = 14, AmenityId = 3 },
            new HotelAmenity { HotelId = 14, AmenityId = 5 }, new HotelAmenity { HotelId = 14, AmenityId = 8 },
            new HotelAmenity { HotelId = 14, AmenityId = 9 }, new HotelAmenity { HotelId = 14, AmenityId = 4 }
        );

        // Promotions
        builder.Entity<Promotion>().HasData(
            new Promotion { Id = 1, Code = "WELCOME10", Description = "10% off for new users", DiscountPercent = 10, ValidFrom = new DateTime(2024, 1, 1), ValidTo = new DateTime(2025, 12, 31), IsActive = true },
            new Promotion { Id = 2, Code = "SUMMER25", Description = "25% off summer bookings", DiscountPercent = 25, ValidFrom = new DateTime(2024, 6, 1), ValidTo = new DateTime(2024, 9, 30), IsActive = true },
            new Promotion { Id = 3, Code = "LOYALTY15", Description = "15% loyalty reward for returning guests", DiscountPercent = 15, ValidFrom = new DateTime(2024, 1, 1), ValidTo = new DateTime(2025, 12, 31), IsActive = true },
            new Promotion { Id = 4, Code = "WEEKEND20", Description = "20% off weekend stays", DiscountPercent = 20, ValidFrom = new DateTime(2024, 1, 1), ValidTo = new DateTime(2025, 6, 30), IsActive = true }
        );
    }
}
