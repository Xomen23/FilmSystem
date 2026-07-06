using System.Globalization;
using System.Net.Http.Json;

namespace FilmSystem.API.Services.Omdb
{
    public class OmdbService : IOmdbService
    {
        // Ime mora da se poklapa sa builder.Services.AddHttpClient("Omdb", ...) u Program.cs
        public const string HttpClientName = "Omdb";

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public OmdbService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient(HttpClientName);
            _configuration = configuration;
        }

        public async Task<OmdbFilmData?> GetByImdbIdAsync(string imdbId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(imdbId))
                throw new ArgumentException("ImdbId je obavezan.", nameof(imdbId));

            var apiKey = _configuration["Omdb:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new OmdbException("Omdb:ApiKey nije podesen u konfiguraciji (appsettings/user-secrets).");

            OmdbResponse? omdb;
            try
            {
                // BaseAddress je vec podesen na named HttpClient-u u Program.cs.
                // Resilience handler (retry + timeout) je registrovan preko
                // AddStandardResilienceHandler(), pa se ovde ne bavimo rucnim retry-jem.
                omdb = await _httpClient.GetFromJsonAsync<OmdbResponse>(
                    $"?apikey={apiKey}&i={Uri.EscapeDataString(imdbId)}", ct);
            }
            catch (HttpRequestException ex)
            {
                throw new OmdbException("OMDb API nije dostupan (mrezna greska).", ex);
            }
            catch (TaskCanceledException ex) when (!ct.IsCancellationRequested)
            {
                // Resilience handler baca TaskCanceledException kad istekne timeout,
                // razlikujemo to od "korisnik je otkazao zahtev" (ct.IsCancellationRequested).
                throw new OmdbException("OMDb API nije odgovorio na vreme (timeout).", ex);
            }

            if (omdb == null)
                throw new OmdbException("OMDb API je vratio prazan odgovor.");

            if (!omdb.IsSuccess)
            {
                // Response = "False" znaci da imdbId ne postoji u OMDb bazi -
                // to nije greska sistema, vec "nije pronadjeno", pa vracamo null.
                return null;
            }

            return new OmdbFilmData
            {
                ImdbId = omdb.ImdbId ?? imdbId,
                Naziv = omdb.Title ?? string.Empty,
                Godina = ParseYear(omdb.Year),
                TrajanjeMin = ParseRuntime(omdb.Runtime),
                Opis = omdb.Plot,
                Poster = (omdb.Poster == "N/A") ? null : omdb.Poster
            };
        }

        // OMDb ponekad vraca "1994–2003" (serije) - uzimamo samo prve 4 cifre.
        private static int ParseYear(string? year)
        {
            if (string.IsNullOrWhiteSpace(year))
                return 0;

            var digits = new string(year.Take(4).ToArray());
            return int.TryParse(digits, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)
                ? result
                : 0;
        }

        public async Task<IEnumerable<OmdbSearchItem>> SearchByTitleAsync(string title, CancellationToken ct = default)
        {
            var apiKey = _configuration["Omdb:ApiKey"];
            var response = await _httpClient.GetFromJsonAsync<OmdbSearchResponse>(
                $"?apikey={apiKey}&s={Uri.EscapeDataString(title)}", ct);

            return response?.SearchResults ?? Enumerable.Empty<OmdbSearchItem>();
        }

        // OMDb vraca "148 min" - izvlacimo samo broj.
        private static int ParseRuntime(string? runtime)
        {
            if (string.IsNullOrWhiteSpace(runtime))
                return 0;

            var digitsOnly = new string(runtime.TakeWhile(char.IsDigit).ToArray());
            return int.TryParse(digitsOnly, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)
                ? result
                : 0;
        }
    }
}
