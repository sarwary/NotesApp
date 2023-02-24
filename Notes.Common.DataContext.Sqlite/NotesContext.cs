using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Notes.Shared;

public partial class NotesContext : DbContext
{

    public NotesContext(DbContextOptions<NotesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Note> Notes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string dir = Environment.CurrentDirectory;
            string path = string.Empty;
            if (dir.EndsWith("net7.0"))
            {
                // Running in the <project>\bin\<Debug|Release>\net7.0 directory.
                path = Path.Combine("..", "..", "..", "..", "Notes.db");
            }
            else
            {
                path = Path.Combine("..", "Notes.db");
            }
            optionsBuilder.UseSqlite($"Filename={path}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
