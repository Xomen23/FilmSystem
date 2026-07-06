namespace FilmSystem.API.DTOs.Film
{
    public class FilmDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public int Godina { get; set; }
        public int TrajanjeMin { get; set; }
        public string? ImdbId { get; set; }
        public string? Opis { get; set; }
        public string? Poster { get; set; }
        public int ZanrId { get; set; }
        public string ZanrNaziv { get; set; } = string.Empty;
    }
}
