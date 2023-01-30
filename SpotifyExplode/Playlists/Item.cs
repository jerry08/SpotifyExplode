using System;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using SpotifyExplode.Users;
using SpotifyExplode.Tracks;

namespace SpotifyExplode.Playlists;

public class Item
{
    [JsonProperty("added_at")]
    public DateTime AddedAt { get; set; }

    [JsonProperty("added_by")]
    public User AddedBy { get; set; } = default!;

    [JsonProperty("track")]
    public Track Track { get; set; } = default!;
}