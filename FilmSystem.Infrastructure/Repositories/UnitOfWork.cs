using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using FilmSystem.Infrastructure.Data;

namespace FilmSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FilmSystemContext _context;

        private IRepository<Zanr>? _zanrovi;
        private IFilmRepository? _filmovi;
        private IRepository<Sala>? _sale;
        private IRepository<Sediste>? _sedista;
        private IRepository<Projekcija>? _projekcije;
        private IRezervacijaRepository? _rezervacije;

        public UnitOfWork(FilmSystemContext context)
        {
            _context = context;
        }

        public IRepository<Zanr> Zanrovi => _zanrovi ??= new Repository<Zanr>(_context);

        public IFilmRepository Filmovi => _filmovi ??= new FilmRepository(_context);

        public IRepository<Sala> Sale => _sale ??= new Repository<Sala>(_context);

        public IRepository<Sediste> Sedista => _sedista ??= new Repository<Sediste>(_context);

        public IRepository<Projekcija> Projekcije => _projekcije ??= new Repository<Projekcija>(_context);

        public IRezervacijaRepository Rezervacije => _rezervacije ??= new RezervacijaRepository(_context);

        public int SaveChanges() => _context.SaveChanges();

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
