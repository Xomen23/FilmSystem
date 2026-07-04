namespace FilmSystem.API.DTOs.Projekcija
{
    // Namerno bez FilmId/SalaId - izmena filma ili sale za postojecu projekciju
    // bi komplikovala vec napravljene rezervacije sedista. Ako to zatreba,
    // korisnik otkaze projekciju i napravi novu.
    public class ProjekcijaUpdateDto
    {
        public DateTime DatumVreme { get; set; }
        public decimal CenaKarte { get; set; }
    }
}
