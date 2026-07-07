using FilmSystem.API.DTOs.Rezervacija;
using FilmSystem.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FilmSystem.API.Features.Rezervacija.Queries
{
    public record GetAllRezervacijeQuery : IRequest<IEnumerable<RezervacijaDto>>;

    public class GetAllRezervacijeHandler : IRequestHandler<GetAllRezervacijeQuery, IEnumerable<RezervacijaDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllRezervacijeHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<IEnumerable<RezervacijaDto>> Handle(GetAllRezervacijeQuery request, CancellationToken ct)
        {
            // 1. Povlačimo sve rezervacije, projekcije i filmove iz uow-a u memoriju da ih povežemo
            var sveRezervacije = _uow.Rezervacije.GetAll().ToList();
            var sveProjekcije = _uow.Projekcije.GetAll().ToList();
            var sviFilmovi = _uow.Filmovi.GetAll().ToList();

            // 2. Ručno ih spajamo u petlji preko ID-jeva tako da objekti više ne budu null
            foreach (var rez in sveRezervacije)
            {
                // Pronalazimo projekciju za ovu rezervaciju
                // Izmeni liniju 29 da bude tačno ovako (sa tvojim unutrašnjim nazivom polja):
                rez.Projekcija = sveProjekcije.FirstOrDefault(p => p.Id == rez.ProjekcijаId);

                if (rez.Projekcija != null)
                {
                    // Pronalazimo film za tu projekciju
                    // NAPOMENA: Ako ti se polje na Projektu zove FilmId, promeni p.FilmId
                    rez.Projekcija.Film = sviFilmovi.FirstOrDefault(f => f.Id == rez.Projekcija.FilmId);
                }
            }

            // 3. Sada kada su svi objekti garantovano napunjeni, mapper će izvući pravi naziv!
            var dtos = sveRezervacije.Select(RezervacijaMapper.ToDto).ToList();

            return dtos;
        }
    }
}