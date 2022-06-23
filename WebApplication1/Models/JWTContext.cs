using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Models
{
    public partial class JWTContext : DbContext
    {
        public JWTContext()
        {
        }

        public JWTContext(DbContextOptions<JWTContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplication1.Models.User> Users { get; set; }
       // public virtual DbSet<User> Users { get; set; } = null!;

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=JWT;Trusted_Connection=True;");
//            }
//        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>(entity =>
        //    {
        //        entity.HasKey(e => e.Username)
        //            .HasName("PK__Users__536C85E54AB2F163");

        //        entity.Property(e => e.Username)
        //            .HasMaxLength(30)
        //            .IsUnicode(false);

        //        entity.Property(e => e.Email)
        //            .HasMaxLength(50)
        //            .IsUnicode(false);

        //        entity.Property(e => e.FirstName)
        //            .HasMaxLength(25)
        //            .IsUnicode(false);

        //        entity.Property(e => e.LastName)
        //            .HasMaxLength(25)
        //            .IsUnicode(false);

        //        entity.Property(e => e.Pass)
        //            .HasMaxLength(50)
        //            .IsUnicode(false)
        //            .HasColumnName("pass");
        //    });

        //    OnModelCreatingPartial(modelBuilder);
        //}

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
