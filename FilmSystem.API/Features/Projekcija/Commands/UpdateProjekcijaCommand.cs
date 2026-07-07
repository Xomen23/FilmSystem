using FilmSystem.API.DTOs.Projekcija;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Projekcija.Commands
{
    public record UpdateProjekcijaCommand(int Id, ProjekcijaUpdateDto Dto) : IRequest;

    public class UpdateProjekcijaHandler : IRequestHandler<UpdateProjekcijaCommand>
    {
        private readonly IUnitOfWork _uow;
        public UpdateProjekcijaHandler(IUnitOfWork uow) => _uow = uow;

        public Task Handle(UpdateProjekcijaCommand request, CancellationToken ct)
        {
            var p = _uow.Projekcije.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Projekcija sa Id {request.Id} ne postoji.");

            p.DatumVreme = request.Dto.DatumVreme;
            p.CenaKarte = request.Dto.CenaKarte;

            _uow.Projekcije.Update(p);
            _uow.SaveChanges();

            return Task.CompletedTask;
        }
    }
}