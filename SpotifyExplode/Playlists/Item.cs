using System;
using System.Text.Json.Serialization;
using SpotifyExplode.Tracks;
using SpotifyExplode.Users;

namespace SpotifyExplode.Playlists;

public class Item
{
    [JsonPropertyName("added_at")]
    public DateTime AddedAt { get; set; }

    public User? AddedBy { get; set; }

    [JsonPropertyName("track")]
    public Track Track { get; set; } = default!;
}