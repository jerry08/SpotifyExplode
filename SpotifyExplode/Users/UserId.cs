using System;
using System.Text.RegularExpressions;
using SpotifyExplode.Utils.Extensions;

namespace SpotifyExplode.Users;

/// <summary>
/// Represents a syntactically valid Spotify user ID.
/// </summary>
public readonly partial struct UserId
{
    /// <summary>
    /// Raw ID value.
    /// </summary>
    public string Value { get; }

    private UserId(string value) => Value = value;

    /// <inheritdoc />
    public override string ToString() => Value;
}

public partial struct UserId
{
    private static bool IsValid(string userId)
    {
        return !Regex.IsMatch(userId, @"[^0-9a-zA-Z_\-]");
    }

    private static string? TryNormalize(string? userIdOrUrl)
    {
        if (string.IsNullOrWhiteSpace(userIdOrUrl))
            return null;

        // Id
        // xxu0yww90v07gbh9veqta7ze0
        if (IsValid(userIdOrUrl!))
            return userIdOrUrl;

        // Regular URL
        // https://open.spotify.com/user/xxu0yww90v07gbh9veqta7ze0
        var regularMatch = Regex.Match(userIdOrUrl, @"spotify\..+?\/user\/([a-zA-Z0-9]+)").Groups[1].Value;
        if (!string.IsNullOrWhiteSpace(regularMatch) && IsValid(regularMatch))
            return regularMatch;

        // Invalid input
        return null;
    }

    /// <summary>
    /// Attempts to parse the specified string as a track ID or URL.
    /// Returns null in case of failure.
    /// </summary>
    public static UserId? TryParse(string? userIdOrUrl) =>
        TryNormalize(userIdOrUrl)?.Pipe(id => new UserId(id));

    /// <summary>
    /// Parses the specified string as a Spotify track ID or URL.
    /// Throws an exception in case of failure.
    /// </summary>
    public static UserId Parse(string userIdOrUrl) =>
        TryParse(userIdOrUrl) ??
        throw new ArgumentException($"Invalid Spotify track ID or URL '{userIdOrUrl}'.");

    /// <summary>
    /// Converts string to ID.
    /// </summary>
    public static implicit operator UserId(string userIdOrUrl) => Parse(userIdOrUrl);

    /// <summary>
    /// Converts ID to string.
    /// </summary>
    public static implicit operator string(UserId userId) => userId.ToString();
}

public partial struct UserId : IEquatable<UserId>
{
    /// <inheritdoc />
    public bool Equals(UserId other) => StringComparer.Ordinal.Equals(Value, other.Value);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is UserId other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.Ordinal.GetHashCode(Value);

    /// <summary>
    /// Equality check.
    /// </summary>
    public static bool operator ==(UserId left, UserId right) => left.Equals(right);

    /// <summary>
    /// Equality check.
    /// </summary>
    public static bool operator !=(UserId left, UserId right) => !(left == right);
}