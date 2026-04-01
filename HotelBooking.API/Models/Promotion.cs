using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBooking.API.Models;

public class Promotion
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(5,2)")]
    public decimal DiscountPercent { get; set; }

    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }

    public bool IsActive { get; set; } = true;
}
