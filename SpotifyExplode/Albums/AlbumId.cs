using System;
using System.Text.RegularExpressions;
using SpotifyExplode.Utils.Extensions;

namespace SpotifyExplode.Albums;

/// <summary>
/// Represents a syntactically valid Spotify album ID.
/// </summary>
public readonly partial struct AlbumId
{
    /// <summary>
    /// Raw ID value.
    /// </summary>
    public string Value { get; }

    private AlbumId(string value) => Value = value;

    /// <inheritdoc />
    public override string ToString() => Value;
}

public partial struct AlbumId
{
    private static bool IsValid(string albumId)
    {
        // Track IDs are always 22 characters
        if (albumId.Length != 22)
            return false;

        return !Regex.IsMatch(albumId, @"[^0-9a-zA-Z_\-]");
    }

    private static string? TryNormalize(string? albumIdOrUrl)
    {
        if (string.IsNullOrWhiteSpace(albumIdOrUrl))
            return null;

        // Id
        // 4yP0hdKOZPNshxUOjY0cZj
        if (IsValid(albumIdOrUrl!))
            return albumIdOrUrl;

        // Regular URL
        // https://open.spotify.com/album/4yP0hdKOZPNshxUOjY0cZj
        var regularMatch = Regex.Match(albumIdOrUrl, @"spotify\..+?\/album\/([a-zA-Z0-9]+)").Groups[1].Value;
        if (!string.IsNullOrWhiteSpace(regularMatch) && IsValid(regularMatch))
            return regularMatch;

        // Invalid input
        return null;
    }

    /// <summary>
    /// Attempts to parse the specified string as a track ID or URL.
    /// Returns null in case of failure.
    /// </summary>
    public static AlbumId? TryParse(string? albumIdOrUrl) =>
        TryNormalize(albumIdOrUrl)?.Pipe(id => new AlbumId(id));

    /// <summary>
    /// Parses the specified string as a Spotify track ID or URL.
    /// Throws an exception in case of failure.
    /// </summary>
    public static AlbumId Parse(string albumIdOrUrl) =>
        TryParse(albumIdOrUrl) ??
        throw new ArgumentException($"Invalid Spotify track ID or URL '{albumIdOrUrl}'.");

    /// <summary>
    /// Converts string to ID.
    /// </summary>
    public static implicit operator AlbumId(string albumIdOrUrl) => Parse(albumIdOrUrl);

    /// <summary>
    /// Converts ID to string.
    /// </summary>
    public static implicit operator string(AlbumId albumId) => albumId.ToString();
}

public partial struct AlbumId : IEquatable<AlbumId>
{
    /// <inheritdoc />
    public bool Equals(AlbumId other) => StringComparer.Ordinal.Equals(Value, other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is AlbumId other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Equality check.
    /// </summary>
    public static bool operator ==(AlbumId left, AlbumId right) => left.Equals(right);

    /// <summary>
    /// Equality check.
    /// </summary>
    public static bool operator !=(AlbumId left, AlbumId right) => !(left == right);
}