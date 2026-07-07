using FilmSystem.API.DTOs.Zanr;

namespace FilmSystem.API.Features.Zanr
{
    public static class ZanrMapper
    {
        public static ZanrDto ToDto(Domain.Models.Zanr z) => new()
        {
            Id = z.Id,
            Naziv = z.Naziv
        };
    }
}