using System.Numerics;

namespace AndreyApi.Models
{
    public class BookUi
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }        
        public int GenreId { get; set; }
        public AutorUi AutorUi { get; set; }
        public GenreUi GenreUi { get; set; }
        public static implicit operator Book(BookUi bookUi)
        {
            return new Book() { Id = bookUi.Id, Title = bookUi.Title, GenreId = bookUi.GenreId, AuthorId = bookUi.AuthorId, Description = bookUi.Description, Author = bookUi.AutorUi, Genre =bookUi.GenreUi };
        }

        public static implicit operator BookUi(Book bookUi)
        {
            return new BookUi() { Id = bookUi.Id, Title = bookUi.Title, GenreId = bookUi.GenreId, AuthorId = bookUi.AuthorId, Description = bookUi.Description, AutorUi = bookUi.Author, GenreUi = bookUi.Genre };
        }
    }

    public class AutorUi
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static implicit operator Autor(AutorUi genreUi)
        {
            return new Autor() { Id = genreUi.Id, Name = genreUi.Name };
        }

        public static implicit operator AutorUi(Autor genre)
        {
            return new AutorUi() { Id = genre.Id, Name = genre.Name };
        }

    }


    public class GenreUi
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator Genre(GenreUi genreUi)
        {
            return new Genre() { Id = genreUi.Id, Name = genreUi.Name };
        }

        public static implicit operator GenreUi(Genre genre)
        {
            return new GenreUi() { Id = genre.Id, Name = genre.Name };
        }
    }
}
