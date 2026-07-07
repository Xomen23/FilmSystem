using FilmSystem.API.DTOs.Projekcija;
using FilmSystem.Domain.Repositories;

namespace FilmSystem.API.Features.Projekcija
{
    public static class ProjekcijaMapper
    {
        public static ProjekcijaDto ToDto(Domain.Models.Projekcija p, IUnitOfWork uow)
        {
            return new ProjekcijaDto
            {
                Id = p.Id,
                DatumVreme = p.DatumVreme,
                CenaKarte = p.CenaKarte,
                FilmId = p.FilmId,
                SalaId = p.SalaId,


                FilmNaziv = (p.Film ?? uow.Filmovi.GetById(p.FilmId))?.Naziv ?? string.Empty,
                SalaNaziv = (p.Sala ?? uow.Sale.GetById(p.SalaId))?.Naziv ?? string.Empty
            };
        }
    }
}