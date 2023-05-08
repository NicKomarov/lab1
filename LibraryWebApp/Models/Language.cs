using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Models;

[Table("Language")]
public partial class Language
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [InverseProperty("Language")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
