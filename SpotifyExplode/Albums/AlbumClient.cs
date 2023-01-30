using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyExplode.Tracks;
using SpotifyExplode.Exceptions;
using System.Net.Http;

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

        var album = JsonConvert.DeserializeObject<Album>(response)!;

        var items = JObject.Parse(response)["tracks"]?["items"]?.ToString();
        if (!string.IsNullOrEmpty(items))
        {
            var albumTracks = JsonConvert.DeserializeObject<List<Track>>(items!)!;
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

        var albumTracks = JObject.Parse(response)["items"]!.ToString();

        return JsonConvert.DeserializeObject<List<Track>>(albumTracks)!;
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