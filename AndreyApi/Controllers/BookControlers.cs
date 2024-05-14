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
            return new { Data = dbContext.Books.Include(u => u.Authors).Include(u => u.Genres).Include(u => u.Image).Select(s =>(BookUi)s).ToList() };
        }

        [HttpGet("{Id:int}")]
        public ActionResult<BookUi> Get(int Id)
        {
            var book = dbContext.Books.Include(u => u.Authors).Include(u => u.Genres).Include(u => u.Image).FirstOrDefault(u => u.Id == Id);
            if (book == null) { return NotFound(); }
            return (BookUi)book;
        }

        [HttpPost]
        public ActionResult<BookUi> Post([FromBody] BookUi book)
        {
            if (book is null) { return BadRequest("Пустой автор"); }
            if (book.Title == string.Empty || book.Description == string.Empty
               || book.AuthorsID[0] == 0 || book.GenresID[0] == 0 || book.ImageId == 0) { return BadRequest("Не полные данные"); }
            if (book.Id != 0) { return BadRequest("Попытка обновления, воспользуйтесь методом PUT"); }

            var authorID = book.AuthorsID.ToArray().Distinct();
            var genreID = book.GenresID.ToArray().Distinct();

            Book b = book;
            dbContext.Books.Add(b);

            foreach (var a in authorID) b.Authors.Add(dbContext.Authors.FirstOrDefault(u => u.Id == a));
            
            foreach (var a in genreID) b.Genres.Add(dbContext.Genres.FirstOrDefault(u => u.Id == a));
            
            dbContext.SaveChanges();
            return (BookUi)b;
        }

        [HttpPut("{Id:int}")]
        public ActionResult<BookUi> Put(int Id, [FromBody] BookUi book)
        {
            if (book is null) { return BadRequest("Пустой автор"); }
            if (book.Title == string.Empty || book.Description == string.Empty
                || book.AuthorsID[0] == 0 || book.GenresID[0] == 0 || book.ImageId == 0) { return BadRequest("Не полные данные"); }
            if (!dbContext.Books.Any(u => u.Id == Id)) { return BadRequest("Попытка добавления, воспользуйтесь методом POST"); }

            var authorID = book.AuthorsID.ToArray().Distinct();
            var genreID = book.GenresID.ToArray().Distinct();

            var b = dbContext.Books.Include(u => u.Authors).Include(u => u.Genres).FirstOrDefault(u => u.Id == Id);
            b.Title = book.Title;
            b.Description = book.Description;
            b.ImageId = book.ImageId;

            b.Authors.Clear();
            foreach (var a in authorID) b.Authors.Add(dbContext.Authors.FirstOrDefault(u => u.Id == a));

            b.Genres.Clear();
            foreach (var a in genreID) b.Genres.Add(dbContext.Genres.FirstOrDefault(u => u.Id == a));
            
            dbContext.Books.Update(b);
            dbContext.SaveChanges();

            return (BookUi)b;
        }

        [HttpDelete("{Id:int}")]
        public ActionResult Del(int Id)
        {
            if (!dbContext.Books.Any(u => u.Id == Id)) { return BadRequest("Не верный ID"); }

            dbContext.Books.Remove(dbContext.Books.First(u => u.Id == Id));

            dbContext.SaveChanges();
            return Ok();
        }
    }
}