using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryWebApp.Models;
using ClosedXML.Excel;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryWebApp.Controllers
{
    public class GenresController : Controller
    {
        private readonly DblibraryContext _context;

        public GenresController(DblibraryContext context)
        {
            _context = context;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
              return _context.Genres != null ? 
                          View(await _context.Genres.ToListAsync()) :
                          Problem("Entity set 'DblibraryContext.Genres'  is null.");
        }

        // GET: Genres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            //  return View(Genre);
            return RedirectToAction("Index", "Books", new { sortId = "genre", id = genre.Id, name = genre.Name });
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        // POST: Genres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Genre genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(genre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GenreExists(genre.Id))
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
            return View(genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Genres == null)
            {
                return NotFound();
            }

            var genre = await _context.Genres
                .FirstOrDefaultAsync(m => m.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Genres == null)
            {
                return Problem("Entity set 'DblibraryContext.Genres'  is null.");
            }
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                _context.Genres.Remove(genre);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GenreExists(int id)
        {
          return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                Genre newgen;
                                var c = (from cat in _context.Genres
                                         where cat.Name.Contains(worksheet.Name)
                                         select cat).ToList();
                                if (c.Count > 0)
{
                                    newgen = c[0];
                                }
                                else
                                {
                                    newgen = new Genre();
                                    newgen.Name = worksheet.Name;
                                    newgen.Description = "from EXCEL" ;
                                    //додати в контекст
                                    _context.Genres.Add(newgen);
                                }

                                Language newlan;
                                var l = (from lan in _context.Languages
                                         select lan).ToList();
                                newlan = l[0];

                                Publisher newpub;
                                var p = (from pub in _context.Publishers
                                         select pub).ToList();
                                newpub = p[0];

                                //перегляд усіх рядків
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Book book = new Book();
                                        book.Title = row.Cell(1).Value.ToString();
                                        book.Description = row.Cell(2).Value.ToString();
                                        book.Genre = newgen;
                                        book.Publisher = newpub;
                                        book.Language = newlan;
                                        _context.Books.Add(book);
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                        for (int i = 3; i <= 4; i++)
{
                                            if (row.Cell(i).Value.ToString().Length > 0)
{
                                                Author author;
                                                var a = (from aut in _context.Authors
                                                         where aut.Name.Contains(row.Cell(i).Value.ToString())
                                                         select aut).ToList();
                                                if (a.Count > 0)
{
                                                    author = a[0];
                                                }
                                                else
                                                {
                                                    author = new Author();
                                                    author.Name = row.Cell(i).Value.ToString();
                                                    author.Qualification = "from EXCEL" ;
                                                    author.Rating = 1;
                                                    //додати в контекст
                                                    _context.Add(author);
                                                }
                                                AuthorBook ab = new AuthorBook();
                                                ab.Book = book;
                                                ab.Author = author;
                                                _context.AuthorBooks.Add(ab);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)
                                        Console.WriteLine(e.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var genres = _context.Genres.Include("Books").ToList();
                
                foreach (var c in genres)
                {
                    var worksheet = workbook.Worksheets.Add(c.Name);
                    worksheet.Cell("A1").Value = "Назва" ;
                    worksheet.Cell("B1").Value = "Опис" ;
                    worksheet.Cell("C1").Value = "Автор 1" ;
                    worksheet.Cell("D1").Value = "Автор 2" ;
                    worksheet.Row(1).Style.Font.Bold = true;
                    var books = c.Books.ToList();
                    
                    for (int i = 0; i < books.Count; i++)
{
                        worksheet.Cell(i + 2, 1).Value = books[i].Title;
                        worksheet.Cell(i + 2, 2).Value = books[i].Description;
                        var ab = _context.AuthorBooks.Where(a => a.BookId == books[i].Id).Include("Author").ToList();
                        //більше 2-ох нікуди писати
                        int j = 0;
                        foreach (var a in ab)
                        {
                            if (j < 3)

{
                                worksheet.Cell(i + 2, j + 3).Value = a.Author.Name;
                                j++;
                            }
                        }
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
{
                        
                        FileDownloadName = $"library_{ DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
