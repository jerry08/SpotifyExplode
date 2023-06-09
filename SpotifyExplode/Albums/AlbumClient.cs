using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using SpotifyExplode.Exceptions;
using SpotifyExplode.Tracks;
using SpotifyExplode.Utils;

namespace SpotifyExplode.Albums;

/// <summary>
/// Operations related to Spotify albums.
/// </summary>
public class AlbumClient
{
    private readonly SpotifyHttp _spotifyHttp;

    /// <summary>
    /// Initializes an instance of <see cref="AlbumClient" />.
    /// </summary>
    public AlbumClient(HttpClient http) =>
        _spotifyHttp = new SpotifyHttp(http);

    /// <summary>
    /// Gets the metadata associated with the specified album.
    /// </summary>
    public async ValueTask<Album> GetAsync(
        AlbumId albumId,
        CancellationToken cancellationToken = default)
    {
        var response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/albums/{albumId}",
            cancellationToken
        );

        var album = JsonSerializer.Deserialize<Album>(response, JsonDefaults.Options)!;

        var items = JsonNode.Parse(response)!["tracks"]?["items"]?.ToString();
        if (!string.IsNullOrEmpty(items))
        {
            var albumTracks = JsonSerializer.Deserialize<List<Track>>(items!, JsonDefaults.Options)!;
            albumTracks.ForEach(track => track.Album = album);
            album.Tracks = albumTracks;
        }

        return album;
    }

    /// <summary>
    /// Gets the metadata associated with the tracks in a specified album.
    /// </summary>
    public async ValueTask<List<Track>> GetTracksAsync(
        AlbumId albumId,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default)
    {
        if (limit is < Constants.MinLimit or > Constants.MaxLimit)
            throw new SpotifyExplodeException($"Limit must be between {Constants.MinLimit} and {Constants.MaxLimit}");

        var response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/albums/{albumId}/tracks?offset={offset}&limit={limit}",
            cancellationToken
        );

        var albumTracks = JsonNode.Parse(response)!["items"]!.ToString();

        return JsonSerializer.Deserialize<List<Track>>(albumTracks, JsonDefaults.Options)!;
    }

    /// <summary>
    /// Gets the metadata associated with the tracks in a specified album.
    /// </summary>
    public async ValueTask<List<Track>> GetAllTracksAsync(
        AlbumId albumId,
        CancellationToken cancellationToken = default)
    {
        var albumtracks = new List<Track>();

        var offset = 0;

        while (true)
        {
            var tracks = await GetTracksAsync(
                albumId,
                offset,
                Constants.MaxLimit,
                cancellationToken
            );

            albumtracks.AddRange(tracks);

            if (tracks.Count < 4)
                break;

            offset += tracks.Count;
        }

        return albumtracks;
    }
}