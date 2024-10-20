using System.Collections.ObjectModel;
using book_archive.Controllers;
using book_archive.Models;
using Microsoft.EntityFrameworkCore;

namespace book_archive
{
    public class BookArchiveDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public BookArchiveDbContext(DbContextOptions<BookArchiveDbContext> options) : base(options)
        {
            
        }
    }
}