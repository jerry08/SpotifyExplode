using System;
using System.Text.RegularExpressions;
using SpotifyExplode.Utils.Extensions;

namespace SpotifyExplode.Playlists;

/// <summary>
/// Represents a syntactically valid Spotify playlist ID.
/// </summary>
public readonly partial struct PlaylistId
{
    /// <summary>
    /// Raw ID value.
    /// </summary>
    public string Value { get; }

    private PlaylistId(string value) => Value = value;

    /// <inheritdoc />
    public override string ToString() => Value;
}

public partial struct PlaylistId
{
    private static bool IsValid(string playlistId)
    {
        // Track IDs are always 22 characters
        if (playlistId.Length != 22)
            return false;

        return !Regex.IsMatch(playlistId, @"[^0-9a-zA-Z_\-]");
    }

    private static string? TryNormalize(string? playlistIdOrUrl)
    {
        if (string.IsNullOrWhiteSpace(playlistIdOrUrl))
            return null;

        // Id
        // 0tSYjDUflcozy78WwUFe6y
        if (IsValid(playlistIdOrUrl!))
            return playlistIdOrUrl;

        // Regular URL
        // https://open.spotify.com/playlist/0tSYjDUflcozy78WwUFe6y
        var regularMatch = Regex.Match(playlistIdOrUrl, @"spotify\..+?\/playlist\/([a-zA-Z0-9]+)").Groups[1].Value;
        if (!string.IsNullOrWhiteSpace(regularMatch) && IsValid(regularMatch))
            return regularMatch;

        // Track Limit URL
        // https://api.spotify.com/v1/playlists/0tSYjDUflcozy78WwUFe6y/tracks?offset=19&limit=19
        var limitMatch = Regex.Match(playlistIdOrUrl, @"spotify\..+?\/v1\/playlists\/([a-zA-Z0-9]+)").Groups[1].Value;
        if (!string.IsNullOrWhiteSpace(limitMatch) && IsValid(limitMatch))
            return limitMatch;

        // Invalid input
        return null;
    }

    /// <summary>
    /// Attempts to parse the specified string as a track ID or URL.
    /// Returns null in case of failure.
    /// </summary>
    public static PlaylistId? TryParse(string? playlistIdOrUrl) =>
        TryNormalize(playlistIdOrUrl)?.Pipe(id => new PlaylistId(id));

    /// <summary>
    /// Parses the specified string as a Spotify track ID or URL.
    /// Throws an exception in case of failure.
    /// </summary>
    public static PlaylistId Parse(string playlistIdOrUrl) =>
        TryParse(playlistIdOrUrl) ??
        throw new ArgumentException($"Invalid Spotify track ID or URL '{playlistIdOrUrl}'.");

    /// <summary>
    /// Converts string to ID.
    /// </summary>
    public static implicit operator PlaylistId(string playlistIdOrUrl) => Parse(playlistIdOrUrl);

    /// <summary>
    /// Converts ID to string.
    /// </summary>
    public static implicit operator string(PlaylistId playlistId) => playlistId.ToString();
}

public partial struct PlaylistId : IEquatable<PlaylistId>
{
    /// <inheritdoc />
    public bool Equals(PlaylistId other) => StringComparer.Ordinal.Equals(Value, other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is PlaylistId other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Equality check.
    /// </summary>
    public static bool operator ==(PlaylistId left, PlaylistId right) => left.Equals(right);

    /// <summary>
    /// Equality check.
    /// </summary>
    public static bool operator !=(PlaylistId left, PlaylistId right) => !(left == right);
}