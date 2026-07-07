using FilmSystem.API.DTOs.Film;
using FilmSystem.Domain.Models;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Film.Commands
{
    public record CreateFilmCommand(FilmCreateDto Dto) : IRequest<FilmDto>;

    public class CreateFilmHandler : IRequestHandler<CreateFilmCommand, FilmDto>
    {
        private readonly IUnitOfWork _uow;
        public CreateFilmHandler(IUnitOfWork uow) => _uow = uow;

        public Task<FilmDto> Handle(CreateFilmCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            var film = new Domain.Models.Film
            {
                Naziv = dto.Naziv,
                Godina = dto.Godina,
                TrajanjeMin = dto.TrajanjeMin,
                ImdbId = dto.ImdbId,
                Opis = dto.Opis,
                Poster = dto.Poster,
                ZanrId = dto.ZanrId
            };

            _uow.Filmovi.Add(film);
            _uow.SaveChanges();

            film = _uow.Filmovi.GetById(film.Id)!;
            return Task.FromResult(FilmMapper.ToDto(film));
        }
    }
}