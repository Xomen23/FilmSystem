using FilmSystem.API.DTOs.Projekcija;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Projekcija.Queries
{
    public record GetAllProjekcijeQuery : IRequest<IEnumerable<ProjekcijaDto>>;

    public class GetAllProjekcijeHandler : IRequestHandler<GetAllProjekcijeQuery, IEnumerable<ProjekcijaDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetAllProjekcijeHandler(IUnitOfWork uow) => _uow = uow;

        public Task<IEnumerable<ProjekcijaDto>> Handle(GetAllProjekcijeQuery request, CancellationToken ct)
        {
            var projekcije = _uow.Projekcije.GetAll();

            // I ovde menjamo direktan poziv sa lambdom p => ...
            var rezultat = projekcije.Select(p => ProjekcijaMapper.ToDto(p, _uow));

            return Task.FromResult(rezultat);
        }
    }
}