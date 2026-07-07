using FilmSystem.API.DTOs.Sediste;
using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Sediste.Queries
{
    public record GetSematskiPrikazQuery(int SalaId, int ProjekcijaId) : IRequest<IEnumerable<SedisteStatusDto>>;

    public class GetSematskiPrikazHandler : IRequestHandler<GetSematskiPrikazQuery, IEnumerable<SedisteStatusDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetSematskiPrikazHandler(IUnitOfWork uow) => _uow = uow;

        public Task<IEnumerable<SedisteStatusDto>> Handle(GetSematskiPrikazQuery request, CancellationToken ct)
        {
            var projekcija = _uow.Projekcije.GetById(request.ProjekcijaId)
                ?? throw new KeyNotFoundException($"Projekcija sa Id {request.ProjekcijaId} ne postoji.");

            if (projekcija.SalaId != request.SalaId)
                throw new InvalidOperationException("Ta projekcija se ne odrzava u navedenoj sali.");

            var sviSedista = _uow.Sedista
                .Find(s => s.SalaId == request.SalaId)
                .OrderBy(s => s.BrojReda).ThenBy(s => s.BrojMesta)
                .ToList();

            var zauzetaIds = _uow.Rezervacije
                .GetByProjekcija(request.ProjekcijaId)
                .Where(r => r.Status != StatusRezervacije.Otkazana && r.Status != StatusRezervacije.Istekla)
                .Select(r => r.SedisteId)
                .ToHashSet();

            var rezultat = sviSedista.Select(s => new SedisteStatusDto
            {
                Id = s.Id,
                BrojReda = s.BrojReda,
                BrojMesta = s.BrojMesta,
                Slobodno = !zauzetaIds.Contains(s.Id)
            });

            return Task.FromResult(rezultat);
        }
    }
}