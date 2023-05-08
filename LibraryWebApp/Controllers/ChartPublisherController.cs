using LibraryWebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartPublisherController : ControllerBase
    {
        private readonly DblibraryContext _context;
        public ChartPublisherController(DblibraryContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var publishers = _context.Publishers.ToList();
            List<object> publishersBook = new List<object>();
            publishersBook.Add(new[] { "Видавець", "Кількість книжок" });
            foreach (var c in publishers)
            {
                var booksInPublisher = from book in _context.Books
                                      where book.PublisherId == c.Id
                                      select book;

                int n = booksInPublisher.Count();
                publishersBook.Add(new object[] { c.Name, n });
            }
            return new JsonResult(publishersBook);
        }
    }
}
