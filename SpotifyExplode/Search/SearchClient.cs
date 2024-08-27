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

using FilterTuple = (string type, Func<JsonNode?, string, IEnumerable<JsonNode>> getItems);

/// <summary>
/// Operations related to Spotify search.
/// </summary>
/// <remarks>
/// Initializes an instance of <see cref="SearchClient" />.
/// </remarks>
public class SearchClient(HttpClient http)
{
    private static readonly IDictionary<Type, FilterTuple> _filters;

    private readonly SpotifyHttp _spotifyHttp = new(http);

    static IEnumerable<JsonNode> GetItems(JsonNode? json, string type)
    {
        return json![$"{type}s"]!["items"]!.AsArray()!;
    }

    static SearchClient()
    {
        SearchClient._filters = new Dictionary<Type, FilterTuple>() {
            { typeof (TrackSearchResult), ("track", GetItems) },
            { typeof (AlbumSearchResult), ("album", GetItems) },
            { typeof (ArtistSearchResult), ("artist", GetItems) },
            { typeof (PlaylistSearchResult), ("playlist", GetItems) }
        };   
    }

    /// <summary>
    /// Gets the metadata associated with the specified artist.
    /// </summary>
    private async ValueTask<List<SR>> GetResultsAsync<SR>(
        string query,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default
    ) where SR : ISearchResult
    {
        if (limit is < Constants.MinLimit or > Constants.MaxLimit)
            throw new SpotifyExplodeException($"Limit must be between {Constants.MinLimit} and {Constants.MaxLimit}");

        //query = query.Replace(" ", "%");

        query = Uri.EscapeDataString(query);

        FilterTuple filter = _filters[typeof (SR)];

        string response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/search?q={query}&type={filter.type}&market=us&limit={limit}&offset={offset}",
            cancellationToken
        );

        return filter.getItems(JsonNode.Parse(response), filter.type)
            .Select(json => JsonSerializer.Deserialize<SR>(json, JsonDefaults.Options)!)
            !.ToList();
    }

    /// <summary>
    /// Gets album search results returned by the specified query.
    /// </summary>
    public async ValueTask<List<AlbumSearchResult>> GetAlbumsAsync(
        string query,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default) =>
        await GetResultsAsync<AlbumSearchResult>(query, offset, limit, cancellationToken);

    /// <summary>
    /// Gets artist search results returned by the specified query.
    /// </summary>
    public async ValueTask<List<ArtistSearchResult>> GetArtistsAsync(
        string query,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default) =>
        await GetResultsAsync<ArtistSearchResult>(query, offset, limit, cancellationToken);

    /// <summary>
    /// Gets playlist search results returned by the specified query.
    /// </summary>
    public async ValueTask<List<PlaylistSearchResult>> GetPlaylistsAsync(
        string query,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default) =>
        await GetResultsAsync<PlaylistSearchResult>(query, offset, limit, cancellationToken);

    /// <summary>
    /// Gets track search results returned by the specified query.
    /// </summary>
    public async ValueTask<List<TrackSearchResult>> GetTracksAsync(
        string query,
        int offset = Constants.DefaultOffset,
        int limit = Constants.DefaultLimit,
        CancellationToken cancellationToken = default) =>
        await GetResultsAsync<TrackSearchResult>(query, offset, limit, cancellationToken);
}