using System.Text.Json.Serialization;

namespace SpotifyExplode.Common;

public class ExternalIds
{
    /// <summary>
    /// International Standard Recording Code
    /// </summary>
    [JsonPropertyName("isrc")]
    public string? IsrcCode { get; set; }

    /// <summary>
    /// International Article Number
    /// </summary>
    [JsonPropertyName("ean")]
    public string? EanCode { get; set; }

    /// <summary>
    /// Universal Product Code
    /// </summary>
    [JsonPropertyName("upc")]
    public string? UpcCode { get; set; }
}