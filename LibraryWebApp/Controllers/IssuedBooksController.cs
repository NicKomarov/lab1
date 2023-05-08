using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryWebApp.Models;

namespace LibraryWebApp.Controllers
{
    public class IssuedBooksController : Controller
    {
        private readonly DblibraryContext _context;

        public IssuedBooksController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: IssuedBooks
        public async Task<IActionResult> Index()
        {
            var dblibraryContext = _context.IssuedBooks.Include(i => i.Book).Include(i => i.Reader);
            return View(await dblibraryContext.ToListAsync());
        }

        // GET: IssuedBooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.IssuedBooks == null)
            {
                return NotFound();
            }

            var issuedBook = await _context.IssuedBooks
                .Include(i => i.Book)
                .Include(i => i.Reader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issuedBook == null)
            {
                return NotFound();
            }

            return View(issuedBook);
        }

        // GET: IssuedBooks/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "Name");
            return View();
        }

        // POST: IssuedBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,ReaderId,IssueDate,DueDate,ReturnDate")] IssuedBook issuedBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(issuedBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", issuedBook.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "Name", issuedBook.ReaderId);
            return View(issuedBook);
        }

        // GET: IssuedBooks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.IssuedBooks == null)
            {
                return NotFound();
            }

            var issuedBook = await _context.IssuedBooks.FindAsync(id);
            if (issuedBook == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", issuedBook.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "Name", issuedBook.ReaderId);
            return View(issuedBook);
        }

        // POST: IssuedBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,ReaderId,IssueDate,DueDate,ReturnDate")] IssuedBook issuedBook)
        {
            if (id != issuedBook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(issuedBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IssuedBookExists(issuedBook.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title", issuedBook.BookId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "Name", issuedBook.ReaderId);
            return View(issuedBook);
        }

        // GET: IssuedBooks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.IssuedBooks == null)
            {
                return NotFound();
            }

            var issuedBook = await _context.IssuedBooks
                .Include(i => i.Book)
                .Include(i => i.Reader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (issuedBook == null)
            {
                return NotFound();
            }

            return View(issuedBook);
        }

        // POST: IssuedBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.IssuedBooks == null)
            {
                return Problem("Entity set 'DblibraryContext.IssuedBooks'  is null.");
            }
            var issuedBook = await _context.IssuedBooks.FindAsync(id);
            if (issuedBook != null)
            {
                _context.IssuedBooks.Remove(issuedBook);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IssuedBookExists(int id)
        {
          return (_context.IssuedBooks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
