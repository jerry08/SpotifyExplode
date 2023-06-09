using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using SpotifyExplode.Albums;
using SpotifyExplode.Artists;

namespace SpotifyExplode.Tracks;

public class Track
{
    /// <inheritdoc />
    [JsonPropertyName("id")]
    public TrackId Id { get; set; }

    /// <inheritdoc />
    public string Url => $"https://open.spotify.com/track/{Id}";

    /// <inheritdoc />
    [JsonPropertyName("name")]
    public string Title { get; set; } = default!;

    [JsonPropertyName("track_number")]
    public int TrackNumber { get; set; }

    [JsonPropertyName("popularity")]
    public int Popularity { get; set; }

    [JsonPropertyName("available_markets")]
    public List<string> AvailableMarkets { get; set; } = default!;

    [JsonPropertyName("disc_number")]
    public int DiscNumber { get; set; }

    [JsonPropertyName("duration_ms")]
    public long DurationMs { get; set; }

    [JsonPropertyName("explicit")]
    public bool Explicit { get; set; }

    [JsonPropertyName("is_local")]
    public bool IsLocal { get; set; }

    [JsonPropertyName("preview_url")]
    public string PreviewUrl { get; set; } = default!;

    [JsonPropertyName("artists")]
    public List<Artist> Artists { get; set; } = default!;

    [JsonPropertyName("album")]
    public Album Album { get; set; } = default!;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => $"Track ({Title})";
}