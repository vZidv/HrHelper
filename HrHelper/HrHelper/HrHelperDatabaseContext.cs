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
        public virtual DbSet<Education> Educations { get; set; } = null!;
        public virtual DbSet<Photo> Photos { get; set; } = null!;
        public virtual DbSet<Summary> Summaries { get; set; } = null!;
        public virtual DbSet<SummaryContact> SummaryContacts { get; set; } = null!;
        public virtual DbSet<SummaryForVacancy> SummaryForVacancies { get; set; } = null!;
        public virtual DbSet<SummaryStatus> SummaryStatuses { get; set; } = null!;
        public virtual DbSet<UserType> UserTypes { get; set; } = null!;
        public virtual DbSet<Vacancy> Vacancies { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //localhost\SQLEXPRESS
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-2BSAL1V\\SQL;Database=HrHelperDatabase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorizationUser>(entity =>
            {
                entity.ToTable("AuthorizationUser");

                entity.HasIndex(e => e.Type, "IX_AuthorizationUser_Type");

                entity.Property(e => e.Login).HasMaxLength(30);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.AuthorizationUsers)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AuthorizationUser_UserType");
            });

            modelBuilder.Entity<Busyness>(entity =>
            {
                entity.ToTable("Busyness");

                entity.Property(e => e.Type).HasMaxLength(25);
            });

            modelBuilder.Entity<Education>(entity =>
            {
                entity.ToTable("Education");

                entity.Property(e => e.EducationName).HasMaxLength(60);
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.ToTable("Photo");

                entity.Property(e => e.Path).HasMaxLength(150);
            });

            modelBuilder.Entity<Summary>(entity =>
            {
                entity.ToTable("Summary");

                entity.HasIndex(e => e.BusynessId, "IX_Summary_BusynessId");

                entity.HasIndex(e => e.ContactsId, "IX_Summary_ContactsId");

                entity.HasIndex(e => e.EducationId, "IX_Summary_EducationId");

                entity.HasIndex(e => e.PhotoId, "IX_Summary_PhotoId");

                entity.HasIndex(e => e.StatusId, "IX_Summary_Status");

                entity.Property(e => e.Address).HasMaxLength(70);

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.Comments).HasMaxLength(200);

                entity.Property(e => e.EducationInstution).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(25);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.LastCompany).HasMaxLength(50);

                entity.Property(e => e.LastJobTitle).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(25);

                entity.Property(e => e.Patronymic).HasMaxLength(25);

                entity.Property(e => e.Town).HasMaxLength(40);

                entity.HasOne(d => d.Busyness)
                    .WithMany(p => p.Summaries)
                    .HasForeignKey(d => d.BusynessId)
                    .HasConstraintName("FK_Summary_Busyness");

                entity.HasOne(d => d.Contacts)
                    .WithMany(p => p.Summaries)
                    .HasForeignKey(d => d.ContactsId)
                    .HasConstraintName("FK_Summary_SummaryContacts");

                entity.HasOne(d => d.Education)
                    .WithMany(p => p.Summaries)
                    .HasForeignKey(d => d.EducationId)
                    .HasConstraintName("FK_Summary_Education");

                entity.HasOne(d => d.Photo)
                    .WithMany(p => p.Summaries)
                    .HasForeignKey(d => d.PhotoId)
                    .HasConstraintName("FK_Summary_Photo");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Summaries)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Summary_SummaryStatus");
            });

            modelBuilder.Entity<SummaryContact>(entity =>
            {
                entity.Property(e => e.Email).HasMaxLength(30);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.Skype).HasMaxLength(50);
            });

            modelBuilder.Entity<SummaryForVacancy>(entity =>
            {
                entity.ToTable("SummaryForVacancy");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.SummaryForVacancies)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SummaryForVacancy_Vacancy");

                entity.HasOne(d => d.Summary)
                    .WithMany(p => p.SummaryForVacancies)
                    .HasForeignKey(d => d.SummaryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SummaryForVacancy_Summary");
            });

            modelBuilder.Entity<SummaryStatus>(entity =>
            {
                entity.ToTable("SummaryStatus");

                entity.Property(e => e.Status).HasMaxLength(20);
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.ToTable("UserType");

                entity.Property(e => e.Type).HasMaxLength(20);
            });

            modelBuilder.Entity<Vacancy>(entity =>
            {
                entity.ToTable("Vacancy");

                entity.Property(e => e.JobTitle).HasMaxLength(60);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
