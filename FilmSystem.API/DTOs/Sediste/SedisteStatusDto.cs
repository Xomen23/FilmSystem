namespace FilmSystem.API.DTOs.Sediste
{
    // Koristi se za GET /api/sale/{salaId}/projekcije/{projekcijaId}/sedista
    public class SedisteStatusDto
    {
        public int Id { get; set; }
        public int BrojReda { get; set; }
        public int BrojMesta { get; set; }
        public bool Slobodno { get; set; }
    }
}
