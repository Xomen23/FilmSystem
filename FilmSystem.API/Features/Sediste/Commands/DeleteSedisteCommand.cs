using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Sediste.Commands
{
    public record DeleteSedisteCommand(int Id) : IRequest;

    public class DeleteSedisteHandler : IRequestHandler<DeleteSedisteCommand>
    {
        private readonly IUnitOfWork _uow;
        public DeleteSedisteHandler(IUnitOfWork uow) => _uow = uow;

        public Task Handle(DeleteSedisteCommand request, CancellationToken ct)
        {
            var sediste = _uow.Sedista.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Sediste sa Id {request.Id} ne postoji.");
            _uow.Sedista.Remove(sediste);
            _uow.SaveChanges();
            return Task.CompletedTask;
        }
    }
}