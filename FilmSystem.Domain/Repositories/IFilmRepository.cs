using FilmSystem.Domain.Models;

namespace FilmSystem.Domain.Repositories
{
    public interface IFilmRepository : IRepository<Film>
    {
        // specificne metode za Film koje ne pokriva genericki IRepository
        IEnumerable<Film> GetByZanr(int zanrId);
        IEnumerable<Film> GetByGodina(int godina);
        Film? GetByImdbId(string imdbId);
    }
}