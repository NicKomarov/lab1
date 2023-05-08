using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Models;

[Table("Publisher")]
public partial class Publisher
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [EmailAddress(ErrorMessage = "Некоректна email адреса")]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Адреса")]
    public string Adress { get; set; } = null!;

    [InverseProperty("Publisher")]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
