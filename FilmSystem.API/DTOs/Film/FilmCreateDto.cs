namespace FilmSystem.API.DTOs.Film
{
    // Rucno kreiranje filma (bez OMDb-a) - naziv/godina/trajanje unosi korisnik.
    // ImdbId/Opis/Poster su opcioni jer se najcesce popunjavaju kroz import endpoint.
    public class FilmCreateDto
    {
        public string Naziv { get; set; } = string.Empty;
        public int Godina { get; set; }
        public int TrajanjeMin { get; set; }
        public string? ImdbId { get; set; }
        public string? Opis { get; set; }
        public string? Poster { get; set; }
        public int ZanrId { get; set; }
    }
}
