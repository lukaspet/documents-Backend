using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApi10Min.Models
{
    public partial class AuthDbContext : IdentityDbContext<Aspnetusers, Aspnetroles, string, Aspnetuserclaims, Aspnetuserroles, Aspnetuserlogins, Aspnetroleclaims, Aspnetusertokens>
    {
        public AuthDbContext()
        {
        }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aspnetroleclaims> Aspnetroleclaims { get; set; }
        public virtual DbSet<Aspnetroles> Aspnetroles { get; set; }
        public virtual DbSet<Aspnetuserclaims> Aspnetuserclaims { get; set; }
        public virtual DbSet<Aspnetuserlogins> Aspnetuserlogins { get; set; }
        public virtual DbSet<Aspnetuserroles> Aspnetuserroles { get; set; }
        public virtual DbSet<Aspnetusers> Aspnetusers { get; set; }
        public virtual DbSet<Aspnetusertokens> Aspnetusertokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aspnetroleclaims>(entity =>
            {
                entity.ToTable("aspnetroleclaims");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IdentityRoles_Claims");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ClaimValue).HasColumnType("longtext");

                entity.Property(e => e.Claimtype).HasColumnType("longtext");

                entity.Property(e => e.RoleId)
                    .IsRequired()
                    .HasColumnType("varchar(128)");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Aspnetroleclaims)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("IdentityRoles_Claims");
            });

            modelBuilder.Entity<Aspnetroles>(entity =>
            {
                entity.ToTable("aspnetroles");

                entity.Property(e => e.Id).HasColumnType("varchar(128)");

                entity.Property(e => e.ConcurrencyStamp).HasColumnType("longtext");

                entity.Property(e => e.Name).HasColumnType("varchar(256)");

                entity.Property(e => e.NormalizedName).HasColumnType("varchar(256)");

                entity.Property(e => e.Description).HasColumnType("longtext");
            });

            modelBuilder.Entity<Aspnetuserclaims>(entity =>
            {
                entity.ToTable("aspnetuserclaims");

                entity.HasIndex(e => e.UserId)
                    .HasName("UserId");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.ClaimType).HasColumnType("longtext");

                entity.Property(e => e.ClaimValue).HasColumnType("longtext");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnType("varchar(128)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Aspnetuserclaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Application_Claims");
            });

            modelBuilder.Entity<Aspnetuserlogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId });

                entity.ToTable("aspnetuserlogins");

                entity.HasIndex(e => e.UserId)
                    .HasName("ApplicationUser_Logins");

                entity.Property(e => e.LoginProvider).HasColumnType("varchar(128)");

                entity.Property(e => e.ProviderKey).HasColumnType("varchar(128)");

                entity.Property(e => e.UserId).HasColumnType("varchar(128)");

                entity.Property(e => e.ProviderDisplayName).HasColumnType("longtext");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Aspnetuserlogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("ApplicationUser_Logins");
            });

            modelBuilder.Entity<Aspnetuserroles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("aspnetuserroles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("IdentityRole_Users");

                entity.Property(e => e.UserId).HasColumnType("varchar(128)");

                entity.Property(e => e.RoleId).HasColumnType("varchar(128)");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Aspnetuserroles)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("IdentityRole_Users");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Aspnetuserroles)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("ApplicationUser_Roles");
            });

            modelBuilder.Entity<Aspnetusers>(entity =>
            {
                entity.ToTable("aspnetusers");

                entity.Property(e => e.Id).HasColumnType("varchar(128)");

                entity.Property(e => e.AccessFailedCount).HasColumnType("int(11)");

                entity.Property(e => e.ConcurrencyStamp).HasColumnType("longtext");

                entity.Property(e => e.Email).HasColumnType("varchar(256)");

                entity.Property(e => e.EmailConfirmed);

                entity.Property(e => e.FullName).HasColumnType("varchar(50)");

                entity.Property(e => e.LockoutEnabled);

                entity.Property(e => e.LockoutEnd).HasColumnType("datetime");

                entity.Property(e => e.NormalizedEmail).HasColumnType("varchar(256)");

                entity.Property(e => e.NormalizedUserName).HasColumnType("varchar(256)");

                entity.Property(e => e.PasswordHash).HasColumnType("longtext");

                entity.Property(e => e.PhoneNumber).HasColumnType("longtext");

                entity.Property(e => e.PhoneNumberConfirmed);

                entity.Property(e => e.SecurityStamp).HasColumnType("longtext");

                entity.Property(e => e.TwoFactorEnabled);

                entity.Property(e => e.UserName).HasColumnType("varchar(256)");
            });

            modelBuilder.Entity<Aspnetusertokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.ToTable("aspnetusertokens");

                entity.Property(e => e.UserId).HasColumnType("varchar(450)");

                entity.Property(e => e.LoginProvider).HasColumnType("varchar(128)");

                entity.Property(e => e.Name).HasColumnType("varchar(128)");

                entity.Property(e => e.Value).HasColumnType("longtext");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Aspnetusertokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("AspNetUsersTokens_Users");
            });
        }
    }
}
