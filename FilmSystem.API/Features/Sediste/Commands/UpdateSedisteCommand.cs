using FilmSystem.API.DTOs.Sediste;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Sediste.Commands
{
    public record UpdateSedisteCommand(int Id, SedisteUpdateDto Dto) : IRequest;

    public class UpdateSedisteHandler : IRequestHandler<UpdateSedisteCommand>
    {
        private readonly IUnitOfWork _uow;
        public UpdateSedisteHandler(IUnitOfWork uow) => _uow = uow;

        public Task Handle(UpdateSedisteCommand request, CancellationToken ct)
        {
            var sediste = _uow.Sedista.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Sediste sa Id {request.Id} ne postoji.");
            sediste.BrojReda = request.Dto.BrojReda;
            sediste.BrojMesta = request.Dto.BrojMesta;
            _uow.Sedista.Update(sediste);
            _uow.SaveChanges();
            return Task.CompletedTask;
        }
    }
}