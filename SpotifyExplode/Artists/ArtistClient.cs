using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using SpotifyExplode.Albums;
using SpotifyExplode.Exceptions;
using SpotifyExplode.Utils;

namespace SpotifyExplode.Artists;

/// <summary>
/// Operations related to Spotify artists.
/// </summary>
public class ArtistClient
{
    private readonly SpotifyHttp _spotifyHttp;

    /// <summary>
    /// Initializes an instance of <see cref="ArtistClient" />.
    /// </summary>
    public ArtistClient(HttpClient http) =>
        _spotifyHttp = new SpotifyHttp(http);

    /// <summary>
    /// Gets the metadata associated with the specified artist.
    /// </summary>
    public async ValueTask<Artist> GetAsync(
        ArtistId artistId,
        CancellationToken cancellationToken = default)
    {
        var response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/artists/{artistId}",
            cancellationToken
        );

        return JsonSerializer.Deserialize<Artist>(response, JsonDefaults.Options)!;
    }

    /// <summary>
    /// Gets the metadata associated with the albums in a specified artist.
    /// </summary>
    public async ValueTask<List<Album>> GetAlbumsAsync(
        ArtistId artistId,
        AlbumType? albumType = null,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default)
    {
        if (limit is < Constants.MinLimit or > Constants.MaxLimit)
            throw new SpotifyExplodeException($"Limit must be between {Constants.MinLimit} and {Constants.MaxLimit}");

        var response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/artists/{artistId}/albums?{(albumType is not null ? $"album_type={albumType.ToString()!.ToUpper()}&" : "")}offset={offset}&limit={limit}",
            cancellationToken
        );

        var artistAlbums = JsonNode.Parse(response)!["items"]!.ToString();

        return JsonSerializer.Deserialize<List<Album>>(artistAlbums, JsonDefaults.Options)!;
    }

    /// <summary>
    /// Gets the metadata associated with the albums in a specified artist.
    /// </summary>
    public async ValueTask<List<Album>> GetAllAlbumsAsync(
        ArtistId artistId,
        AlbumType? albumType = null,
        CancellationToken cancellationToken = default)
    {
        var artistAlbums = new List<Album>();

        var offset = 0;

        while (true)
        {
            var albums = await GetAlbumsAsync(
                artistId,
                albumType,
                offset,
                Constants.MaxLimit,
                cancellationToken
            );

            artistAlbums.AddRange(albums);

            if (albums.Count < 4)
                break;

            offset += albums.Count;
        }

        return artistAlbums;
    }
}