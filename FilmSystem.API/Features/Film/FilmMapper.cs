using FilmSystem.API.DTOs.Film;

namespace FilmSystem.API.Features.Film
{
    // Statička helper klasa - zamenjuje privatni ToDto() koji je bio u kontroleru
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