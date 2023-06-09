using System.Text.Json.Serialization;

namespace SpotifyExplode.Playlists;

public class Follower
{
    [JsonPropertyName("href")]
    public string? Link { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}