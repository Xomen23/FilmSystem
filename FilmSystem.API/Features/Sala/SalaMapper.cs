using FilmSystem.API.DTOs.Sala;

namespace FilmSystem.API.Features.Sala
{
    public static class SalaMapper
    {
        public static SalaDto ToDto(Domain.Models.Sala s) => new()
        {
            Id = s.Id,
            Naziv = s.Naziv,
            BrojRedova = s.BrojRedova,
            MestaPoRedu = s.MestaPoRedu
        };
    }
}