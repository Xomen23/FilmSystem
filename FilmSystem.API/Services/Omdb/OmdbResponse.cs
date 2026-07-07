using System.Text.Json.Serialization;

namespace FilmSystem.API.Services.Omdb
{

    public class OmdbResponse
    {
        [JsonPropertyName("Title")]
        public string? Title { get; set; }

        [JsonPropertyName("Year")]
        public string? Year { get; set; }

        [JsonPropertyName("Runtime")]
        public string? Runtime { get; set; }

        [JsonPropertyName("Plot")]
        public string? Plot { get; set; }

        [JsonPropertyName("Poster")]
        public string? Poster { get; set; }

        [JsonPropertyName("imdbID")]
        public string? ImdbId { get; set; }

        [JsonPropertyName("Response")]
        public string? Response { get; set; }

        [JsonPropertyName("Error")]
        public string? Error { get; set; }

        public bool IsSuccess => string.Equals(Response, "True", StringComparison.OrdinalIgnoreCase);
    }
}
