using System.Collections.Generic;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace AndreyApi.Models
{
    public class BookUi
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ImageId { get; set; }
        public ImageUi ImageUi { get; set; }
        public List<int> AuthorsID { get; set; } = new();
        public List<AuthorUi> AuthorsUi { get; set; } = new();
        public List<int> GenresID { get; set; } = new();
        public List<GenreUi> GenresUi { get; set; } = new();

        public static implicit operator Book(BookUi? bookUi)
        {
            if (bookUi == null) return null;
            return new Book() { Id = bookUi.Id, Title = bookUi.Title, Description = bookUi.Description, ImageId = bookUi.ImageId};
        }

        public static implicit operator BookUi(Book? book)
        {
            if (book == null) return null;
            return new BookUi() { Id = book.Id, Title = book.Title, Description = book.Description, ImageId = book.ImageId,
                                    ImageUi = book.Image, AuthorsUi = [.. book.Authors],  GenresUi = [.. book.Genres] };
        }
    }

    public class AuthorUi
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator Author(AuthorUi? authorUi)
        {
            if (authorUi == null) return null;
            return new Author() { Id = authorUi.Id, Name = authorUi.Name };
        }

        public static implicit operator AuthorUi(Author? author)
        {
            if (author == null) return null;
            return new AuthorUi() { Id = author.Id, Name = author.Name };
        }

    }


    public class GenreUi
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static implicit operator Genre(GenreUi? genreUi)
        {
            if (genreUi == null) return null;
            return new Genre() { Id = genreUi.Id, Name = genreUi.Name };
        }

        public static implicit operator GenreUi(Genre? genre)
        {
            if (genre == null) return null;
            return new GenreUi() { Id = genre.Id, Name = genre.Name };
        }
    }

    public class ImageUi
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public static implicit operator Image(ImageUi? imageUi)
        {
            if (imageUi == null) return null;
            return new Image() { Id = imageUi.Id, Name = imageUi.Name, Type = imageUi.Type, Path = "" };
        }

        public static implicit operator ImageUi(Image? image)
        {
            if (image == null) return null;
            return new ImageUi() { Id = image.Id, Name = image.Name, Type = image.Type };
        }

    }
}
