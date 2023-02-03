using Newtonsoft.Json;
using SpotifyExplode.Common;
using SpotifyExplode.Playlists;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyExplode.Artists;

public class Artist
{
    [JsonProperty("id")]
    public ArtistId Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; } = default!;

    [JsonProperty("followers")]
    public Follower Followers { get; set; } = default!;

    [JsonProperty("genres")]
    public List<string> Genres { get; set; } = default!;

    [JsonProperty("images")]
    public List<Image> Images { get; set; } = default!;

    [JsonProperty("popularity")]
    public int Popularity { get; set; }
}