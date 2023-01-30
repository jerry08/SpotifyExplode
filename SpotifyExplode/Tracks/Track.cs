using Newtonsoft.Json;
using SpotifyExplode.Albums;
using SpotifyExplode.Artists;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyExplode.Tracks;

public class Track
{
    /// <inheritdoc />
    [JsonProperty("id")]
    public TrackId Id { get; set; }

    /// <inheritdoc />
    public string Url => $"https://open.spotify.com/track/{Id}";

    /// <inheritdoc />
    [JsonProperty("name")]
    public string Title { get; set; } = default!;

    [JsonProperty("track_number")]
    public int TrackNumber { get; set; }

    [JsonProperty("popularity")]
    public int Popularity { get; set; }

    [JsonProperty("available_markets")]
    public List<string> AvailableMarkets { get; set; } = default!;

    [JsonProperty("disc_number")]
    public int DiscNumber { get; set; }

    [JsonProperty("duration_ms")]
    public long DurationMs { get; set; }

    [JsonProperty("explicit")]
    public bool Explicit { get; set; }

    [JsonProperty("is_local")]
    public bool IsLocal { get; set; }

    [JsonProperty("preview_url")]
    public string PreviewUrl { get; set; } = default!;

    [JsonProperty("artists")]
    public List<Artist> Artists { get; set; } = default!;

    [JsonProperty("album")]
    public Album Album { get; set; } = default!;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => $"Track ({Title})";
}