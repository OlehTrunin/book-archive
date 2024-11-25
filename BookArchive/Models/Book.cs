using System.ComponentModel.DataAnnotations;

namespace BookArchive.Models;

public class Book
{
    public Book(string title)
    {
        Title = title;
    }

    [Key] public int Id { get; set; }

    public string Title { get; set; }

    public string? Description { get; set; }

    public byte[]? CoverImage { get; set; }

    public byte[]? BookFile { get; set; }

    public int? Year { get; set; }

    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    public DateTime CreatedAt { get; set; }
}