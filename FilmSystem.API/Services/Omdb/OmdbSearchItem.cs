using System.Text.Json.Serialization;

namespace FilmSystem.API.Services.Omdb
{
    public class OmdbSearchItem
    {
        [JsonPropertyName("Title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("Year")]
        public string Year { get; set; } = string.Empty;

        [JsonPropertyName("imdbID")]
        public string ImdbId { get; set; } = string.Empty;

        [JsonPropertyName("Poster")]
        public string? Poster { get; set; }
    }

}
