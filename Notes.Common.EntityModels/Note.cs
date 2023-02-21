using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Notes.Shared;

public partial class Note
{
    [Key]
    public int NoteId { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(100)")]
    public string? NoteTitle { get; set; }
    [Required]
    [Column(TypeName = "nvarchar(250)")]
    public string? NoteBody { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? NoteDate { get; set; }
}
