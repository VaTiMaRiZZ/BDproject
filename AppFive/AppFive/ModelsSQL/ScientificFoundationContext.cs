using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace AppFive.ModelsSQL;

public partial class ScientificFoundationContext : DbContext
{
    public ScientificFoundationContext()
    {
    }

    public ScientificFoundationContext(DbContextOptions<ScientificFoundationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Direction> Directions { get; set; }

    public virtual DbSet<Grant> Grants { get; set; }

    public virtual DbSet<Partisipant> Partisipants { get; set; }

    public virtual DbSet<Scientist> Scientists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=scientific_foundation", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.45-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Direction>(entity =>
        {
            entity.HasKey(e => e.IdDirection).HasName("PRIMARY");

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
            entity.HasKey(e => new { e.IdGrants, e.IdScient, e.IdDirection })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("grants");

            entity.HasIndex(e => e.IdScient, "frg_key_one_idx");

            entity.HasIndex(e => e.IdDirection, "frg_key_two_idx");

            entity.Property(e => e.IdGrants).HasColumnName("id_grants");
            entity.Property(e => e.IdScient).HasColumnName("id_scient");
            entity.Property(e => e.IdDirection)
                .HasMaxLength(45)
                .HasColumnName("id_direction");
            entity.Property(e => e.DateEnd)
                .HasColumnType("datetime")
                .HasColumnName("date_end");
            entity.Property(e => e.DateStart)
                .HasColumnType("datetime")
                .HasColumnName("date_start");
            entity.Property(e => e.NameTheme)
                .HasMaxLength(45)
                .HasColumnName("name_theme");
            entity.Property(e => e.Organization)
                .HasMaxLength(45)
                .HasColumnName("organization");
            entity.Property(e => e.Summa).HasColumnName("summa");

            entity.HasOne(d => d.IdDirectionNavigation).WithMany(p => p.Grants)
                .HasForeignKey(d => d.IdDirection)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("frg_key_two");

            entity.HasOne(d => d.IdScientNavigation).WithMany(p => p.Grants)
                .HasForeignKey(d => d.IdScient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("frg_key_one");
        });

        modelBuilder.Entity<Partisipant>(entity =>
        {
            entity.HasKey(e => new { e.IdGrant, e.IdScient })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("partisipant");

            entity.HasIndex(e => e.IdScient, "frg_parti_two_idx");

            entity.Property(e => e.IdGrant).HasColumnName("id_grant");
            entity.Property(e => e.IdScient).HasColumnName("id_scient");

            entity.HasOne(d => d.IdScientNavigation).WithMany(p => p.Partisipants)
                .HasForeignKey(d => d.IdScient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("frg_parti_two");
        });

        modelBuilder.Entity<Scientist>(entity =>
        {
            entity.HasKey(e => e.IdScient).HasName("PRIMARY");

            entity.ToTable("scientists");

            entity.Property(e => e.IdScient)
                .ValueGeneratedNever()
                .HasColumnName("id_scient");
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
