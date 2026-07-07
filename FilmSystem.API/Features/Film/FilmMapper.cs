using FilmSystem.API.DTOs.Film;

namespace FilmSystem.API.Features.Film
{
    public static class FilmMapper
    {
        public static FilmDto ToDto(Domain.Models.Film film) => new()
        {
            Id = film.Id,
            Naziv = film.Naziv,
            Godina = film.Godina,
            TrajanjeMin = film.TrajanjeMin,
            ImdbId = film.ImdbId,
            Opis = film.Opis,
            Poster = film.Poster,
            ZanrId = film.ZanrId,
            ZanrNaziv = film.Zanr?.Naziv ?? string.Empty
        };
    }
}