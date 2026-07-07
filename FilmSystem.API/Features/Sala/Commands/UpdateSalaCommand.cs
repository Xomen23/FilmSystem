using FilmSystem.API.DTOs.Sala;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Sala.Commands
{
    public record UpdateSalaCommand(int Id, SalaUpdateDto Dto) : IRequest;

    public class UpdateSalaHandler : IRequestHandler<UpdateSalaCommand>
    {
        private readonly IUnitOfWork _uow;
        public UpdateSalaHandler(IUnitOfWork uow) => _uow = uow;

        public Task Handle(UpdateSalaCommand request, CancellationToken ct)
        {
            var sala = _uow.Sale.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Sala sa Id {request.Id} ne postoji.");

            sala.Naziv = request.Dto.Naziv;

            _uow.Sale.Update(sala);
            _uow.SaveChanges();

            return Task.CompletedTask;
        }
    }
}