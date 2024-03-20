using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SpotifyExplode.Exceptions;
using SpotifyExplode.Utils;
using SpotifyExplode.Utils.Extensions;

namespace SpotifyExplode.Tracks;

/// <summary>
/// Operations related to Spotify tracks.
/// </summary>
public class TrackClient
{
    private readonly HttpClient _http;
    private readonly SpotifyHttp _spotifyHttp;

    /// <summary>
    /// Initializes an instance of <see cref="TrackClient" />.
    /// </summary>
    public TrackClient(HttpClient http)
    {
        _http = http;
        _spotifyHttp = new SpotifyHttp(http);
    }

    /// <summary>
    /// Gets the metadata associated with the specified track.
    /// </summary>
    public async ValueTask<Track> GetAsync(
        TrackId trackId,
        CancellationToken cancellationToken = default)
    {
        var response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/tracks/{trackId}",
            cancellationToken
        );

        return JsonSerializer.Deserialize<Track>(response, JsonDefaults.Options)!;
    }

    /// <summary>
    /// Gets the best match YouTube ID using <see href="https://spotifydown.com/">spotifydown.com</see>.
    /// </summary>
    public async ValueTask<string?> GetYoutubeIdAsync(
        TrackId trackId,
        CancellationToken cancellationToken = default)
    {
        var response = await _http.ExecuteAsync(
            //$"https://api.spotifydown.com/metadata/track/{trackId}",
            $"https://api.spotifydown.com/getId/{trackId}",
            new Dictionary<string, string>()
            {
                { "referer", "https://spotifydown.com/" },
                { "origin", "https://spotifydown.com" }
            },
            cancellationToken
        );

        if (string.IsNullOrEmpty(response))
            return null;

        var data = JsonNode.Parse(response)!;

        _ = bool.TryParse(data["success"]?.ToString(), out var success);

        if (!success)
            throw new SpotifyExplodeException(data["message"]!.ToString());

        return JsonNode.Parse(response)?["id"]!.ToString();
    }

    /// <summary>
    /// Gets the metadata associated with the specified track.
    /// </summary>
    public async ValueTask<string?> GetDownloadUrlAsync(
        TrackId trackId,
        CancellationToken cancellationToken = default)
    {
        var url = "";

        try
        {
            url = await GetSpotifyDownloaderUrlAsync(trackId, cancellationToken);
        }
        catch { }

        // Fallback
        if (string.IsNullOrEmpty(url))
            url = await GetSpotifymateUrlAsync(trackId, cancellationToken);

        return url;
    }

    private async Task<KeyValuePair<string?, string?>> GetSpotifymateToken(CancellationToken cancellationToken)
    {
        var html = await _http.ExecuteAsync(
            "https://spotifymate.com/",
            cancellationToken
        );

        var document = new HtmlDocument();
        document.LoadHtml(html);

        var hiddenInput = document.GetElementbyId("get_video")?.SelectSingleNode("//input[@type=\"hidden\"]")?.Attributes;

        return new KeyValuePair<string?, string?>(hiddenInput?["name"]?.Value, hiddenInput?["value"]?.Value);
    }

    /// <summary>
    /// Gets download link from <see href="https://spotifymate.com">spotifymate.com</see>
    /// </summary>
    public async ValueTask<string?> GetSpotifymateUrlAsync(
        TrackId trackId,
        CancellationToken cancellationToken = default)
    {
        var formContent = new FormUrlEncodedContent(new KeyValuePair<string?, string?>[]
        {
            new("url", $"https://open.spotify.com/track/{trackId}"),
            await GetSpotifymateToken(cancellationToken)
        });

        var response = await _http.PostAsync(
            "https://spotifymate.com/action",
            null,
            formContent,
            cancellationToken
        );

        if (string.IsNullOrEmpty(response))
            return null;

        var document = new HtmlDocument();
        document.LoadHtml(response);

        return document.GetElementbyId("download-block")?
            .SelectSingleNode(".//a")?.Attributes["href"]?.Value;
    }

    /// <summary>
    /// Gets download link from <see href="https://spotify-downloader.com/">spotify-downloader.com</see>
    /// </summary>
    public async ValueTask<string?> GetSpotifyDownloaderUrlAsync(
        TrackId trackId,
        CancellationToken cancellationToken = default)
    {
        var formContent = new FormUrlEncodedContent(new KeyValuePair<string?, string?>[]
        {
            new("link", $"https://open.spotify.com/track/{trackId}")
        });

        var response = await _http.PostAsync(
            "https://api.spotify-downloader.com/",
            null,
            formContent,
            cancellationToken
        );

        return JsonNode.Parse(response)!["audio"]?["url"]?.ToString();
    }
}