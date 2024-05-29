using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using SpotifyExplode.Artists;
using SpotifyExplode.Common;
using SpotifyExplode.Tracks;

namespace SpotifyExplode.Albums;

public class Album
{
    [JsonPropertyName("id")]
    public AlbumId Id { get; set; }

    public string Url => $"https://open.spotify.com/track/{Id}";

    [JsonPropertyName("label")]
    public string Label { get; set; } = default!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("album_type")]
    public AlbumType AlbumType { get; set; }

    [JsonPropertyName("popularity")]
    public int Popularity { get; set; }

    [JsonPropertyName("release_date")]
    public string ReleaseDateStr { get; set; } = default!;

    public DateTime? ReleaseDate => DateTime.TryParse(ReleaseDateStr, out var releaseDate) ? releaseDate : null;

    [JsonPropertyName("total_tracks")]
    public int TotalTracks { get; set; }

    [JsonIgnore]
    public List<Track> Tracks { get; set; } = default!;

    [JsonPropertyName("available_markets")]
    public List<string> AvailableMarkets { get; set; } = default!;

    [JsonPropertyName("artists")]
    public List<Artist> Artists { get; set; } = default!;

    [JsonPropertyName("images")]
    public List<Image> Images { get; set; } = default!;

    [JsonPropertyName("genres")]
    public List<string> Genres { get; set; } = default!;

    /// <summary>
    /// Known external IDs for the track.
    /// </summary>
    [JsonPropertyName("external_ids")]
    public ExternalIds? ExternalIds { get; set; } = default!;
    
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => $"Album ({Name})";
}