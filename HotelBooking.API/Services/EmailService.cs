using MailKit.Net.Smtp;
using MimeKit;

namespace HotelBooking.API.Services;

public interface IEmailService
{
    Task SendBookingConfirmationAsync(string toEmail, string guestName, string bookingRef,
        string hotelName, string roomCategory, string roomNumber,
        DateTime checkIn, DateTime checkOut, decimal totalPrice);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task SendBookingConfirmationAsync(string toEmail, string guestName, string bookingRef,
        string hotelName, string roomCategory, string roomNumber,
        DateTime checkIn, DateTime checkOut, decimal totalPrice)
    {
        try
        {
            var smtpHost = _config["Email:SmtpHost"];
            if (string.IsNullOrEmpty(smtpHost))
            {
                _logger.LogWarning("Email SMTP not configured. Skipping email for booking {BookingRef}", bookingRef);
                return;
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Hotel Booking", _config["Email:FromAddress"] ?? "noreply@hotelbooking.com"));
            message.To.Add(new MailboxAddress(guestName, toEmail));
            message.Subject = $"Booking Confirmation - {bookingRef}";

            message.Body = new TextPart("html")
            {
                Text = $@"
                <html>
                <body style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <div style='background: #2563eb; color: white; padding: 20px; text-align: center;'>
                        <h1>Booking Confirmed!</h1>
                    </div>
                    <div style='padding: 20px; background: #f8fafc;'>
                        <p>Dear {guestName},</p>
                        <p>Your booking has been confirmed. Here are your details:</p>
                        <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                            <tr><td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'><strong>Booking Reference</strong></td><td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'>{bookingRef}</td></tr>
                            <tr><td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'><strong>Hotel</strong></td><td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'>{hotelName}</td></tr>
                            <tr><td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'><strong>Room</strong></td><td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'>{roomCategory} - {roomNumber}</td></tr>
                            <tr><td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'><strong>Check-in</strong></td><td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'>{checkIn:dddd, MMMM dd, yyyy}</td></tr>
                            <tr><td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'><strong>Check-out</strong></td><td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'>{checkOut:dddd, MMMM dd, yyyy}</td></tr>
                            <tr><td style='padding: 8px;'><strong>Total Price</strong></td><td style='padding: 8px; font-size: 1.2em; color: #2563eb;'>${totalPrice:F2}</td></tr>
                        </table>
                        <p>Thank you for choosing our hotel. We look forward to welcoming you!</p>
                    </div>
                    <div style='padding: 10px; text-align: center; color: #94a3b8; font-size: 12px;'>
                        <p>Hotel Booking System &copy; {DateTime.UtcNow.Year}</p>
                    </div>
                </body>
                </html>"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(
                smtpHost,
                int.Parse(_config["Email:SmtpPort"] ?? "587"),
                MailKit.Security.SecureSocketOptions.StartTls);

            var username = _config["Email:Username"];
            if (!string.IsNullOrEmpty(username))
                await client.AuthenticateAsync(username, _config["Email:Password"]);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Booking confirmation email sent to {Email} for {BookingRef}", toEmail, bookingRef);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send booking confirmation email to {Email}", toEmail);
        }
    }
}
