using Newtonsoft.Json;
using SpotifyExplode.Playlists;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SpotifyExplode.Users;

public class User
{
    [JsonProperty("id")]
    public UserId Id { get; set; }

    [JsonProperty("display_name")]
    public string DisplayName { get; set; } = default!;

    [JsonProperty("followers")]
    public Follower Followers { get; set; } = default!;

    [JsonProperty("images")]
    public List<Image> Images { get; set; } = default!;
}