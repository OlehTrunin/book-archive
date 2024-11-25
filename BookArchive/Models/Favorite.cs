namespace BookArchive.Models;

public class Favorite
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
}