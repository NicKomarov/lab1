using LibraryWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartGenreController : ControllerBase
    {
        private readonly DblibraryContext _context;
        public ChartGenreController(DblibraryContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var genres = _context.Genres.ToList();
            List<object> genresBook = new List<object>();
            genresBook.Add(new[] { "Жанр", "Кількість книжок" });
            //int i = 1;
            foreach (var c in genres)
            {
                var booksInGenre = from book in _context.Books
                                   where book.GenreId == c.Id
                                   select book;

                int n = booksInGenre.Count();
                genresBook.Add(new object[] { c.Name, n });
                //i = i + 1;
            }
            return new JsonResult(genresBook);
        }
    }
}
