using System.Text.Json.Serialization;

namespace FilmSystem.API.Services.Omdb
{
    // Mapira JSON koji vraca OMDb API (http://www.omdbapi.com/?i=<imdbId>).
    // OMDb koristi PascalCase nazive polja, ali JSON je case-sensitive na nacin
    // koji ne odgovara C# konvenciji za neka polja (npr. "Response", "Error"),
    // pa svuda eksplicitno navodimo JsonPropertyName da izbegnemo iznenadjenja.
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

        // OMDb vraca "Response": "True"/"False" (string, ne bool!) i "Error"
        // popunjen samo kad "Response" = "False" (npr. nepostojeci imdbId).
        [JsonPropertyName("Response")]
        public string? Response { get; set; }

        [JsonPropertyName("Error")]
        public string? Error { get; set; }

        public bool IsSuccess => string.Equals(Response, "True", StringComparison.OrdinalIgnoreCase);
    }
}
