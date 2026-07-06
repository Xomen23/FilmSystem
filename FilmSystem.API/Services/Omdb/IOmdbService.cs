namespace FilmSystem.API.Services.Omdb
{
    public interface IOmdbService
    {
        // Vraca null ako OMDb ne prepoznaje dati imdbId (Response = False),
        // baca OmdbException za mrezne/HTTP greske (nakon iscrpljenih retry pokusaja).
        Task<OmdbFilmData?> GetByImdbIdAsync(string imdbId, CancellationToken ct = default);

        Task<IEnumerable<OmdbSearchItem>> SearchByTitleAsync(string title, CancellationToken ct = default);
    }

    // Posebna exception klasa da je kontroler moze uhvatiti odvojeno od "film ne postoji"
    // slucaja i vratiti 502/503 umesto 404.
    public class OmdbException : Exception
    {
        public OmdbException(string message, Exception? inner = null) : base(message, inner)
        {
        }
    }
}
