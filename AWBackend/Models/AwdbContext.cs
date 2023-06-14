using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace AWBackend.Models;

public partial class AwdbContext : DbContext
{
    private readonly IConfiguration Configuration;
    public AwdbContext(IConfiguration _configuration)
    {
       Configuration = _configuration;
    }

    public AwdbContext(DbContextOptions<AwdbContext> options , IConfiguration _configuration)
        : base(options)
    {
        Configuration = _configuration;
    }
    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(this.Configuration.GetConnectionString("ConnStr"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(entity =>
        {
            entity.Property(e => e.Course)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Dob)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
