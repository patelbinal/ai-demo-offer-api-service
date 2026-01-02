using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfferApiService.Models;
using OfferApiService.Services;

namespace OfferApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfferController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly RabbitMQPublisher _publisher;
        public OfferController(AppDbContext context, RabbitMQPublisher publisher)
        {
            _context = context;
            _publisher = publisher;
        }

        // GET: api/offer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offer>>> GetOffers()
        {
            return await _context.Offers.ToListAsync();
        }

        // GET: api/offer/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Offer>> GetOffer(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
                return NotFound();
            return offer;
        }

        // POST: api/offer
        [HttpPost]
        public async Task<ActionResult<Offer>> CreateOffer(Offer offer)
        {
            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();
            // Publish OfferCreated event to RabbitMQ
            var offerCreatedEvent = new OfferCreatedEvent
            {
                OfferId = offer.Id,
                Seller = await _context.Sellers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == offer.SellerId),
                Vin = offer.Vin,
                Make = offer.Make,
                Model = offer.Model,
                Year = offer.Year,
                Price = offer.Price,
                Location = offer.Location,
                Condition = offer.Condition
            };
            _publisher.PublishOfferCreated(offerCreatedEvent);
            Console.WriteLine($"OfferCreated event published for SellerId: {offer.SellerId}, VIN: {offer.Vin}");
            return CreatedAtAction(nameof(GetOffer), new { id = offer.Id }, offer);
        }

        // PUT: api/offer/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOffer(int id, Offer offer)
        {
            if (id != offer.Id)
                return BadRequest();
            var existingOffer = await _context.Offers.FindAsync(id);
            if (existingOffer == null)
                return NotFound();
            // Update fields
            existingOffer.SellerId = offer.SellerId;
            existingOffer.Vin = offer.Vin;
            existingOffer.Make = offer.Make;
            existingOffer.Model = offer.Model;
            existingOffer.Year = offer.Year;
            existingOffer.Price = offer.Price;
            existingOffer.Location = offer.Location;
            existingOffer.Condition = offer.Condition;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/offer/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
                return NotFound();
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
