using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Shared.Model;

namespace Data.Context;

public partial class BookstoreContext : DbContext
{

    public BookstoreContext()
    {
    }
    public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }

    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Book> Books { get; set; } 
    public virtual DbSet<Format> Formats { get; set; }
    public virtual DbSet<Publisher> Publishers { get; set; }
    public virtual DbSet<BookAuthor> BookAuthors { get; set; }
    public virtual DbSet<Store> Stores { get; set; }
    public virtual DbSet<Inventory> Inventory { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = new SqlConnectionStringBuilder()
        {
            ServerSPN = "localhost",
            InitialCatalog = "Jeremias Granqvist Labb1",
            TrustServerCertificate = true,
            IntegratedSecurity = true
        }.ToString();
        
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Finnish_Swedish_CI_AS");


        //book configuration
        modelBuilder.Entity<Book>()
                    .HasKey(b => b.ISBN13);

        modelBuilder.Entity<Book>()
                    .Property(b => b.Title)
                    .HasMaxLength(255);
       
        modelBuilder.Entity<Book>()
                    .Property(b => b.Language)
                    .HasMaxLength(50);

        modelBuilder.Entity<Book>()
                    .Property(b => b.ReleaseDate);

        //many-to-many bookauthor configuration
        modelBuilder.Entity<Book>()
                    .HasMany(b => b.Authors)
                    .WithMany(a => a.Books)
                    .UsingEntity<BookAuthor>(
              j => j.HasOne(ba => ba.Author)
                    .WithMany(a => a.BookAuthors)
                    .HasForeignKey(ba => ba.AuthorID),
              j => j.HasOne(ba => ba.Book)
                    .WithMany(b => b.BookAuthors)
                    .HasForeignKey(ba => ba.ISBN13),
              j =>
              {
                  j.HasKey(ba => new { ba.ISBN13, ba.AuthorID });
              });

        modelBuilder.Entity<Book>()
            .Navigation(b => b.Authors)
            .UsePropertyAccessMode(PropertyAccessMode.Property);

        //author configuration
        modelBuilder.Entity<Author>()
                    .Property(a => a.Birthdate);
        modelBuilder.Entity<Author>()
                    .HasKey(a => a.AuthorID);


        //format och publisher configuration
        modelBuilder.Entity<Format>()
                    .HasKey(f => f.FormatID);

        modelBuilder.Entity<Publisher>()
                    .HasKey(p => p.PublisherID);

        //stores configuration
        modelBuilder.Entity<Store>()
                    .HasKey(s => s.StoreID);
        modelBuilder.Entity<Store>()
                    .Property(s => s.StoreName)
                    .HasMaxLength(50);

        //inventory configuration
        modelBuilder.Entity<Inventory>()
                    .HasKey(i => new { i.InventoryISBN13, i.StoreID });
        modelBuilder.Entity<Inventory>()
                    .HasOne(i => i.book)
                    .WithMany(b => b.inventories)
                    .HasForeignKey(i => i.InventoryISBN13);

        modelBuilder.Entity<Inventory>()
                    .HasOne(i => i.book)
                    .WithMany(b => b.inventories)
                    .HasForeignKey(i => i.InventoryISBN13);


        base.OnModelCreating(modelBuilder);

    }
}
