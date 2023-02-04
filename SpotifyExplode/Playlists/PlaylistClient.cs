using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyExplode.Exceptions;
using SpotifyExplode.Tracks;
using SpotifyExplode.Users;

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
    /// <exception cref="RequestLimitExceededException"></exception>
    public async ValueTask<List<Item>> GetItemsAsync(
        PlaylistId playlistId,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default)
    {
        if (limit is < Constants.MinLimit or > Constants.MaxLimit)
            throw new SpotifyExplodeException($"Limit must be between {Constants.MinLimit} and {Constants.MaxLimit}");

        var response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/playlists/{playlistId}/tracks?offset={offset}&limit={limit}",
            cancellationToken
        );

        var playlistJObj = JObject.Parse(response);

        var tracksItems = playlistJObj["tracks"]?["items"]?.ToString()
            ?? playlistJObj["items"]?.ToString();

        var list = new List<Item>();

        if (string.IsNullOrEmpty(tracksItems))
            return list;

        foreach (var token in JArray.Parse(tracksItems!))
        {
            var item = JsonConvert.DeserializeObject<Item>(token.ToString())!;

            var userId = token["added_by"]?["id"]?.ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                item.AddedBy = JsonConvert.DeserializeObject<User>(token["added_by"]!.ToString())!;
            }

            list.Add(item);
        }

        return list;
    }

    /// <summary>
    /// Gets the items associated with the specified playlist.
    /// </summary>
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

    /// <summary>
    /// Gets the tracks associated with the specified playlist.
    /// </summary>
    public async ValueTask<List<Track>> GetTracksAsync(
        PlaylistId playlistId,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default) =>
        (await GetItemsAsync(playlistId, offset, limit, cancellationToken))
            .ConvertAll(x => x.Track);

    /// <summary>
    /// Gets all the tracks associated with the specified playlist.
    /// </summary>
    public async ValueTask<List<Track>> GetAllTracksAsync(
        PlaylistId playlistId,
        CancellationToken cancellationToken = default) =>
        (await GetAllItemsAsync(playlistId, cancellationToken)).ConvertAll(x => x.Track);
}