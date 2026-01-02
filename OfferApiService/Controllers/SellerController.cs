using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfferApiService.Models;

namespace OfferApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SellerController : ControllerBase
    {
        private readonly AppDbContext _context;
        public SellerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/seller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
        {
            return await _context.Sellers.ToListAsync();
        }

        // GET: api/seller/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Seller>> GetSeller(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
                return NotFound();
            return seller;
        }

        // POST: api/seller
        [HttpPost]
        public async Task<ActionResult<Seller>> CreateSeller(Seller seller)
        {
            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSeller), new { id = seller.Id }, seller);
        }

        // PUT: api/seller/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeller(int id, Seller seller)
        {
            if (id != seller.Id)
                return BadRequest();
            var existingSeller = await _context.Sellers.FindAsync(id);
            if (existingSeller == null)
                return NotFound();
            existingSeller.FirstName = seller.FirstName;
            existingSeller.LastName = seller.LastName;
            existingSeller.Email = seller.Email;
            existingSeller.PhoneNumber = seller.PhoneNumber;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/seller/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeller(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
                return NotFound();
            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

