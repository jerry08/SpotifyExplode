using Newtonsoft.Json;

namespace SpotifyExplode.Playlists;

public class Follower
{
    [JsonProperty("href")]
    public string? Link { get; set; }

    [JsonProperty("total")]
    public int Total { get; set; }
}