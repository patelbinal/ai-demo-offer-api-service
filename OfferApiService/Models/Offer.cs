using System.ComponentModel.DataAnnotations;

namespace OfferApiService.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; } // Primary key for Offer
        [Required]
        public int SellerId { get; set; } // Foreign key to Seller
        public Seller? Seller { get; set; } // Navigation property
        public string? Vin { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public decimal? Price { get; set; }
        public bool Discrepancies => true;
        public Location? Location { get; set; }
        public Condition? Condition { get; set; }
    }

    public class Seller
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
        public ICollection<Offer>? Offers { get; set; }
    }

    public class Location
    {
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }

    public class Condition
    {
        public int? Mileage { get; set; }
    }
}
