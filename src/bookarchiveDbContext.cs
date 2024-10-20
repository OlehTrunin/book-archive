using Microsoft.EntityFrameworkCore;

namespace book_archive
{
    public class BookArchiveDbContext : DbContext
    {
        public BookArchiveDbContext(DbContextOptions<BookArchiveDbContext> options) : base(options)
        {
            
        }
    }
}