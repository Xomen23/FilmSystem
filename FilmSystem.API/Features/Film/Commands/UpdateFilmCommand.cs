using FilmSystem.API.DTOs.Film;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Film.Commands
{
    public record UpdateFilmCommand(int Id, FilmUpdateDto Dto) : IRequest;

    public class UpdateFilmHandler : IRequestHandler<UpdateFilmCommand>
    {
        private readonly IUnitOfWork _uow;
        public UpdateFilmHandler(IUnitOfWork uow) => _uow = uow;

        public Task Handle(UpdateFilmCommand request, CancellationToken ct)
        {
            var film = _uow.Filmovi.GetById(request.Id)
                ?? throw new KeyNotFoundException($"Film sa Id {request.Id} ne postoji.");

            film.Naziv = request.Dto.Naziv;
            film.Godina = request.Dto.Godina;
            film.TrajanjeMin = request.Dto.TrajanjeMin;
            film.Opis = request.Dto.Opis;
            film.Poster = request.Dto.Poster;
            film.ZanrId = request.Dto.ZanrId;

            _uow.Filmovi.Update(film);
            _uow.SaveChanges();
            return Task.CompletedTask;
        }
    }
}