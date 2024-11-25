namespace BookArchive.Models;

public class Rating
{
    public int Id { get; set; }
    public int RatingValue { get; set; }
    public virtual Book Book { get; set; }
    public virtual User User { get; set; }
}