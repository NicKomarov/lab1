using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryWebApp.Models;
using Microsoft.Data.SqlClient;

namespace LibraryWebApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly DblibraryContext _context;

        public BooksController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        public async Task<IActionResult> Index(string? sortId, int? id, string? name)
        {
            var booksBy = from s in _context.Books
                          select s; 
            switch (sortId)
            {
                case "genre":
                    if (id == null) return RedirectToAction("Genres", "Index");

                    ViewBag.GenreId = id;
                    ViewBag.GenreName = name;
                    booksBy = _context.Books.Where(b => b.GenreId == id).Include(b => b.Genre).Include(b => b.Language).Include(b => b.Publisher).Include(b => b.AuthorBooks).ThenInclude(ab => ab.Author);
                    break;
                case "language":
                    if (id == null) return RedirectToAction("Languages", "Index");

                    ViewBag.LanguageId = id;
                    ViewBag.LanguageName = name;
                    booksBy = _context.Books.Where(b => b.LanguageId == id).Include(b => b.Genre).Include(b => b.Language).Include(b => b.Publisher).Include(b => b.AuthorBooks).ThenInclude(ab => ab.Author);
                    break;
                case "publisher":
                    if (id == null) return RedirectToAction("Publishers", "Index");

                    ViewBag.PublisherId = id;
                    ViewBag.PublisherName = name;
                    booksBy = _context.Books.Where(b => b.PublisherId == id).Include(b => b.Genre).Include(b => b.Language).Include(b => b.Publisher).Include(b => b.AuthorBooks).ThenInclude(ab => ab.Author);
                    break;
                case "author":
                    if (id == null) return RedirectToAction("Authors", "Index");

                    ViewBag.AuthorId = id;
                    ViewBag.AuthorName = name;
                    var booksList = _context.AuthorBooks.Where(ab => ab.AuthorId == id).Select(ab => ab.Book);
                    booksBy = _context.Books.Where(b => booksList.Contains(b)).Include(b => b.Genre).Include(b => b.Language).Include(b => b.Publisher).Include(b => b.AuthorBooks).ThenInclude(ab => ab.Author);
                    break;
                case "reader":
                    if (id == null) return RedirectToAction("Readers", "Index");

                    ViewBag.ReaderId = id;
                    ViewBag.ReaderName = name;
                    var booksList1 = _context.IssuedBooks.Where(ab => ab.ReaderId == id).Select(ab => ab.Book);
                    booksBy = _context.Books.Where(b => booksList1.Contains(b)).Include(b => b.Genre).Include(b => b.Language).Include(b => b.Publisher).Include(b => b.AuthorBooks).ThenInclude(ab => ab.Author);
                    break;
                default:
                    booksBy = _context.Books.Include(b => b.Genre).Include(b => b.Language).Include(b => b.Publisher).Include(b => b.AuthorBooks).ThenInclude(ab => ab.Author);
                    break;
            }


            //var dblibraryContext = _context.Books.Include(b => b.Genre).Include(b => b.Language).Include(b => b.Publisher);
            return View(await booksBy.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Language)
                .Include(b => b.Publisher)
                .Include(b => b.AuthorBooks)
                    .ThenInclude(ab => ab.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create(int genreId)
        {
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name");
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name");
            ViewData["AuthorIds"] = new MultiSelectList(_context.Authors, "Id", "Name");
            // ViewBag.GenreId = genreId;
            // ViewBag.GenreName = _context.Genres.Where(c => c.Id == genreId).FirstOrDefault().Name;
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PublisherId,GenreId,LanguageId,Title,Description")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                // return RedirectToAction("Index", "Books", new {id = genreId, name = _context.Genres.Where(c => c.Id == genreId).FirstOrDefault().Name});
            }
            var authorIds = book.AuthorBooks.Select(ab => ab.AuthorId);

            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Description", book.GenreId);
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", book.LanguageId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            ViewData["AuthorIds"] = new MultiSelectList(_context.Authors, "Id", "Name", authorIds);
            return View(book);
            // return RedirectToAction("Index", "Books", new { id = genreId, name = _context.Genres.Where(c => c.Id == genreId).FirstOrDefault().Name });
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", book.LanguageId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PublisherId,GenreId,LanguageId,Title,Description")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
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
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", book.GenreId);
            ViewData["LanguageId"] = new SelectList(_context.Languages, "Id", "Name", book.LanguageId);
            ViewData["PublisherId"] = new SelectList(_context.Publishers, "Id", "Name", book.PublisherId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Genre)
                .Include(b => b.Language)
                .Include(b => b.Publisher)
                .Include(b => b.AuthorBooks)
                    .ThenInclude(ab => ab.Author)
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
            if (_context.Books == null)
            {
                return Problem("Entity set 'DblibraryContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
