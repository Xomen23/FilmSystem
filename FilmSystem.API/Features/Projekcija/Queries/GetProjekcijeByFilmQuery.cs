using FilmSystem.API.DTOs.Projekcija;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Projekcija.Queries
{
    public record GetProjekcijeByFilmQuery(int FilmId) : IRequest<IEnumerable<ProjekcijaDto>>;

    public class GetProjekcijeByFilmHandler : IRequestHandler<GetProjekcijeByFilmQuery, IEnumerable<ProjekcijaDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetProjekcijeByFilmHandler(IUnitOfWork uow) => _uow = uow;

        public Task<IEnumerable<ProjekcijaDto>> Handle(GetProjekcijeByFilmQuery request, CancellationToken ct)
        {
            var projekcije = _uow.Projekcije.Find(p => p.FilmId == request.FilmId);

            // Eksplicitno prosleđujemo p i _uow u mapper unutar Select-a
            var rezultat = projekcije.Select(p => ProjekcijaMapper.ToDto(p, _uow));

            return Task.FromResult(rezultat);
        }
    }
}