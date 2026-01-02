namespace OfferApiService.Models
{
    public class OfferCreatedEvent
    {
        public int OfferId { get; set; }
        public Seller? Seller { get; set; }
        public string? Vin { get; set; }
        public string? Make { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
        public decimal? Price { get; set; }
        public Location? Location { get; set; }
        public Condition? Condition { get; set; }
    }
}
