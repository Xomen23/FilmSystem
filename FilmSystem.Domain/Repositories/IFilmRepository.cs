using FilmSystem.Domain.Models;

namespace FilmSystem.Domain.Repositories
{
    public interface IFilmRepository : IRepository<Film>
    {
        IEnumerable<Film> GetByZanr(int zanrId);
        IEnumerable<Film> GetByGodina(int godina);
        Film? GetByImdbId(string imdbId);
    }
}