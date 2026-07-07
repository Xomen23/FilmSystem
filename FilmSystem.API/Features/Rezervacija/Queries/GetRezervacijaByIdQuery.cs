using FilmSystem.API.DTOs.Rezervacija;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Rezervacija.Queries
{
    public record GetRezervacijaByIdQuery(int Id) : IRequest<RezervacijaDto?>;

    public class GetRezervacijaByIdHandler : IRequestHandler<GetRezervacijaByIdQuery, RezervacijaDto?>
    {
        private readonly IUnitOfWork _uow;
        public GetRezervacijaByIdHandler(IUnitOfWork uow) => _uow = uow;

        public Task<RezervacijaDto?> Handle(GetRezervacijaByIdQuery request, CancellationToken ct)
        {
            var r = _uow.Rezervacije.GetById(request.Id);
            return Task.FromResult(r == null ? null : RezervacijaMapper.ToDto(r));
        }
    }
}