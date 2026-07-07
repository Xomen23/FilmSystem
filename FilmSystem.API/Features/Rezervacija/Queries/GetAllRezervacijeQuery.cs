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
     
            var sveRezervacije = _uow.Rezervacije.GetAll().ToList();
            var sveProjekcije = _uow.Projekcije.GetAll().ToList();
            var sviFilmovi = _uow.Filmovi.GetAll().ToList();

         
            foreach (var rez in sveRezervacije)
            {
                
                rez.Projekcija = sveProjekcije.FirstOrDefault(p => p.Id == rez.ProjekcijаId);

                if (rez.Projekcija != null)
                {
           
                    rez.Projekcija.Film = sviFilmovi.FirstOrDefault(f => f.Id == rez.Projekcija.FilmId);
                }
            }

        
            var dtos = sveRezervacije.Select(RezervacijaMapper.ToDto).ToList();

            return dtos;
        }
    }
}