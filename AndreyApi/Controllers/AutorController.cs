using AndreyApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AndreyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutorController : ControllerBase
    {
        AppDBContext dbContext;
        public AutorController(AppDBContext appcontext)
        {
            dbContext = appcontext;
        }

        [HttpGet()]
        public IEnumerable<AutorUi> Get()
        {
            return dbContext.Autors.Select(u=>(AutorUi)u).ToList();
        }

        [HttpGet("{Id:int}")]
        public ActionResult<AutorUi> Get(int Id)
        {
            var autor = dbContext.Autors.FirstOrDefault(u => u.Id == Id);
            if (autor == null) { return NotFound(); }
            return (AutorUi)autor;
        }

        [HttpPost]
        public ActionResult<AutorUi> Post([FromBody] AutorUi autor)
        {
            if(autor is null) { return BadRequest("Пустой автор"); }
            if(autor.Name == string.Empty) { return BadRequest("Пустое имя"); }
            if(autor.Id !=0 ) { return BadRequest("Попытка обновления, воспользуйтесь методом PUT"); }
            Autor aut = autor;
            dbContext.Autors.Add(aut);
            dbContext.SaveChanges();

            return (AutorUi)aut;
        }

        [HttpPut("{Id:int}")]
        public ActionResult<AutorUi> Put(int Id,[FromBody] AutorUi autor)
        {
            if (autor is null) { return BadRequest("Пустой автор"); }
            if (autor.Name == string.Empty) { return BadRequest("Пустое имя"); }
            if (!dbContext.Autors.Any(u=>u.Id == Id)) { return BadRequest("Попытка добавления, воспользуйтесь методом POST"); }
            var aut = dbContext.Autors.FirstOrDefault(u => u.Id==Id);
            aut.Name = autor.Name;
            dbContext.Autors.Update(aut);
            dbContext.SaveChanges();

            return (AutorUi)aut;
        }

        [HttpDelete("{Id:int}")]
        public ActionResult Del(int Id)
        {
            if (!dbContext.Autors.Any(u=>u.Id == Id)) { return BadRequest("NotFound"); }

            dbContext.Autors.Remove(dbContext.Autors.First(u => u.Id == Id));

            dbContext.SaveChanges();
            return Ok();
        }
    }
}
