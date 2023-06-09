using System.Collections.Generic;
using System.Text.Json.Serialization;
using SpotifyExplode.Common;
using SpotifyExplode.Playlists;

namespace SpotifyExplode.Artists;

public class Artist
{
    [JsonPropertyName("id")]
    //[JsonConverter(typeof(ArtistIdJsonConverter))]
    public ArtistId Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("followers")]
    public Follower Followers { get; set; } = default!;

    [JsonPropertyName("genres")]
    public List<string> Genres { get; set; } = default!;

    [JsonPropertyName("images")]
    public List<Image> Images { get; set; } = default!;

    [JsonPropertyName("popularity")]
    public int Popularity { get; set; }
}