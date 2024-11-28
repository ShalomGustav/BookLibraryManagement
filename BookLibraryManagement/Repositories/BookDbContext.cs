using BookLibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace BookLibraryManagement.Repositories;

public class BookDbContext : DbContext
{
    public virtual DbSet<BookModel> Books { get; set; }
    public virtual DbSet<BookAuthorModel> BookAuthor { get; set; }

    public BookDbContext()
    {
    }

    public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookModel>().ToTable("Books").HasKey(x => x.Id);

        modelBuilder.Entity<BookModel>().Property(x => x.Id).HasColumnName("Id").ValueGeneratedOnAdd();
        modelBuilder.Entity<BookModel>().Property(x => x.Title).HasColumnName("Title").HasMaxLength(256);
        modelBuilder.Entity<BookModel>().Property(x => x.PublishedYear).HasColumnName("PublishedYear");
        modelBuilder.Entity<BookModel>().Property(x => x.Genre).HasColumnName("Genre").HasMaxLength(128);

        // Настройка навигационного свойства Author
        modelBuilder.Entity<BookAuthorModel>()
            .HasOne(x => x.Book)
            .WithOne(y => y.Author)
            .HasForeignKey<BookModel>(z => z.AuthorId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        //Data Source=(local);Initial Catalog=BookLibrary;Persist Security Info=True;User ID=test;Password=test;MultipleActiveResultSets=True;Connect Timeout=30;TrustServerCertificate=True
        //Server=DESKTOP-LRED2L4;Database=BookLibrary;Trusted_Connection=True;TrustServerCertificate=True;
        optionsBuilder.UseSqlServer("Server=DESKTOP-LRED2L4;Database=BookLibrary;Trusted_Connection=True;TrustServerCertificate=True");
    }
}
