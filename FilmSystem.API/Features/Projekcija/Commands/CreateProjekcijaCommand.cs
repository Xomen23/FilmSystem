using FilmSystem.API.DTOs.Projekcija;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Projekcija.Commands
{
    public record CreateProjekcijaCommand(ProjekcijaCreateDto Dto) : IRequest<ProjekcijaDto>;

    public class CreateProjekcijaHandler : IRequestHandler<CreateProjekcijaCommand, ProjekcijaDto>
    {
        private readonly IUnitOfWork _uow;
        public CreateProjekcijaHandler(IUnitOfWork uow) => _uow = uow;

        public Task<ProjekcijaDto> Handle(CreateProjekcijaCommand request, CancellationToken ct)
        {
            var p = new Domain.Models.Projekcija
            {
                DatumVreme = request.Dto.DatumVreme,
                CenaKarte = request.Dto.CenaKarte,
                FilmId = request.Dto.FilmId,
                SalaId = request.Dto.SalaId
            };
            _uow.Projekcije.Add(p);
            _uow.SaveChanges();
            return Task.FromResult(ProjekcijaMapper.ToDto(p, _uow));
        }
    }
}