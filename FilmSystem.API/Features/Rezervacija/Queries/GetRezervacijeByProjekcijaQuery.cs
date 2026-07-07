using FilmSystem.API.DTOs.Rezervacija;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Rezervacija.Queries
{
    public record GetRezervacijeByProjekcijaQuery(int ProjekcijaId) : IRequest<IEnumerable<RezervacijaDto>>;

    public class GetRezervacijeByProjekcijaHandler : IRequestHandler<GetRezervacijeByProjekcijaQuery, IEnumerable<RezervacijaDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetRezervacijeByProjekcijaHandler(IUnitOfWork uow) => _uow = uow;

        public Task<IEnumerable<RezervacijaDto>> Handle(GetRezervacijeByProjekcijaQuery request, CancellationToken ct)
            => Task.FromResult(_uow.Rezervacije.GetByProjekcija(request.ProjekcijaId).Select(RezervacijaMapper.ToDto));
    }
}