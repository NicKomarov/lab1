using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Models;

[Table("Author")]
public partial class Author
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Ім'я")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Оцінка")]
    public int Rating { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Кваліфікація")]
    public string Qualification { get; set; } = null!;

    [InverseProperty("Author")]
    public virtual ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();
}
