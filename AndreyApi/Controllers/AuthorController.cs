using AndreyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AndreyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
        AppDBContext dbContext;
        public AuthorController(AppDBContext appcontext)
        {
            dbContext = appcontext;
        }

        [HttpGet()]
        public ActionResult<object> Get()
        {
            return  new { Data= dbContext.Authors.Select(u => (AuthorUi)u).ToList() };
        }

        [HttpGet("{Id:int}")]
        public ActionResult<AuthorUi> Get(int Id)
        {
            var autor = dbContext.Authors.FirstOrDefault(u => u.Id == Id);
            if (autor == null) { return NotFound(); }
            return (AuthorUi)autor;
        }

        [HttpPost]
        public ActionResult<AuthorUi> Post([FromBody] AuthorUi autor)
        {
            if(autor is null) { return BadRequest("Пустой автор"); }
            if(autor.Name == string.Empty) { return BadRequest("Пустое имя"); }
            if(autor.Id !=0 ) { return BadRequest("Попытка обновления, воспользуйтесь методом PUT"); }
            Author aut = autor;
            dbContext.Authors.Add(aut);
            dbContext.SaveChanges();

            return (AuthorUi)aut;
        }

        [HttpPut("{Id:int}")]
        public ActionResult<AuthorUi> Put(int Id,[FromBody] AuthorUi autor)
        {
            if (autor is null) { return BadRequest("Пустой автор"); }
            if (autor.Name == string.Empty) { return BadRequest("Пустое имя"); }
            if (!dbContext.Authors.Any(u=>u.Id == Id)) { return BadRequest("Попытка добавления, воспользуйтесь методом POST"); }
            var aut = dbContext.Authors.FirstOrDefault(u => u.Id==Id);
            aut.Name = autor.Name;
            dbContext.Authors.Update(aut);
            dbContext.SaveChanges();

            return (AuthorUi)aut;
        }

        [HttpDelete("{Id:int}")]
        public ActionResult Del(int Id)
        {
            if (!dbContext.Authors.Any(u=>u.Id == Id)) { return BadRequest("Не верный ID"); }

            dbContext.Authors.Remove(dbContext.Authors.First(u => u.Id == Id));

            dbContext.SaveChanges();
            return Ok();
        }
    }
}
