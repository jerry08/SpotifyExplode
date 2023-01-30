using System.Runtime.Serialization;

namespace SpotifyExplode.Albums;

/// <summary>
/// Spotify's album types
/// </summary>
public enum AlbumType
{
    /// <summary>
    /// Album
    /// </summary>
    [EnumMember(Value = "album")]
    Album,

    /// <summary>
    /// Single
    /// </summary>
    [EnumMember(Value = "single")]
    Single,

    /// <summary>
    /// Compilation
    /// </summary>
    [EnumMember(Value = "compilation")]
    Compilation
}