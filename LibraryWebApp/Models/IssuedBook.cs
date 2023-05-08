using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Models;

[Table("IssuedBook")]
public partial class IssuedBook
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("BookID")]
    [Display(Name = "Книга")]
    public int BookId { get; set; }

    [Column("ReaderID")]
    [Display(Name = "Читач")]
    public int ReaderId { get; set; }

    [Column(TypeName = "date")]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Дата видачі")]
    public DateTime IssueDate { get; set; }

    [Column(TypeName = "date")]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Дата повернення")]
    public DateTime DueDate { get; set; }

    [Column(TypeName = "date")]
    [Display(Name = "Справжня дата повернення")]
    public DateTime? ReturnDate { get; set; }

    [ForeignKey("BookId")]
    [InverseProperty("IssuedBooks")]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Книга")]
    public virtual Book Book { get; set; } = null!;

    [ForeignKey("ReaderId")]
    [InverseProperty("IssuedBooks")]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Читач")]
    public virtual Reader Reader { get; set; } = null!;
}
