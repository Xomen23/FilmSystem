using FilmSystem.API.DTOs.Rezervacija;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Rezervacija.Queries
{
    public record GetAllRezervacijeQuery : IRequest<IEnumerable<RezervacijaDto>>;

    public class GetAllRezervacijeHandler : IRequestHandler<GetAllRezervacijeQuery, IEnumerable<RezervacijaDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllRezervacijeHandler(IUnitOfWork uow) => _uow = uow;

        public Task<IEnumerable<RezervacijaDto>> Handle(GetAllRezervacijeQuery request, CancellationToken ct)
            => Task.FromResult(_uow.Rezervacije.GetAll().Select(RezervacijaMapper.ToDto));
    }
}