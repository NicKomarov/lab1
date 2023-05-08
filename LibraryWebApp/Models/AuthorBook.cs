using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApp.Models;

[Table("AuthorBook")]
public partial class AuthorBook
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("AuthorID")]
    public int AuthorId { get; set; }

    [Column("BookID")]
    public int BookId { get; set; }

    [ForeignKey("AuthorId")]
    [InverseProperty("AuthorBooks")]
    public virtual Author Author { get; set; } = null!;

    [ForeignKey("BookId")]
    [InverseProperty("AuthorBooks")]
    public virtual Book Book { get; set; } = null!;
}
