using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Models;

public partial class DblibraryContext : DbContext
{
    public DblibraryContext()
    {
    }

    public DblibraryContext(DbContextOptions<DblibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<AuthorBook> AuthorBooks { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<IssuedBook> IssuedBooks { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= DESKTOP-CD5UUTH\\SQLEXPRESS; Database=DBLibrary; Trusted_Connection=True; Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorBook>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BookId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Author).WithMany(p => p.AuthorBooks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorBook_Author1");

            entity.HasOne(d => d.Book).WithMany(p => p.AuthorBooks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuthorBook_Book");
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_Genre");

            entity.HasOne(d => d.Language).WithMany(p => p.Books)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_Language");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book_Publisher");
        });

        modelBuilder.Entity<IssuedBook>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ReaderId).ValueGeneratedOnAdd();

            entity.HasOne(d => d.Book).WithMany(p => p.IssuedBooks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssuedBook_Book");

            entity.HasOne(d => d.Reader).WithMany(p => p.IssuedBooks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_IssuedBook_Reader");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.Property(e => e.ContactNumber).IsFixedLength();
            entity.Property(e => e.Email).IsFixedLength();
            entity.Property(e => e.Name).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
