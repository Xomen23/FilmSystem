using FilmSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmSystem.Infrastructure.Data
{
    public class FilmSystemContext : DbContext
    {
        public FilmSystemContext(DbContextOptions<FilmSystemContext> options) : base(options)
        {
        }

        public DbSet<Zanr> Zanrovi => Set<Zanr>();
        public DbSet<Film> Filmovi => Set<Film>();
        public DbSet<Sala> Sale => Set<Sala>();
        public DbSet<Sediste> Sedista => Set<Sediste>();
        public DbSet<Projekcija> Projekcije => Set<Projekcija>();
        public DbSet<Rezervacija> Rezervacije => Set<Rezervacija>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===== Zanr (1) - Film (M) =====
            // Film mora imati zanr (obavezan FK), zabranjeno brisanje zanra dok ima filmove
            modelBuilder.Entity<Film>()
                .HasOne(f => f.Zanr)
                .WithMany(z => z.Filmovi)
                .HasForeignKey(f => f.ZanrId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Sala (1) - Sediste (M) =====
            // Sediste ne postoji bez sale -> brisanjem sale brisu se i sedista
            modelBuilder.Entity<Sediste>()
                .HasOne(s => s.Sala)
                .WithMany(sa => sa.Sedista)
                .HasForeignKey(s => s.SalaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // ===== Film (1) - Projekcija (M) =====
            // zabranjeno brisanje filma dok ima zakazanih projekcija
            modelBuilder.Entity<Projekcija>()
                .HasOne(p => p.Film)
                .WithMany(f => f.Projekcije)
                .HasForeignKey(p => p.FilmId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Sala (1) - Projekcija (M) =====
            // zabranjeno brisanje sale dok ima zakazanih projekcija
            modelBuilder.Entity<Projekcija>()
                .HasOne(p => p.Sala)
                .WithMany(s => s.Projekcije)
                .HasForeignKey(p => p.SalaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Projekcija (1) - Rezervacija (M) =====
            // brisanjem projekcije brisu se i njene rezervacije
            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Projekcija)
                .WithMany(p => p.Rezervacije)
                .HasForeignKey(r => r.ProjekcijаId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // ===== Sediste (1) - Rezervacija (M) =====
            // zabranjeno brisanje sedista dok ima rezervacija (izbegava se i konflikt
            // "multiple cascade paths" u SQL Serveru, jer Projekcija vec kaskadno brise)
            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Sediste)
                .WithMany(s => s.Rezervacije)
                .HasForeignKey(r => r.SedisteId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // ===== Dodatna podesavanja kolona =====
            modelBuilder.Entity<Projekcija>()
                .Property(p => p.CenaKarte)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Film>()
                .Property(f => f.Naziv)
                .IsRequired()
                .HasMaxLength(300);

            modelBuilder.Entity<Zanr>()
                .Property(z => z.Naziv)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Sala>()
                .Property(s => s.Naziv)
                .IsRequired()
                .HasMaxLength(100);

            // ImdbId je jedinstven kad postoji (film moze biti dodat i bez OMDb podataka).
            // HasFilter je bitan: SQL Server inace dozvoljava samo JEDNU NULL vrednost
            // u unique indeksu, a nama ce vise filmova imati ImdbId = null.
            modelBuilder.Entity<Film>()
                .HasIndex(f => f.ImdbId)
                .IsUnique()
                .HasFilter("[ImdbId] IS NOT NULL");
        }
    }
}
