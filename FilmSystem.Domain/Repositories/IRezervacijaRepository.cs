using FilmSystem.Domain.Models;

namespace FilmSystem.Domain.Repositories
{
    public interface IRezervacijaRepository : IRepository<Rezervacija>
    {
        IEnumerable<Rezervacija> GetByProjekcija(int projekcijaId);
        IEnumerable<Sediste> GetSlobodnaSedista(int projekcijaId);
    }
}