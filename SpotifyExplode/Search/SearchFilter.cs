namespace SpotifyExplode.Search;

/// <summary>
/// Filter applied to a Spotify search query.
/// </summary>
public enum SearchFilter
{
    /// <summary>
    /// Only search for albums.
    /// </summary>
    Album,

    /// <summary>
    /// Only search for artists.
    /// </summary>
    Artist,

    /// <summary>
    /// Only search for playlists.
    /// </summary>
    Playlist,

    /// <summary>
    /// Only search for tracks.
    /// </summary>
    Track,

    //Implement later

    /*/// <summary>
    /// Only search for shows.
    /// </summary>
    Show,

    /// <summary>
    /// Only search for episodes.
    /// </summary>
    Episode,

    /// <summary>
    /// Only search for audiobooks.
    /// </summary>
    AudioBook*/
}