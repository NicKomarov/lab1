using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Models;

[Table("Book")]
public partial class Book
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("PublisherID")]
    [Display(Name = "Видавець")]
    public int PublisherId { get; set; }

    [Column("GenreID")]
    [Display(Name = "Жанр")]
    public int GenreId { get; set; }

    [Column("LanguageID")]
    [Display(Name = "Мова")]
    public int LanguageId { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Назва")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Опис")]
    public string Description { get; set; } = null!;

    [InverseProperty("Book")]
    [Display(Name = "Автори")]
    public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();

    [ForeignKey("GenreId")]
    [InverseProperty("Books")]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Жанр")]
    public virtual Genre Genre { get; set; } = null!;

    [InverseProperty("Book")]
    public virtual ICollection<IssuedBook> IssuedBooks { get; set; } = new List<IssuedBook>();

    [ForeignKey("LanguageId")]
    [InverseProperty("Books")]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Мова")]
    public virtual Language Language { get; set; } = null!;

    [ForeignKey("PublisherId")]
    [InverseProperty("Books")]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Видавець")]
    public virtual Publisher Publisher { get; set; } = null!;
}
