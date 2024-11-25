namespace BookArchive.Models;

using System.ComponentModel.DataAnnotations;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public byte[] CoverImage { get; set; }

    public byte[] BookFile { get; set; }

    public int Year { get; set; }
}