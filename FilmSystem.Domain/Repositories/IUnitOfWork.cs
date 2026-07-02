using FilmSystem.Domain.Models;

namespace FilmSystem.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Zanr> Zanrovi { get; }
        IFilmRepository Filmovi { get; }
        IRepository<Sala> Sale { get; }
        IRepository<Sediste> Sedista { get; }
        IRepository<Projekcija> Projekcije { get; }
        IRezervacijaRepository Rezervacije { get; }

        int SaveChanges();
    }
}