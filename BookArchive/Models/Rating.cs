namespace BookArchive.Models;

public class Rating
{
    public int Id { get; set; }
    public int RatingValue { get; set; }
    public int BookId { get; set; }
    public int UserId { get; set; }
}