using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Models;

[Table("Genre")]
public partial class Genre
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Жанр")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Інформація про жанр")]
    public string Description { get; set; } = null!;

    [InverseProperty("Genre")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
