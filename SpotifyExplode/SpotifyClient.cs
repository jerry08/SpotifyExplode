using System.Net.Http;
using SpotifyExplode.Albums;
using SpotifyExplode.Artists;
using SpotifyExplode.Playlists;
using SpotifyExplode.Search;
using SpotifyExplode.Tracks;
using SpotifyExplode.Users;

namespace SpotifyExplode;

/// <summary>
/// Client for interacting with Spotify.
/// </summary>
public class SpotifyClient(HttpClient http)
{
    /// <summary>
    /// Operations related to Spotify search.
    /// </summary>
    public SearchClient Search { get; } = new SearchClient(http);

    /// <summary>
    /// Operations related to Spotify tracks.
    /// </summary>
    public TrackClient Tracks { get; } = new TrackClient(http);

    /// <summary>
    /// Operations related to Spotify artists.
    /// </summary>
    public ArtistClient Artists { get; } = new ArtistClient(http);

    /// <summary>
    /// Operations related to Spotify albums.
    /// </summary>
    public AlbumClient Albums { get; } = new AlbumClient(http);

    /// <summary>
    /// Operations related to Spotify playlists.
    /// </summary>
    public PlaylistClient Playlists { get; } = new PlaylistClient(http);

    /// <summary>
    /// Operations related to Spotify users.
    /// </summary>
    public UserClient Users { get; } = new UserClient(http);

    /// <summary>
    /// Initializes an instance of <see cref="SpotifyClient" />.
    /// </summary>
    public SpotifyClient() : this(Http.Client)
    {
    }
}