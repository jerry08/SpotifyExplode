using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using SpotifyExplode.Exceptions;
using SpotifyExplode.Utils;

namespace SpotifyExplode.Search;

/// <summary>
/// Operations related to Spotify search.
/// </summary>
public class SearchClient
{
    private readonly SpotifyHttp _spotifyHttp;

    /// <summary>
    /// Initializes an instance of <see cref="SearchClient" />.
    /// </summary>
    public SearchClient(HttpClient http) =>
        _spotifyHttp = new SpotifyHttp(http);

    /// <summary>
    /// Gets the metadata associated with the specified artist.
    /// </summary>
    public async ValueTask<List<ISearchResult>> GetResultsAsync(
        string query,
        SearchFilter searchFilter = SearchFilter.Track,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default)
    {
        if (limit is < Constants.MinLimit or > Constants.MaxLimit)
            throw new SpotifyExplodeException($"Limit must be between {Constants.MinLimit} and {Constants.MaxLimit}");

        //query = query.Replace(" ", "%");
        query = Uri.EscapeDataString(query);

        var searchFilterStr = searchFilter switch
        {
            SearchFilter.Track => "&type=track",
            SearchFilter.Album => "&type=album",
            SearchFilter.Artist => "&type=artist",
            SearchFilter.Playlist => "&type=playlist",

            //Implement later
            //SearchFilter.Show => "&type=show",
            //SearchFilter.Episode => "&type=episode",
            //SearchFilter.AudioBook => "&type=audiobook",

            _ => ""
        };

        var results = new List<ISearchResult>();

        var response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/search?q={query}{searchFilterStr}&market=us&limit={limit}&offset={offset}",
            cancellationToken
        );

        switch (searchFilter)
        {
            case SearchFilter.Album:
                var albums = JsonNode.Parse(response)!["albums"]!["items"]!.ToString();
                results.AddRange(JsonSerializer.Deserialize<List<AlbumSearchResult>>(albums, JsonDefaults.Options)!);
                break;

            case SearchFilter.Artist:
                var artists = JsonNode.Parse(response)!["artists"]!["items"]!.ToString();
                results.AddRange(JsonSerializer.Deserialize<List<ArtistSearchResult>>(artists, JsonDefaults.Options)!);
                break;

            case SearchFilter.Playlist:
                var playlists = JsonNode.Parse(response)!["playlists"]!["items"]!.ToString();
                results.AddRange(JsonSerializer.Deserialize<List<PlaylistSearchResult>>(playlists, JsonDefaults.Options)!);
                break;

            case SearchFilter.Track:
                var tracks = JsonNode.Parse(response)!["tracks"]!["items"]!.ToString();
                results.AddRange(JsonSerializer.Deserialize<List<TrackSearchResult>>(tracks, JsonDefaults.Options)!);
                break;
        }

        return results;
    }

    /// <summary>
    /// Gets album search results returned by the specified query.
    /// </summary>
    public async ValueTask<List<AlbumSearchResult>> GetAlbumsAsync(
        string query,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default) =>
        (await GetResultsAsync(query, SearchFilter.Album, offset, limit, cancellationToken))
            .OfType<AlbumSearchResult>().ToList();

    /// <summary>
    /// Gets artist search results returned by the specified query.
    /// </summary>
    public async ValueTask<List<ArtistSearchResult>> GetArtistsAsync(
        string query,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default) =>
        (await GetResultsAsync(query, SearchFilter.Artist, offset, limit, cancellationToken))
            .OfType<ArtistSearchResult>().ToList();

    /// <summary>
    /// Gets playlist search results returned by the specified query.
    /// </summary>
    public async ValueTask<List<PlaylistSearchResult>> GetPlaylistsAsync(
        string query,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default) =>
        (await GetResultsAsync(query, SearchFilter.Playlist, offset, limit, cancellationToken))
            .OfType<PlaylistSearchResult>().ToList();

    /// <summary>
    /// Gets track search results returned by the specified query.
    /// </summary>
    public async ValueTask<List<TrackSearchResult>> GetTracksAsync(
        string query,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default) =>
        (await GetResultsAsync(query, SearchFilter.Track, offset, limit, cancellationToken))
            .OfType<TrackSearchResult>().ToList();
}