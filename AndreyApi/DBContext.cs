using AndreyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AndreyApi
{
    public class AppDBContext : DbContext
    {
        public DbSet<Autor> Autors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Image> Images { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Autor>().HasKey(autor => autor.Id);
            modelBuilder.Entity<Autor>().Property(u=>u.Id).ValueGeneratedOnAdd();  


            modelBuilder.Entity<Book>().HasKey(u=>u.Id);
            modelBuilder.Entity<Book>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Book>().HasOne(u=>u.Author).WithMany(u=>u.Books).HasForeignKey(u=>u.AuthorId);
            modelBuilder.Entity<Book>().HasOne(u => u.Genre).WithMany(u => u.Books).HasForeignKey(u => u.GenreId);

            modelBuilder.Entity<Genre>().HasKey(u => u.Id);
            modelBuilder.Entity<Genre>().Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Image>().HasKey(u => u.Id);
            modelBuilder.Entity<Image>().Property(u => u.Id).ValueGeneratedOnAdd();

        }
    }
}
