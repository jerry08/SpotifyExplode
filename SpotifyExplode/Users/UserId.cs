using SpotifyExplode.Utils.Extensions;
using System;
using System.Text.RegularExpressions;

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
    private static readonly Regex UserUrlRegex = new Regex(@"^(https?://)?open.spotify.com/user/([^\/]*)/?");

    private static string? TryNormalize(string? userIdOrUrl)
    {
        if (userIdOrUrl == null)
            return null;

        // Regular URL
        // https://open.spotify.com/user/xxu0yww90v07gbh9veqta7ze0
        Match match = UserUrlRegex.Match(userIdOrUrl);
        if (match.Success)
            userIdOrUrl = Uri.UnescapeDataString(match.Groups[match.Groups.Count - 1].Value);

        if (userIdOrUrl == "")
            return null;

        /* 
         * Id
         * 
         * xxu0yww90v07gbh9veqta7ze0
         * kasper.spotify
         * jloðbrók
         * 
         * The regexp @"[^0-9a-zA-Z_\-]" leaves some ids as invalid
         * I am certainly confused as to why spotifty would allow 'ð' to be part of an id
         * I can't think of any way of validating this, with my limited knowledge of what is a valid spotify user id
         */
        return userIdOrUrl;
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