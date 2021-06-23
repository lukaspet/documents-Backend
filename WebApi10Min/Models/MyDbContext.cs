using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApi10Min.Models
{
    public partial class MyDbContext : DbContext
    {
        public MyDbContext()
        {
        }

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categoria> Categoria { get; set; }
        public virtual DbSet<DocFile> DocFile { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<LogBackend> LogBackend { get; set; }
        public virtual DbSet<LogFrontend> LogFrontend { get; set; }
        public virtual DbSet<Parte2> Parte2 { get; set; }
        public virtual DbSet<Parte3> Parte3 { get; set; }
        public virtual DbSet<Societa> Societa { get; set; }
        public virtual DbSet<Sottocategoria> Sottocategoria { get; set; }
        public virtual DbSet<Ufficio> Ufficio { get; set; }
        public virtual DbSet<UserSubscribe> UserSubscribe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("categoria");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DescrizioneCategoria)
                    .HasColumnName("descrizione_categoria")
                    .HasColumnType("longtext");

                entity.Property(e => e.NomeCategoria)
                    .IsRequired()
                    .HasColumnName("nome_categoria")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<DocFile>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.ToTable("doc_file");

                entity.Property(e => e.FileId)
                    .HasColumnName("file_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DocumentId)
                    .HasColumnName("document_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FileName)
                    .HasColumnName("file_name")
                    .HasColumnType("varchar(128)");

                entity.Property(e => e.Master)
                    .HasColumnName("master")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Path)
                    .HasColumnName("path")
                    .HasColumnType("longtext");

                entity.Property(e => e.Position)
                    .HasColumnName("position")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.UniquefileName)
                    .HasColumnName("uniquefile_name")
                    .HasColumnType("varchar(256)");
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("document");

                entity.HasIndex(e => e.CategoriaId)
                    .HasName("document_catetgory");

                entity.HasIndex(e => e.DescrizioneDocumento)
                    .HasName("descrizione_documento");

                entity.HasIndex(e => e.SocietaId)
                    .HasName("document_societa");

                entity.HasIndex(e => e.SottocategoriaId)
                    .HasName("document_undercategory");

                entity.HasIndex(e => e.UfficioId)
                    .HasName("document_ufficio");

                entity.Property(e => e.DocumentId)
                    .HasColumnName("document_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cartella)
                    .HasColumnName("cartella")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.CategoriaId)
                    .HasColumnName("categoria_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DataArchivio)
                    .HasColumnName("data_archivio")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataDocumento)
                    .HasColumnName("data_documento")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataScadenza)
                    .HasColumnName("data_scadenza")
                    .HasColumnType("date");

                entity.Property(e => e.DescrizioneDocumento)
                    .HasColumnName("descrizione_documento")
                    .HasColumnType("longtext");

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasColumnType("longtext");

                entity.Property(e => e.Parte2Id)
                    .HasColumnName("parte2_id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.Parte3Id)
                    .HasColumnName("parte3_id")
                    .HasDefaultValueSql("'0'");

                entity.Property(e => e.PosizioneCartella)
                    .HasColumnName("posizione_cartella")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.SocietaId)
                    .HasColumnName("societa_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.SottocategoriaId)
                    .HasColumnName("sottocategoria_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UfficioId)
                    .HasColumnName("ufficio_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Document)
                    .HasForeignKey(d => d.CategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_catetgory");

                entity.HasOne(d => d.Societa)
                    .WithMany(p => p.Document)
                    .HasForeignKey(d => d.SocietaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_societa");

                entity.HasOne(d => d.Sottocategoria)
                    .WithMany(p => p.Document)
                    .HasForeignKey(d => d.SottocategoriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_undercategory");

                entity.HasOne(d => d.Ufficio)
                    .WithMany(p => p.Document)
                    .HasForeignKey(d => d.UfficioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("document_ufficio");
            });

            modelBuilder.Entity<LogBackend>(entity =>
            {
                entity.HasKey(e => e.IdLog);

                entity.ToTable("log_backend");

                entity.Property(e => e.IdLog)
                    .HasColumnName("id_log")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Datetime)
                    .HasColumnName("datetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.EventType)
                    .HasColumnName("event_type")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("longtext");

                entity.Property(e => e.User)
                    .HasColumnName("user")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.UserIp)
                    .HasColumnName("user_ip")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<LogFrontend>(entity =>
            {
                entity.HasKey(e => e.IdLog);

                entity.ToTable("log_frontend");

                entity.Property(e => e.IdLog)
                    .HasColumnName("id_log")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DatetimeBackend)
                    .HasColumnName("datetime_backend")
                    .HasColumnType("datetime");

                entity.Property(e => e.DatetimeFrontend)
                    .HasColumnName("datetime_frontend")
                    .HasColumnType("datetime");

                entity.Property(e => e.DatetimeInsert)
                    .HasColumnName("datetime_insert")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.EventType)
                    .HasColumnName("event_type")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Message)
                    .HasColumnName("message")
                    .HasColumnType("longtext");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.User)
                    .HasColumnName("user")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.UserIp)
                    .HasColumnName("user_ip")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Parte2>(entity =>
            {
                entity.ToTable("parte2");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NomeParte2)
                    .HasColumnName("nome_parte2")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Parte3>(entity =>
            {
                entity.ToTable("parte3");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NomeParte3)
                    .HasColumnName("nome_parte3")
                    .HasColumnType("varchar(50)");
            });

            modelBuilder.Entity<Societa>(entity =>
            {
                entity.ToTable("societa");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NomeSocieta)
                    .IsRequired()
                    .HasColumnName("nome_societa")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<Sottocategoria>(entity =>
            {
                entity.ToTable("sottocategoria");

                entity.HasIndex(e => e.CategoriaId)
                    .HasName("FK_sottocategoria_categoria");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoriaId)
                    .HasColumnName("categoria_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DescrizioneSottocategoria)
                    .HasColumnName("descrizione_sottocategoria")
                    .HasColumnType("longtext");

                entity.Property(e => e.NomeSottocategoria)
                    .IsRequired()
                    .HasColumnName("nome_sottocategoria")
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Sottocategoria)
                    .HasForeignKey(d => d.CategoriaId)
                    .HasConstraintName("FK_sottocategoria_categoria");
            });

            modelBuilder.Entity<Ufficio>(entity =>
            {
                entity.ToTable("ufficio");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.NomeUfficio)
                    .IsRequired()
                    .HasColumnName("nome_ufficio")
                    .HasColumnType("varchar(100)");
            });

            modelBuilder.Entity<UserSubscribe>(entity =>
            {
                entity.ToTable("user_subscribe");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DocumentId)
                    .HasColumnName("document_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("user_id")
                    .HasColumnType("varchar(128)");
            });
        }
    }
}
