using System.Security.Claims;
using BookArchive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookArchive.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavoritesController : ControllerBase
{
    private readonly DataContext _context;
    private readonly ILogger<FavoritesController> _logger;

    public FavoritesController(DataContext context, ILogger<FavoritesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("add/{bookId}")]
    public async Task<IActionResult> AddFavorite(int bookId)
    {
        // Debug authentication state
        _logger.LogInformation($"IsAuthenticated: {User.Identity?.IsAuthenticated}");
        _logger.LogInformation($"Name: {User.Identity?.Name}");
        
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Unauthorized(new { success = false, message = "User not authenticated" });
        }

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        _logger.LogInformation($"UserId from claims: {userId}");

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { success = false, message = "User ID not found in claims" });
        }

        // Check if already in favorites
        var exists = await _context.Favorites
            .AnyAsync(f => f.UserId == int.Parse(userId) && f.BookId == bookId);
        
        if (exists)
        {
            return Ok(new { success = true, message = "Книжка вже в улюблених" });
        }

        var favorite = new Favorite
        {
            UserId = int.Parse(userId),
            BookId = bookId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Книжку додано до улюблених" });
    }

    [HttpGet("check/{bookId}")]
    public async Task<IActionResult> CheckFavorite(int bookId)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Ok(new { isFavorite = false });
        }

        var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Ok(new { isFavorite = false });
        }

        var exists = await _context.Favorites
            .AnyAsync(f => f.UserId == int.Parse(userId) && f.BookId == bookId);
        
        return Ok(new { isFavorite = exists });
    }
    
    [HttpDelete("remove/{bookId}")]
    [Authorize]
    public async Task<IActionResult> RemoveFavorite(int bookId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized(new { success = false, message = "User not authenticated" });
        }

        var userIdInt = int.Parse(userId);
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userIdInt && f.BookId == bookId);

        if (favorite == null)
        {
            return NotFound(new { success = false, message = "Книжки немає в улюблених" });
        }

        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();

        return Ok(new { success = true, message = "Книжку вилучено з улюблених" });
    }
}