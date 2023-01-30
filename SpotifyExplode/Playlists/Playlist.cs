using System.Collections.Generic;
using Newtonsoft.Json;
using SpotifyExplode.Tracks;
using SpotifyExplode.Users;

namespace SpotifyExplode.Playlists;

public class Playlist
{
    [JsonProperty("id")]
    public PlaylistId Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("description")]
    public string Description { get; set; } = default!;

    [JsonProperty("followers")]
    public Follower Followers { get; set; } = default!;

    [JsonProperty("owner")]
    public User Owner { get; set; } = default!;

    [JsonProperty("items")]
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
    [JsonProperty("limit")]
    public int Limit { get; set; }

    //[JsonProperty("next")]
    //public PlaylistId? Next { get; set; }

    [JsonProperty("offset")]
    public int Offset { get; set; }

    //[JsonProperty("previous")]
    //public PlaylistId? Previous { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }

    [JsonIgnore]
    public List<Track> Tracks => Items.ConvertAll(x => x.Track);
}