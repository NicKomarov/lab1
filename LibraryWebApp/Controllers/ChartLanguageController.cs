using LibraryWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartLanguageController : ControllerBase
    {
        private readonly DblibraryContext _context;
        public ChartLanguageController(DblibraryContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var languages = _context.Languages.ToList();
            List<object> languagesBook = new List<object>();
            languagesBook.Add(new[] { "Мова", "Кількість книжок" });
            foreach (var c in languages)
            {
                var booksInLanguage = from book in _context.Books
                                   where book.LanguageId == c.Id
                                   select book;

                int n = booksInLanguage.Count();
                languagesBook.Add(new object[] { c.Name, n });
            }
            return new JsonResult(languagesBook);
        }
    }
}
