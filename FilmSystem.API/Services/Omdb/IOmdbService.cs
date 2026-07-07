namespace FilmSystem.API.Services.Omdb
{
    public interface IOmdbService
    {
        Task<OmdbFilmData?> GetByImdbIdAsync(string imdbId, CancellationToken ct = default);

        Task<IEnumerable<OmdbSearchItem>> SearchByTitleAsync(string title, CancellationToken ct = default);
    }

  
    public class OmdbException : Exception
    {
        public OmdbException(string message, Exception? inner = null) : base(message, inner)
        {
        }
    }
}
