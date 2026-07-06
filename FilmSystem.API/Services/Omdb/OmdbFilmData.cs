namespace FilmSystem.API.Services.Omdb
{
    // Ociscen/parsiran rezultat OMDb poziva - Runtime i Year u OMDb dolaze kao
    // stringovi ("148 min", "1994") pa ih ovde parsiramo u int da ih FilmController
    // moze direktno smestiti u Film entitet bez dodatnog parsiranja.
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
