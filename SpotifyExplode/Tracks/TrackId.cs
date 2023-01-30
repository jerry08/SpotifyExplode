using System;
using System.Text.RegularExpressions;
using SpotifyExplode.Utils.Extensions;

namespace SpotifyExplode.Tracks;

/// <summary>
/// Represents a syntactically valid Spotify track ID.
/// </summary>
public readonly partial struct TrackId
{
    /// <summary>
    /// Raw ID value.
    /// </summary>
    public string Value { get; }

    private TrackId(string value) => Value = value;

    /// <inheritdoc />
    public override string ToString() => Value;
}

public partial struct TrackId
{
    private static bool IsValid(string trackId)
    {
        // Track IDs are always 22 characters
        if (trackId.Length != 22)
            return false;

        return !Regex.IsMatch(trackId, @"[^0-9a-zA-Z_\-]");
    }

    private static string? TryNormalize(string? trackIdOrUrl)
    {
        if (string.IsNullOrWhiteSpace(trackIdOrUrl))
            return null;

        // Id
        // 0VjIjW4GlUZAMYd2vXMi3b
        if (IsValid(trackIdOrUrl!))
            return trackIdOrUrl;

        // Regular URL
        // https://open.spotify.com/track/0VjIjW4GlUZAMYd2vXMi3b
        var regularMatch = Regex.Match(trackIdOrUrl, @"spotify\..+?\/track\/([a-zA-Z0-9]+)").Groups[1].Value;
        if (!string.IsNullOrWhiteSpace(regularMatch) && IsValid(regularMatch))
            return regularMatch;

        // Invalid input
        return null;
    }

    /// <summary>
    /// Attempts to parse the specified string as a track ID or URL.
    /// Returns null in case of failure.
    /// </summary>
    public static TrackId? TryParse(string? trackIdOrUrl) =>
        TryNormalize(trackIdOrUrl)?.Pipe(id => new TrackId(id));

    /// <summary>
    /// Parses the specified string as a Spotify track ID or URL.
    /// Throws an exception in case of failure.
    /// </summary>
    public static TrackId Parse(string trackIdOrUrl) =>
        TryParse(trackIdOrUrl) ??
        throw new ArgumentException($"Invalid Spotify track ID or URL '{trackIdOrUrl}'.");

    /// <summary>
    /// Converts string to ID.
    /// </summary>
    public static implicit operator TrackId(string trackIdOrUrl) => Parse(trackIdOrUrl);

    /// <summary>
    /// Converts ID to string.
    /// </summary>
    public static implicit operator string(TrackId trackId) => trackId.ToString();
}

public partial struct TrackId : IEquatable<TrackId>
{
    /// <inheritdoc />
    public bool Equals(TrackId other) => StringComparer.Ordinal.Equals(Value, other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is TrackId other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Equality check.
    /// </summary>
    public static bool operator ==(TrackId left, TrackId right) => left.Equals(right);

    /// <summary>
    /// Equality check.
    /// </summary>
    public static bool operator !=(TrackId left, TrackId right) => !(left == right);
}