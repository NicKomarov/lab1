using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Models;

[Table("Reader")]
public partial class Reader
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Ім'я")]
    public string Name { get; set; } = null!;

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [EmailAddress(ErrorMessage = "Некоректна email адреса")]
    public string Email { get; set; } = null!;

    [StringLength(50)]
    [Required(ErrorMessage = "Поле не може бути порожнім")]
    [Display(Name = "Контактний номер")]
    public string ContactNumber { get; set; } = null!;

    [InverseProperty("Reader")]
    public virtual ICollection<IssuedBook> IssuedBooks { get; set; } = new List<IssuedBook>();
}
