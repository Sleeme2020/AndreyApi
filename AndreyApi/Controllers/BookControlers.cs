using AndreyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AndreyApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        AppDBContext dbContext;
        public BookController(AppDBContext appcontext)
        {
            dbContext = appcontext;
        }

        [HttpGet()]
        public ActionResult<object> Get()
        {
            return new { Data = dbContext.Books.Include(u => u.Author).Include(u => u.Genre).Include(u => u.Image).Select(s => (BookUi)s).ToList() };
        }

        [HttpGet("{Id:int}")]
        public ActionResult<BookUi> Get(int Id)
        {
            var book = dbContext.Books.Include(u => u.Author).Include(u => u.Genre).FirstOrDefault(u => u.Id == Id);
            if (book == null) { return NotFound(); }
            return (BookUi)book;
        }

        [HttpPost]
        public ActionResult<BookUi> Post([FromBody] BookUi book)
        {
            if (book is null) { return BadRequest("Пустой автор"); }
            if (book.Title == string.Empty && book.Description == string.Empty
                && book.AuthorId == 0 && book.GenreId == 0 && book.ImageId == 0) { return BadRequest("Пустое имя"); }
            if (book.Id != 0) { return BadRequest("Попытка обновления, воспользуйтесь методом PUT"); }
            Book b = book;
            dbContext.Books.Add(b);
            dbContext.SaveChanges();

            return (BookUi)b;
        }

        [HttpPut("{Id:int}")]
        public ActionResult<BookUi> Put(int Id, [FromBody] BookUi book)
        {
            if (book is null) { return BadRequest("Пустой автор"); }
            if (book.Title == string.Empty && book.Description == string.Empty
                && book.AuthorId == 0 && book.GenreId == 0) { return BadRequest("Пустое имя"); }
            if (!dbContext.Books.Any(u => u.Id == Id)) { return BadRequest("Попытка добавления, воспользуйтесь методом POST"); }
            var aut = dbContext.Books.FirstOrDefault(u => u.Id == Id);
            aut.Title = book.Title;
            aut.Description = book.Description;
            aut.AuthorId = book.AuthorId;
            aut.GenreId = book.GenreId;
            dbContext.Books.Update(aut);
            dbContext.SaveChanges();

            return (BookUi)aut;
        }

        [HttpDelete("{Id:int}")]
        public ActionResult Del(int Id)
        {
            if (!dbContext.Books.Any(u => u.Id == Id)) { return BadRequest("NotFound"); }

            dbContext.Books.Remove(dbContext.Books.First(u => u.Id == Id));

            dbContext.SaveChanges();
            return Ok();
        }
    }
}