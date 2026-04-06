using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppFive.ModelsPostgre;

public partial class ScienceDbContext : DbContext
{
    public ScienceDbContext()
    {
    }

    public ScienceDbContext(DbContextOptions<ScienceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Direction> Directions { get; set; }

    public virtual DbSet<Grant> Grants { get; set; }

    public virtual DbSet<Partisipant> Partisipants { get; set; }

    public virtual DbSet<Scientist> Scientists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Server=localhost;Username=postgres;Password=1234;Database=science_db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Direction>(entity =>
        {
            entity.HasKey(e => e.IdDirection).HasName("directions_pkey");

            entity.ToTable("directions");

            entity.Property(e => e.IdDirection)
                .HasMaxLength(45)
                .HasColumnName("id_direction");
            entity.Property(e => e.NameDirection)
                .HasMaxLength(45)
                .HasColumnName("name_direction");
        });

        modelBuilder.Entity<Grant>(entity =>
        {
            entity.HasKey(e => e.IdGrants).HasName("grants_pkey");

            entity.ToTable("grants");

            entity.Property(e => e.IdGrants)
                .ValueGeneratedNever()
                .HasColumnName("id_grants");
            entity.Property(e => e.DateEnd).HasColumnName("date_end");
            entity.Property(e => e.DateStart).HasColumnName("date_start");
            entity.Property(e => e.IdDirection)
                .HasMaxLength(45)
                .HasColumnName("id_direction");
            entity.Property(e => e.IdScient).HasColumnName("id_scient");
            entity.Property(e => e.NameTheme)
                .HasMaxLength(45)
                .HasColumnName("name_theme");
            entity.Property(e => e.Organization)
                .HasMaxLength(45)
                .HasColumnName("organization");
            entity.Property(e => e.Summa).HasColumnName("summa");

            entity.HasOne(d => d.IdDirectionNavigation).WithMany(p => p.Grants)
                .HasForeignKey(d => d.IdDirection)
                .HasConstraintName("grants_fk_two");

            entity.HasOne(d => d.IdScientNavigation).WithMany(p => p.Grants)
                .HasForeignKey(d => d.IdScient)
                .HasConstraintName("grants_fk");
        });

        modelBuilder.Entity<Partisipant>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("partisipant");

            entity.Property(e => e.IdGrant).HasColumnName("id_grant");
            entity.Property(e => e.IdScient).HasColumnName("id_scient");

            entity.HasOne(d => d.IdGrantNavigation).WithMany()
                .HasForeignKey(d => d.IdGrant)
                .HasConstraintName("partisipant___fk");

            entity.HasOne(d => d.IdScientNavigation).WithMany()
                .HasForeignKey(d => d.IdScient)
                .HasConstraintName("partisipant___fk_2");
        });

        modelBuilder.Entity<Scientist>(entity =>
        {
            entity.HasKey(e => e.IdScient).HasName("scientists_pkey");

            entity.ToTable("scientists");

            entity.Property(e => e.IdScient).HasColumnName("id_scient");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Degree)
                .HasMaxLength(45)
                .HasColumnName("degree");
            entity.Property(e => e.FioScient)
                .HasMaxLength(45)
                .HasColumnName("fio_scient");
            entity.Property(e => e.Title)
                .HasMaxLength(45)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
