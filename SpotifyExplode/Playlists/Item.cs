using System;
using Newtonsoft.Json;
using SpotifyExplode.Tracks;
using SpotifyExplode.Users;

namespace SpotifyExplode.Playlists;

public class Item
{
    [JsonProperty("added_at")]
    public DateTime AddedAt { get; set; }

    public User? AddedBy { get; set; }

    [JsonProperty("track")]
    public Track Track { get; set; } = default!;
}