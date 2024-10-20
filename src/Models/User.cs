namespace book_archive.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}