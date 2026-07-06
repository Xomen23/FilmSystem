namespace FilmSystem.API.DTOs.Film
{
    public class FilmUpdateDto
    {
        public string Naziv { get; set; } = string.Empty;
        public int Godina { get; set; }
        public int TrajanjeMin { get; set; }
        public string? Opis { get; set; }
        public string? Poster { get; set; }
        public int ZanrId { get; set; }
    }
}
