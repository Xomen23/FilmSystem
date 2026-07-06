using System.Text.Json.Serialization;

namespace FilmSystem.API.Services.Omdb
{
    public class OmdbSearchResponse
    {
        [JsonPropertyName("Search")]
        public List<OmdbSearchItem>? SearchResults { get; set; }

        [JsonPropertyName("Response")]
        public string? Response { get; set; }
    }
}
