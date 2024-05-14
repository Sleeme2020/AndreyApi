using AndreyApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace AndreyApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : ControllerBase
    {
        AppDBContext dbContext;
        public ImageController(AppDBContext appcontext)
        {
            dbContext = appcontext;
        }

        [HttpGet()]
        public ActionResult<object> Get()
        {
            return new { Data = dbContext.Images.Select(s => (ImageUi)s).ToList() };
        }

        [HttpGet("Info{Id:int}")]
        public ActionResult<ImageUi> GetInfo(int Id)
        {
            var image = dbContext.Images.FirstOrDefault(u => u.Id == Id);
            if (image == null) { return NotFound(); }
            return (ImageUi)image;
        }

        [HttpGet("File{Id:int}")]
        public ActionResult<ImageUi> GetFile(int Id)
        {
            if (!dbContext.Images.Any(u => u.Id == Id)) { return BadRequest("Фаила с таким Id не существует"); }
            var image = dbContext.Images.FirstOrDefault(u => u.Id == Id);
            if (image == null) { return NotFound(); }
            return PhysicalFile(image.Path, "application/octet-stream", image.Name);
        }

        [HttpPost]
        public ActionResult<ImageUi> Post(IFormFile file)
        {
            if (file is null) { return BadRequest("Картинка не выбрана"); }

            // путь к папке, где будут храниться файлы
            var uploadPath = $"{Directory.GetCurrentDirectory()}/Image";
            string extension = Path.GetExtension(file.FileName).Replace(".", "");
            uploadPath += $"/{extension}";
            // создаем папку для хранения файлов
            Directory.CreateDirectory(uploadPath);
            // путь к файлу
            string fullPath = $"{uploadPath}/{file.FileName}";

            Image im = new() { Name = file.FileName, Path = fullPath, Type = extension };

            if (dbContext.Images.Any(u => u.Path == fullPath)) { return BadRequest("Фаил с таким именем существует"); }
            dbContext.Images.Add(im);
            dbContext.SaveChanges();

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyToAsync(fileStream);
                return Ok((ImageUi)im);
            }

        }

        [HttpPut("{Id:int}")]
        public ActionResult<ImageUi> Put(int Id, IFormFile file)
        {
            if (file is null) { return BadRequest("Картинка не выбрана"); }
            if (!dbContext.Images.Any(u => u.Id == Id)) { return BadRequest("Попытка добавления, воспользуйтесь методом POST"); }

            // путь к папке, где будут храниться файлы
            var uploadPath = $"{Directory.GetCurrentDirectory()}/Image";
            string extension = Path.GetExtension(file.FileName).Replace(".", "");
            uploadPath += $"/{extension}";
            // создаем папку для хранения файлов
            Directory.CreateDirectory(uploadPath);
            // путь к файлу
            string fullPath = $"{uploadPath}/{file.FileName}";
            
            var im = dbContext.Images.FirstOrDefault(u => u.Id == Id);
            string delfile = im.Path;
            im.Name = file.FileName;
            im.Type = extension;
            im.Path = fullPath;

            if (dbContext.Images.Any(u => u.Path == fullPath)) { return BadRequest("Фаил с таким именем существует"); }
            dbContext.Images.Update(im);
            dbContext.SaveChanges();

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyToAsync(fileStream);
                FileInfo fileInf = new(delfile);
                if (fileInf.Exists) { fileInf.Delete(); }
                return Ok((ImageUi)im);
            }
        }


        [HttpDelete("{Id:int}")]
        public ActionResult Del(int Id)
        {
            if (!dbContext.Images.Any(u => u.Id == Id)) { return BadRequest("Не верный ID"); }

            FileInfo fileInf = new(dbContext.Images.First(u => u.Id == Id).Path);

            if (fileInf.Exists)
            {
                fileInf.Delete();
                dbContext.Images.Remove(dbContext.Images.First(u => u.Id == Id));
                dbContext.SaveChanges();
                return Ok("Фаил удален");
            }
            return NotFound();
        }

    }
}
