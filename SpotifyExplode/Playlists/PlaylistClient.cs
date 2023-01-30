using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyExplode.Exceptions;
using SpotifyExplode.Tracks;

namespace SpotifyExplode.Playlists;

/// <summary>
/// Operations related to Spotify playlists.
/// </summary>
public class PlaylistClient
{
    private readonly SpotifyHttp _spotifyHttp;

    /// <summary>
    /// Initializes an instance of <see cref="PlaylistClient" />.
    /// </summary>
    public PlaylistClient(HttpClient http) =>
        _spotifyHttp = new SpotifyHttp(http);

    /// <summary>
    /// Gets the metadata associated with the specified playlist.
    /// </summary>
    public async ValueTask<Playlist> GetAsync(
        PlaylistId playlistId,
        CancellationToken cancellationToken = default)
    {
        var response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/playlists/{playlistId}",
            cancellationToken
        );

        var playlistJObj = JObject.Parse(response);
        var tracksItems = playlistJObj["tracks"]?["items"]?.ToString() ?? playlistJObj["items"]!.ToString();

        var palylist = JsonConvert.DeserializeObject<Playlist>(response)!;
        palylist.Items = JsonConvert.DeserializeObject<List<Item>>(tracksItems)!;

        return palylist;
    }

    /// <summary>
    /// Gets the tracks associated with the specified playlist.
    /// </summary>
    /// <param name="playlistId"></param>
    /// <param name="offset"></param>
    /// <param name="limit">Limit should not exceed 100 according to Spotify</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="RequestLimitExceededException"></exception>
    public async ValueTask<List<Item>> GetItemsAsync(
        PlaylistId playlistId,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default)
    {
        if (limit is < Constants.MinLimit or > Constants.MaxLimit)
            throw new SpotifyExplodeException($"Limit must be between {Constants.MinLimit} and {Constants.MaxLimit}");

        var playlist = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/playlists/{playlistId}/tracks?offset={offset}&limit={limit}",
            cancellationToken
        );

        var playlistJObj = JObject.Parse(playlist);
        var tracksItems = JObject.Parse(playlist)["tracks"]?["items"]?.ToString() ?? playlistJObj["items"]?.ToString();

        if (tracksItems is null)
            return new();

        return JsonConvert.DeserializeObject<List<Item>>(tracksItems)!;
    }

    public async ValueTask<List<Item>> GetAllItemsAsync(
        PlaylistId playlistId,
        CancellationToken cancellationToken = default)
    {
        var playlistItems = new List<Item>();

        var offset = 0;

        while (true)
        {
            var tracks = await GetItemsAsync(
                playlistId,
                offset,
                Constants.MaxLimit,
                cancellationToken
            );

            playlistItems.AddRange(tracks);

            if (tracks.Count < 4)
                break;

            offset += tracks.Count;
        }

        return playlistItems;
    }

    public async ValueTask<List<Track>> GetTracksAsync(
        PlaylistId playlistId,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default) =>
        (await GetItemsAsync(playlistId, offset, limit, cancellationToken))
            .ConvertAll(x => x.Track);

    public async ValueTask<List<Track>> GetAllTracksAsync(
        PlaylistId playlistId,
        CancellationToken cancellationToken = default) =>
        (await GetAllItemsAsync(playlistId, cancellationToken)).ConvertAll(x => x.Track);
}