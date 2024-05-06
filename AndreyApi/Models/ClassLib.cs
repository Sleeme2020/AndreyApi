namespace AndreyApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public Autor Author { get; set; }
        public int GenreId { get; set; }        
        public Genre Genre { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
    }

    public class Autor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<Book> Books { get; set; } = new();
    }


    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Book> Books { get; set; } = new();
    }

    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
    }
}
