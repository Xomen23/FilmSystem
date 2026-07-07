using FilmSystem.API.DTOs.Projekcija;
using FilmSystem.Domain.Repositories;

namespace FilmSystem.API.Features.Projekcija
{
    public static class ProjekcijaMapper
    {
        // Dodajemo IUnitOfWork u parametre metode
        public static ProjekcijaDto ToDto(Domain.Models.Projekcija p, IUnitOfWork uow)
        {
            return new ProjekcijaDto
            {
                Id = p.Id,
                DatumVreme = p.DatumVreme,
                CenaKarte = p.CenaKarte,
                FilmId = p.FilmId,
                SalaId = p.SalaId,

                // Ako p.Film fali, mapper ga sam povuče iz baze "on-the-fly"!
                FilmNaziv = (p.Film ?? uow.Filmovi.GetById(p.FilmId))?.Naziv ?? string.Empty,
                SalaNaziv = (p.Sala ?? uow.Sale.GetById(p.SalaId))?.Naziv ?? string.Empty
            };
        }
    }
}