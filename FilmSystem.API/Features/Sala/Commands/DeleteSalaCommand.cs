using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Sala.Commands
{
    public record DeleteSalaCommand(int Id) : IRequest;

    public class DeleteSalaHandler : IRequestHandler<DeleteSalaCommand>
    {
        private readonly IUnitOfWork _uow;
        public DeleteSalaHandler(IUnitOfWork uow) => _uow = uow;

        public Task Handle(DeleteSalaCommand request, CancellationToken ct)
        {
            var sala = _uow.Sale.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Sala sa Id {request.Id} ne postoji.");

            if (_uow.Projekcije.Find(p => p.SalaId == request.Id).Any())
                throw new InvalidOperationException($"Sala sa Id {request.Id} ne moze biti obrisana jer ima projekcije.");

            _uow.Sale.Remove(sala);
            _uow.SaveChanges();
            return Task.CompletedTask;
        }
    }
}