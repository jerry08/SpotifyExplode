using System;
using System.Text.RegularExpressions;
using SpotifyExplode.Utils.Extensions;

namespace SpotifyExplode.Artists;

/// <summary>
/// Represents a syntactically valid Spotify artist ID.
/// </summary>
public readonly partial struct ArtistId
{
    /// <summary>
    /// Raw ID value.
    /// </summary>
    public string Value { get; }

    private ArtistId(string value) => Value = value;

    /// <inheritdoc />
    public override string ToString() => Value;
}

public partial struct ArtistId
{
    private static bool IsValid(string artistId)
    {
        // Track IDs are always 22 characters
        if (artistId.Length != 22)
            return false;

        return !Regex.IsMatch(artistId, @"[^0-9a-zA-Z_\-]");
    }

    private static string? TryNormalize(string? artistIdOrUrl)
    {
        if (string.IsNullOrWhiteSpace(artistIdOrUrl))
            return null;

        // Id
        // 1fZAAHNWdSM5gqbi9o5iEA
        if (IsValid(artistIdOrUrl!))
            return artistIdOrUrl;

        // Regular URL
        // https://open.spotify.com/artist/1fZAAHNWdSM5gqbi9o5iEA
        var regularMatch = Regex.Match(artistIdOrUrl, @"spotify\..+?\/artist\/([a-zA-Z0-9]+)").Groups[1].Value;
        if (!string.IsNullOrWhiteSpace(regularMatch) && IsValid(regularMatch))
            return regularMatch;

        // Invalid input
        return null;
    }

    /// <summary>
    /// Attempts to parse the specified string as a track ID or URL.
    /// Returns null in case of failure.
    /// </summary>
    public static ArtistId? TryParse(string? artistIdOrUrl) =>
        TryNormalize(artistIdOrUrl)?.Pipe(id => new ArtistId(id));

    /// <summary>
    /// Parses the specified string as a Spotify track ID or URL.
    /// Throws an exception in case of failure.
    /// </summary>
    public static ArtistId Parse(string artistIdOrUrl) =>
        TryParse(artistIdOrUrl) ??
        throw new ArgumentException($"Invalid Spotify track ID or URL '{artistIdOrUrl}'.");

    /// <summary>
    /// Converts string to ID.
    /// </summary>
    public static implicit operator ArtistId(string artistIdOrUrl) => Parse(artistIdOrUrl);

    /// <summary>
    /// Converts ID to string.
    /// </summary>
    public static implicit operator string(ArtistId artistId) => artistId.ToString();
}

public partial struct ArtistId : IEquatable<ArtistId>
{
    /// <inheritdoc />
    public bool Equals(ArtistId other) => StringComparer.Ordinal.Equals(Value, other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ArtistId other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Equality check.
    /// </summary>
    public static bool operator ==(ArtistId left, ArtistId right) => left.Equals(right);

    /// <summary>
    /// Equality check.
    /// </summary>
    public static bool operator !=(ArtistId left, ArtistId right) => !(left == right);
}