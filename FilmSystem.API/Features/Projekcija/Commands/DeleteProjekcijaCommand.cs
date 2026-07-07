using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Projekcija.Commands
{
    public record DeleteProjekcijaCommand(int Id) : IRequest;

    public class DeleteProjekcijaHandler : IRequestHandler<DeleteProjekcijaCommand>
    {
        private readonly IUnitOfWork _uow;
        public DeleteProjekcijaHandler(IUnitOfWork uow) => _uow = uow;

        public Task Handle(DeleteProjekcijaCommand request, CancellationToken ct)
        {
            var p = _uow.Projekcije.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Projekcija sa Id {request.Id} ne postoji.");
            _uow.Projekcije.Remove(p);
            _uow.SaveChanges();
            return Task.CompletedTask;
        }
    }
}