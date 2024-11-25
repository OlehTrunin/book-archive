using System.Security.Claims;
using BookArchive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookArchive.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RatingsController(DataContext context) : ControllerBase
{
    [HttpPost("{bookId}")]
    public async Task<IActionResult> Create(int bookId, [FromBody] Rating rating)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        rating.UserId = int.Parse(userId);
        rating.BookId = bookId;

        context.Ratings.Add(rating);
        await context.SaveChangesAsync();

        return Ok(rating);
    }

    [HttpPut("{bookId}/{ratingId}")]
    public async Task<IActionResult> Update(int bookId, int ratingId, [FromBody] Rating rating)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var existingRating = await context.Ratings
            .FirstOrDefaultAsync(r => r.Id == ratingId && r.UserId == int.Parse(userId));

        if (existingRating == null)
        {
            return NotFound();
        }

        existingRating.RatingValue = rating.RatingValue;

        await context.SaveChangesAsync();

        return Ok(existingRating);
    }

    [HttpGet("{bookId}")]
    public async Task<ActionResult<IEnumerable<Rating>>> GetRatings(int bookId)
    {
        var ratings = await context.Ratings
            .Where(r => r.BookId == bookId)
            .ToListAsync();

        return ratings.Any()
            ? Ok(ratings)
            : NotFound();
    }
}