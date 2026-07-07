using FilmSystem.API.DTOs.Projekcija;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Projekcija.Queries
{
    public record GetProjekcijaByIdQuery(int Id) : IRequest<ProjekcijaDto?>;

    public class GetProjekcijaByIdHandler : IRequestHandler<GetProjekcijaByIdQuery, ProjekcijaDto?>
    {
        private readonly IUnitOfWork _uow;
        public GetProjekcijaByIdHandler(IUnitOfWork uow) => _uow = uow;

        public Task<ProjekcijaDto?> Handle(GetProjekcijaByIdQuery request, CancellationToken ct)
        {
            var p = _uow.Projekcije.GetById(request.Id);
            return Task.FromResult(p == null ? null : ProjekcijaMapper.ToDto(p , _uow));
        }
    }
}