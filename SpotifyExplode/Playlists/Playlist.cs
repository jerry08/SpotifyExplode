using System.Collections.Generic;
using System.Text.Json.Serialization;
using SpotifyExplode.Tracks;
using SpotifyExplode.Users;

namespace SpotifyExplode.Playlists;

public class Playlist
{
    [JsonPropertyName("id")]
    public PlaylistId Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("description")]
    public string Description { get; set; } = default!;

    [JsonPropertyName("followers")]
    public Follower Followers { get; set; } = default!;

    [JsonPropertyName("owner")]
    public User Owner { get; set; } = default!;

    [JsonPropertyName("items")]
    public List<Item> Items { get; set; } = default!;

    /// <summary>
    /// Maximum number of results to return.
    /// Default: 20
    /// Minimum: 1
    /// Maximum: 50
    /// Note: The limit is applied within each type, not on the total response.
    /// For example, if the limit value is 3 and the type is artist,album,
    /// the response contains 3 artists and 3 albums.
    /// </summary>
    /// <value></value>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    //[JsonPropertyName("next")]
    //public PlaylistId? Next { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    //[JsonPropertyName("previous")]
    //public PlaylistId? Previous { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonIgnore]
    public List<Track> Tracks => Items.ConvertAll(x => x.Track);
}