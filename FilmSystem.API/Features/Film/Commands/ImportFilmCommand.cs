using FilmSystem.API.DTOs.Film;
using FilmSystem.API.Services.Omdb;
using FilmSystem.Domain.Repositories;
using MediatR;

namespace FilmSystem.API.Features.Film.Commands
{
    public record ImportFilmCommand(string ImdbId, int ZanrId) : IRequest<FilmDto>;

    public class ImportFilmHandler : IRequestHandler<ImportFilmCommand, FilmDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IOmdbService _omdb;
        public ImportFilmHandler(IUnitOfWork uow, IOmdbService omdb)
        {
            _uow = uow;
            _omdb = omdb;
        }

        public async Task<FilmDto> Handle(ImportFilmCommand request, CancellationToken ct)
        {
            // validacija zanra i duplikata je u kontroleru pre Send()
            var omdbData = await _omdb.GetByImdbIdAsync(request.ImdbId, ct);
            if (omdbData == null)
                throw new KeyNotFoundException($"OMDb ne prepoznaje ImdbId '{request.ImdbId}'.");

            var film = new Domain.Models.Film
            {
                Naziv = omdbData.Naziv,
                Godina = omdbData.Godina,
                TrajanjeMin = omdbData.TrajanjeMin,
                Opis = omdbData.Opis,
                Poster = omdbData.Poster,
                ImdbId = omdbData.ImdbId,
                ZanrId = request.ZanrId
            };

            _uow.Filmovi.Add(film);
            _uow.SaveChanges();

            film = _uow.Filmovi.GetById(film.Id)!;
            return FilmMapper.ToDto(film);
        }
    }
}