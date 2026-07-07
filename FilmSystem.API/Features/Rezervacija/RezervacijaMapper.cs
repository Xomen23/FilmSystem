using FilmSystem.API.DTOs.Rezervacija;

namespace FilmSystem.API.Features.Rezervacija
{
    public static class RezervacijaMapper
    {
        public static RezervacijaDto ToDto(Domain.Models.Rezervacija r) => new()
        {
            Id = r.Id,
            VremeKreiranja = r.VremeKreiranja,
            Status = r.Status.ToString(),
            ProjekcijаId = r.ProjekcijаId,
            SedisteId = r.SedisteId,

    
            FilmNaziv = r.Projekcija?.Film?.Naziv ?? "Nepoznat Film",
            CenaKarte = r.Projekcija?.CenaKarte ?? 0,
            SedisteBroj = r.SedisteId 
        };
    }
}