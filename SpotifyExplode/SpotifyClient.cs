using System.Net.Http;
using SpotifyExplode.Search;
using SpotifyExplode.Tracks;
using SpotifyExplode.Albums;
using SpotifyExplode.Artists;
using SpotifyExplode.Users;
using SpotifyExplode.Playlists;

namespace SpotifyExplode;

/// <summary>
/// Client for interacting with Spotify.
/// </summary>
public class SpotifyClient
{
    /// <summary>
    /// Operations related to Spotify search.
    /// </summary>
    public SearchClient Search { get; }

    /// <summary>
    /// Operations related to Spotify tracks.
    /// </summary>
    public TrackClient Tracks { get; }

    /// <summary>
    /// Operations related to Spotify artists.
    /// </summary>
    public ArtistClient Artists { get; }

    /// <summary>
    /// Operations related to Spotify albums.
    /// </summary>
    public AlbumClient Albums { get; }

    /// <summary>
    /// Operations related to Spotify playlists.
    /// </summary>
    public PlaylistClient Playlists { get; }

    /// <summary>
    /// Operations related to Spotify users.
    /// </summary>
    public UserClient Users { get; }

    public SpotifyClient(HttpClient http)
    {
        Search = new SearchClient(http);
        Tracks = new TrackClient(http);
        Artists = new ArtistClient(http);
        Albums = new AlbumClient(http);
        Playlists = new PlaylistClient(http);
        Users = new UserClient(http);
    }

    /// <summary>
    /// Initializes an instance of <see cref="SpotifyClient" />.
    /// </summary>
    public SpotifyClient() : this(Http.Client)
    {
    }
}