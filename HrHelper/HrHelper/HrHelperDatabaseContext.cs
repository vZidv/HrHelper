using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HrHelper
{
    public partial class HrHelperDatabaseContext : DbContext
    {
        public HrHelperDatabaseContext()
        {
        }

        public HrHelperDatabaseContext(DbContextOptions<HrHelperDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AuthorizationUser> AuthorizationUsers { get; set; } = null!;
        public virtual DbSet<Busyness> Busynesses { get; set; } = null!;
        public virtual DbSet<Photo> Photos { get; set; } = null!;
        public virtual DbSet<Summary> Summaries { get; set; } = null!;
        public virtual DbSet<SummaryStatus> SummaryStatuses { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-2BSAL1V\\SQL;Database=HrHelperDatabase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorizationUser>(entity =>
            {
                entity.ToTable("AuthorizationUser");

                entity.Property(e => e.Login).HasMaxLength(30);

                entity.Property(e => e.Password).HasMaxLength(50);
            });

            modelBuilder.Entity<Busyness>(entity =>
            {
                entity.ToTable("Busyness");

                entity.Property(e => e.Type).HasMaxLength(25);
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("Photo");

                entity.Property(e => e.Path).HasMaxLength(150);
            });

            modelBuilder.Entity<Summary>(entity =>
            {
                entity.ToTable("Summary");

                entity.Property(e => e.Address).HasMaxLength(70);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Comments).HasMaxLength(200);

                entity.Property(e => e.Education).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(25);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.JobTitle).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(25);

                entity.Property(e => e.Patronymic).HasMaxLength(25);

                entity.Property(e => e.Phone).HasMaxLength(12);

                entity.Property(e => e.Specialization).HasMaxLength(50);

                entity.Property(e => e.Town).HasMaxLength(40);

                entity.HasOne(d => d.Busyness)
                    .WithMany(p => p.Summaries)
                    .HasForeignKey(d => d.BusynessId)
                    .HasConstraintName("FK_Summary_Busyness");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.Summaries)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK_Summary_Photo");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Summaries)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Summary_SummaryStatus");
            });

            modelBuilder.Entity<SummaryStatus>(entity =>
            {
                entity.ToTable("SummaryStatus");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Status).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
