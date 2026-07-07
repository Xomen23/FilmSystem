using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Film.Commands
{
    public record DeleteFilmCommand(int Id) : IRequest;

    public class DeleteFilmHandler : IRequestHandler<DeleteFilmCommand>
    {
        private readonly IUnitOfWork _uow;
        public DeleteFilmHandler(IUnitOfWork uow) => _uow = uow;

        public Task Handle(DeleteFilmCommand request, CancellationToken ct)
        {
            var film = _uow.Filmovi.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Film sa Id {request.Id} ne postoji.");

            if (_uow.Projekcije.Find(p => p.FilmId == request.Id).Any())
                throw new InvalidOperationException($"Film sa Id {request.Id} ne moze biti obrisan jer ima zakazane projekcije.");

            _uow.Filmovi.Remove(film);
            _uow.SaveChanges();
            return Task.CompletedTask;
        }
    }
}