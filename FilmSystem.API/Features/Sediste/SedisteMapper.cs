using FilmSystem.API.DTOs.Sediste;

namespace FilmSystem.API.Features.Sediste
{
    public static class SedisteMapper
    {
        public static SedisteDto ToDto(Domain.Models.Sediste s) => new()
        {
            Id = s.Id,
            BrojReda = s.BrojReda,
            BrojMesta = s.BrojMesta,
            SalaId = s.SalaId
        };
    }
}