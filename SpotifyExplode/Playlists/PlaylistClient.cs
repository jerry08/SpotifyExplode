﻿using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using SpotifyExplode.Exceptions;
using SpotifyExplode.Tracks;
using SpotifyExplode.Users;
using SpotifyExplode.Utils;

namespace SpotifyExplode.Playlists;

/// <summary>
/// Operations related to Spotify playlists.
/// </summary>
/// <remarks>
/// Initializes an instance of <see cref="PlaylistClient" />.
/// </remarks>
public class PlaylistClient(HttpClient http)
{
    private readonly SpotifyHttp _spotifyHttp = new(http);

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

        var playlistJObj = JsonNode.Parse(response);
        var tracksItems = playlistJObj!["tracks"]?["items"]?.ToString() ?? playlistJObj["items"]!.ToString();

        var playlist = JsonSerializer.Deserialize<Playlist>(response, JsonDefaults.Options)!;
        playlist.Items = JsonSerializer.Deserialize<List<Item>>(tracksItems, JsonDefaults.Options)!;

        return playlist;
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

        var playlistJObj = JsonNode.Parse(response);

        var tracksItems = playlistJObj!["tracks"]?["items"]?.ToString()
            ?? playlistJObj["items"]?.ToString();

        var list = new List<Item>();

        if (string.IsNullOrEmpty(tracksItems))
            return list;

        foreach (var token in JsonNode.Parse(tracksItems!)!.AsArray())
        {
            var item = JsonSerializer.Deserialize<Item>(token!.ToString(), JsonDefaults.Options)!;

            var userId = token["added_by"]?["id"]?.ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                item.AddedBy = JsonSerializer.Deserialize<User>(
                    token["added_by"]!.ToString(),
                    JsonDefaults.Options
                )!;
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