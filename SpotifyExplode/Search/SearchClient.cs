using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpotifyExplode.Exceptions;

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
                var albums = JObject.Parse(response)["albums"]!["items"]!.ToString();
                results.AddRange(JsonConvert.DeserializeObject<List<AlbumSearchResult>>(albums)!);
                break;

            case SearchFilter.Artist:
                var artists = JObject.Parse(response)["artists"]!["items"]!.ToString();
                results.AddRange(JsonConvert.DeserializeObject<List<ArtistSearchResult>>(artists)!);
                break;

            case SearchFilter.Playlist:
                var playlists = JObject.Parse(response)["playlists"]!["items"]!.ToString();
                results.AddRange(JsonConvert.DeserializeObject<List<PlaylistSearchResult>>(playlists)!);
                break;

            case SearchFilter.Track:
                var tracks = JObject.Parse(response)["tracks"]!["items"]!.ToString();
                results.AddRange(JsonConvert.DeserializeObject<List<TrackSearchResult>>(tracks)!);
                break;
        }

        return results;
    }
}