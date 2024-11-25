using System.ComponentModel.DataAnnotations;

namespace BookArchive.Models;

public class Book
{
    public Book(string title)
    {
        Title = title;
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string? Description { get; set; }

    public byte[]? CoverImage { get; set; }

    public byte[]? BookFile { get; set; }

    public int? Year { get; set; }
}