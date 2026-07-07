namespace FilmSystem.API.Services.Omdb
{

    public class OmdbFilmData
    {
        public string ImdbId { get; set; } = string.Empty;
        public string Naziv { get; set; } = string.Empty;
        public int Godina { get; set; }
        public int TrajanjeMin { get; set; }
        public string? Opis { get; set; }
        public string? Poster { get; set; }
    }
}
