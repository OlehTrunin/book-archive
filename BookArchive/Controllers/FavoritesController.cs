using System.Security.Claims;
using BookArchive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookArchive.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FavoritesController(DataContext context) : ControllerBase
{
    [HttpPost("{bookId}")]
    public async Task<IActionResult> AddFavorite(int bookId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var favorite = new Favorite
        {
            UserId = int.Parse(userId),
            BookId = bookId,
            CreatedAt = DateTime.UtcNow
        };

        context.Favorites.Add(favorite);
        await context.SaveChangesAsync();

        return Ok(favorite);
    }

    [HttpDelete("{bookId}")]
    public async Task<IActionResult> RemoveFavorite(int bookId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var userIdInt = int.Parse(userId);
        var favorite = await context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userIdInt && f.BookId == bookId);

        if (favorite == null)
        {
            return NotFound();
        }

        context.Favorites.Remove(favorite);
        await context.SaveChangesAsync();

        return Ok();
    }
}