using Microsoft.EntityFrameworkCore;

namespace book_archive
{
    public class BookarchiveDbContext : DbContext
    {
        public BookArchiveDbContext(DbContextOptions<BookarchiveDbContext> options)
        {
            
        }
    }
}