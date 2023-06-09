using System.Collections.Generic;
using System.Text.Json.Serialization;
using SpotifyExplode.Common;
using SpotifyExplode.Playlists;

namespace SpotifyExplode.Users;

public class User
{
    [JsonPropertyName("id")]
    public UserId Id { get; set; }

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = default!;

    [JsonPropertyName("followers")]
    public Follower Followers { get; set; } = default!;

    [JsonPropertyName("images")]
    public List<Image> Images { get; set; } = default!;
}