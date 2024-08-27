using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using System.Xml.Linq;
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
    public List<Image> Images { get; set; } = [];

    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    public override string ToString() => $"User ({DisplayName})";
}