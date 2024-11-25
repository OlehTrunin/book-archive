using BookArchive.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookArchive.Controllers
{
    public class BooksController(DataContext context) : Controller
    {
        // GET: Books
        public async Task<IActionResult> Index()
        {
            return View(await context.Books.ToListAsync());
        }

// GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            
            return View(book);
        }

// GET: Books/Create
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

// POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Year")] Book book, 
            IFormFile? coverImage, 
            IFormFile? bookFile)
        {
            if (ModelState.IsValid)
            {
                if (coverImage is { Length: > 0 })
                {
                    using var memoryStream = new MemoryStream();
                    await coverImage.CopyToAsync(memoryStream);
                    book.CoverImage = memoryStream.ToArray();
                }

                if (bookFile is { Length: > 0 })
                {
                    using var memoryStream = new MemoryStream();
                    await bookFile.CopyToAsync(memoryStream);
                    book.BookFile = memoryStream.ToArray();
                }
                
                context.Add(book);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

// GET: Books/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            
            return View(book);
        }

// POST: Books/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Year,CoverImage,BookFile")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(book);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

// GET: Books/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var book = await context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            
            return View(book);
        }

// POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book != null)
            {
                context.Books.Remove(book);
            }
            
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return context.Books.Any(e => e.Id == id);
        }
    }
}