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

            // DODAJ OVE LINIJE:
            // Proveravamo usput da li su objekti null (pomoću ?) da aplikacija ne bi pukla
            FilmNaziv = r.Projekcija?.Film?.Naziv ?? "Nepoznat Film",
            CenaKarte = r.Projekcija?.CenaKarte ?? 0,
            SedisteBroj = r.SedisteId // Ovde šaljemo čist ID sedišta koji frontend traži
        };
    }
}