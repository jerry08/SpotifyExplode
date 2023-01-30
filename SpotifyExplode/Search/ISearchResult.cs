namespace SpotifyExplode.Search;

/// <summary>
/// <p>
///     Abstract result returned by a search query.
///     Use pattern matching to handle specific instances of this type.
/// </p>
/// <p>
///     Can be either one of the following:
///     <list type="bullet">
///         <item><see cref="AlbumSearchResult" /></item>
///         <item><see cref="ArtistSearchResult" /></item>
///         <item><see cref="TrackSearchResult" /></item>
///         <item><see cref="PlaylistSearchResult" /></item>
///     </list>
/// </p>
/// </summary>

public interface ISearchResult
{
    /// <summary>
    /// Result URL.
    /// </summary>
    string? Url { get; }

    /// <summary>
    /// Result title.
    /// </summary>
    string? Title { get; }
}