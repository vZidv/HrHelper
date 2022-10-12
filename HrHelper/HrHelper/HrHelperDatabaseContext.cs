using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HrHelper
{
    public partial class HrHelperDatabaseContext : DbContext
    {
        //Scaffold-DbContext "Data Source=DESKTOP-2BSAL1V\SQL;Database=HrHelperDatabase;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer
        public HrHelperDatabaseContext()
        {
        }


        public virtual DbSet<AuthorizationUser> AuthorizationUsers { get; set; } = null!;

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
