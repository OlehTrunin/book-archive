using BookArchive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookArchive.Controllers
{
    public class RatingsController(DataContext context) : Controller
    {
        [HttpPost("{bookId}")]
        public async Task<IActionResult> Create(int bookId, [FromBody] Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // rating.User = User.Identity.Name;
            // rating.Book = bookId;

            context.Ratings.Add(rating);
            await context.SaveChangesAsync();

            return Ok(rating);
        }

        [HttpPut("{bookId}/{ratingId}")]
        public async Task<IActionResult> Update(int bookId, int ratingId, [FromBody] Rating rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingRating = await context.Ratings.FindAsync(ratingId);

            if (existingRating == null)
            {
                return NotFound();
            }

            existingRating.RatingValue = rating.RatingValue;

            context.Ratings.Update(existingRating);
            await context.SaveChangesAsync();

            return Ok(existingRating);
        }

        [HttpGet("{bookId}")]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRatings(int bookId)
        {
            var ratings = await context.Ratings
                // .Where(r => r.Book == bookId)
                .ToListAsync();

            return ratings.Any()
                ? Ok(ratings)
                : NotFound();
        }
    }
}