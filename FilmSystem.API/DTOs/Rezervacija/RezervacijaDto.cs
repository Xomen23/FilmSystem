namespace FilmSystem.API.DTOs.Rezervacija
{
    public class RezervacijaDto
    {
        public int Id { get; set; }
        public DateTime VremeKreiranja { get; set; }
        public string Status { get; set; } = string.Empty;

        public int ProjekcijаId { get; set; }
        public int SedisteId { get; set; }
    }
}
