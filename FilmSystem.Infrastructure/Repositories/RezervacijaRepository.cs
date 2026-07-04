using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using FilmSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FilmSystem.Infrastructure.Repositories
{
    public class RezervacijaRepository : Repository<Rezervacija>, IRezervacijaRepository
    {
        public RezervacijaRepository(FilmSystemContext context) : base(context)
        {
        }

        public override IEnumerable<Rezervacija> GetAll() =>
            _dbSet.Include(r => r.Projekcija)
                  .Include(r => r.Sediste)
                  .ToList();

        public override Rezervacija? GetById(int id) =>
            _dbSet.Include(r => r.Projekcija)
                  .Include(r => r.Sediste)
                  .FirstOrDefault(r => r.Id == id);

        public IEnumerable<Rezervacija> GetByProjekcija(int projekcijaId) =>
            _dbSet.Include(r => r.Sediste)
                  .Where(r => r.ProjekcijаId == projekcijaId)
                  .ToList();

        // Vraca sedista u sali date projekcije koja NEMAJU aktivnu rezervaciju.
        // "Aktivna" = svaki status osim Otkazana/Istekla - ta sedista se smatraju
        // ponovo slobodnim (state machine logika iz Rezervacija kontrolera).
        public IEnumerable<Sediste> GetSlobodnaSedista(int projekcijaId)
        {
            var projekcija = _context.Projekcije
                .Include(p => p.Sala)
                    .ThenInclude(s => s.Sedista)
                .FirstOrDefault(p => p.Id == projekcijaId);

            if (projekcija == null)
                return Enumerable.Empty<Sediste>();

            var zauzetaSedistaIds = _context.Rezervacije
                .Where(r => r.ProjekcijаId == projekcijaId
                         && r.Status != StatusRezervacije.Otkazana
                         && r.Status != StatusRezervacije.Istekla)
                .Select(r => r.SedisteId)
                .ToHashSet();

            return projekcija.Sala.Sedista
                .Where(s => !zauzetaSedistaIds.Contains(s.Id))
                .ToList();
        }
    }
}
