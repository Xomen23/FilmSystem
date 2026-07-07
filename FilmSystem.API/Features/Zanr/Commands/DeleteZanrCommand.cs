using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Zanr.Commands
{
    public record DeleteZanrCommand(int Id) : IRequest;

    public class DeleteZanrHandler : IRequestHandler<DeleteZanrCommand>
    {
        private readonly IUnitOfWork _uow;
        public DeleteZanrHandler(IUnitOfWork uow) => _uow = uow;

        public Task Handle(DeleteZanrCommand request, CancellationToken ct)
        {
            var zanr = _uow.Zanrovi.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Zanr sa Id {request.Id} ne postoji.");

            if (_uow.Filmovi.Find(f => f.ZanrId == request.Id).Any())
                throw new InvalidOperationException($"Zanr sa Id {request.Id} ne moze biti obrisan jer ima filmove.");

            _uow.Zanrovi.Remove(zanr);
            _uow.SaveChanges();
            return Task.CompletedTask;
        }
    }
}