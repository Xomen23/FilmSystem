using FilmSystem.API.DTOs.Zanr;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Zanr.Commands
{
    public record UpdateZanrCommand(int Id, ZanrUpdateDto Dto) : IRequest;

    public class UpdateZanrHandler : IRequestHandler<UpdateZanrCommand>
    {
        private readonly IUnitOfWork _uow;
        public UpdateZanrHandler(IUnitOfWork uow) => _uow = uow;

        public Task Handle(UpdateZanrCommand request, CancellationToken ct)
        {
            var zanr = _uow.Zanrovi.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Zanr sa Id {request.Id} ne postoji.");
            zanr.Naziv = request.Dto.Naziv;
            _uow.Zanrovi.Update(zanr);
            _uow.SaveChanges();
            return Task.CompletedTask;
        }
    }
}