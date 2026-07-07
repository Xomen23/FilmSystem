using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using FilmSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FilmSystem.Infrastructure.Repositories
{
    public class FilmRepository : Repository<Film>, IFilmRepository
    {
        public FilmRepository(FilmSystemContext context) : base(context)
        {
        }

        public override IEnumerable<Film> GetAll() =>
            _dbSet.Include(f => f.Zanr).ToList();

        public override Film? GetById(int id) =>
            _dbSet.Include(f => f.Zanr).FirstOrDefault(f => f.Id == id);

        public IEnumerable<Film> GetByZanr(int zanrId) =>
            _dbSet.Include(f => f.Zanr)
                  .Where(f => f.ZanrId == zanrId)
                  .ToList();

        public IEnumerable<Film> GetByGodina(int godina) =>
            _dbSet.Include(f => f.Zanr)
                  .Where(f => f.Godina == godina)
                  .ToList();

        public Film? GetByImdbId(string imdbId) =>
            _dbSet.FirstOrDefault(f => f.ImdbId == imdbId);
    }
}
