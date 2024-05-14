using AndreyApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AndreyApi
{
    public class AppDBContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
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

            modelBuilder.Entity<Author>().HasKey(autor => autor.Id);
            modelBuilder.Entity<Author>().Property(u=>u.Id).ValueGeneratedOnAdd();  

            modelBuilder.Entity<Book>().HasKey(u=>u.Id);
            modelBuilder.Entity<Book>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Book>().HasOne(u => u.Image).WithMany(u => u.Books).HasForeignKey(u => u.ImageId);

            modelBuilder.Entity<Genre>().HasKey(u => u.Id);
            modelBuilder.Entity<Genre>().Property(u => u.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Image>().HasKey(u => u.Id);
            modelBuilder.Entity<Image>().Property(u => u.Id).ValueGeneratedOnAdd();

        }
    }
}
