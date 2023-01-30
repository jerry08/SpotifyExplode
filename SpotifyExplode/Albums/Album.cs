using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using SpotifyExplode.Artists;
using SpotifyExplode.Tracks;

namespace SpotifyExplode.Albums;

public class Album
{
    [JsonProperty("id")]
    public AlbumId Id { get; set; }

    public string Url => $"https://open.spotify.com/track/{Id}";

    [JsonProperty("label")]
    public string Label { get; set; } = default!;

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("album_type")]
    public AlbumType AlbumType { get; set; }

    [JsonProperty("popularity")]
    public int Popularity { get; set; }

    [JsonProperty("release_date")]
    public string ReleaseDateStr { get; set; } = default!;

    public DateTime? ReleaseDate => DateTime.TryParse(ReleaseDateStr, out DateTime releaseDate) ? releaseDate : null;

    [JsonProperty("total_tracks")]
    public int TotalTracks { get; set; }

    [JsonIgnore]
    public List<Track> Tracks { get; set; } = default!;

    [JsonProperty("available_markets")]
    public List<string> AvailableMarkets { get; set; } = default!;

    [JsonProperty("artists")]
    public List<Artist> Artists { get; set; } = default!;

    [JsonProperty("images")]
    public List<Image> Images { get; set; } = default!;

    [JsonProperty("genres")]
    public List<string> Genres { get; set; } = default!;

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => $"Album ({Name})";
}