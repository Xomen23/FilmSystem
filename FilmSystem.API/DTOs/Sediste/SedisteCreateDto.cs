namespace FilmSystem.API.DTOs.Sediste
{
    // Za dodavanje pojedinacnog sedista u vec postojecu salu
    // (npr. sala je naknadno prosirena za jos jedan red)
    public class SedisteCreateDto
    {
        public int SalaId { get; set; }
        public int BrojReda { get; set; }
        public int BrojMesta { get; set; }
    }
}
