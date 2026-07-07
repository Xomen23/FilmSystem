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

            modelBuilder.Entity<Film>()
                .HasOne(f => f.Zanr)
                .WithMany(z => z.Filmovi)
                .HasForeignKey(f => f.ZanrId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Sediste>()
                .HasOne(s => s.Sala)
                .WithMany(sa => sa.Sedista)
                .HasForeignKey(s => s.SalaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Projekcija>()
                .HasOne(p => p.Film)
                .WithMany(f => f.Projekcije)
                .HasForeignKey(p => p.FilmId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Projekcija>()
                .HasOne(p => p.Sala)
                .WithMany(s => s.Projekcije)
                .HasForeignKey(p => p.SalaId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Projekcija)
                .WithMany(p => p.Rezervacije)
                .HasForeignKey(r => r.ProjekcijаId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

   
            modelBuilder.Entity<Rezervacija>()
                .HasOne(r => r.Sediste)
                .WithMany(s => s.Rezervacije)
                .HasForeignKey(r => r.SedisteId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

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

            modelBuilder.Entity<Film>()
                .HasIndex(f => f.ImdbId)
                .IsUnique()
                .HasFilter("[ImdbId] IS NOT NULL");
        }
    }
}
