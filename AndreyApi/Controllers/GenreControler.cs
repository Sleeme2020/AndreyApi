using AndreyApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace AndreyApi.Controllers
{
   
    [ApiController]
    [Route("[controller]")]
    public class GenreController : ControllerBase
    {
        AppDBContext dbContext;
        public GenreController(AppDBContext appcontext)
        {
            dbContext = appcontext;
        }

        [HttpGet()]
        public ActionResult<object> Get()
        {
            return new { Data = dbContext.Genres.Select(u => (GenreUi)u).ToList() };
        }

        [HttpGet("{Id:int}")]
        public ActionResult<GenreUi> Get(int Id)
        {
            var genre = dbContext.Genres.FirstOrDefault(u => u.Id == Id);
            if (genre == null) { return NotFound(); }
            return (GenreUi)genre;
        }

        [HttpPost]
        public ActionResult<GenreUi> Post([FromBody] GenreUi genre)
        {
            if (genre is null) { return BadRequest("Пустой автор"); }
            if (genre.Name == string.Empty) { return BadRequest("Пустое имя"); }
            if (genre.Id != 0) { return BadRequest("Попытка обновления, воспользуйтесь методом PUT"); }
            Genre gen = genre;
            dbContext.Genres.Add(gen);
            dbContext.SaveChanges();

            return (GenreUi)gen;
        }

        [HttpPut("{Id:int}")]
        public ActionResult<GenreUi> Put(int Id, [FromBody] GenreUi genre)
        {
            if (genre is null) { return BadRequest("Пустой автор"); }
            if (genre.Name == string.Empty) { return BadRequest("Пустое имя"); }
            if (!dbContext.Genres.Any(u => u.Id == Id)) { return BadRequest("Попытка добавления, воспользуйтесь методом POST"); }
            var aut = dbContext.Genres.FirstOrDefault(u => u.Id == Id);
            aut.Name = genre.Name;
            dbContext.Genres.Update(aut);
            dbContext.SaveChanges();

            return (GenreUi)aut;
        }

        [HttpDelete("{Id:int}")]
        public ActionResult Del(int Id)
        {
            if (!dbContext.Genres.Any(u => u.Id == Id)) { return BadRequest("Не верный ID"); }

            dbContext.Genres.Remove(dbContext.Genres.First(u => u.Id == Id));

            dbContext.SaveChanges();
            return Ok();
        }
    }
}
