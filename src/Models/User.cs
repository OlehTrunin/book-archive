namespace book_archive.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        // TODO: check how to do relations
        public virtual Role Role { get; set; }
    }
}