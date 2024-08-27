using System;
using System.Collections.Generic;
using System.Diagnostics;
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
/// <remarks>
/// Initializes an instance of <see cref="TrackClient" />.
/// </remarks>
public class TrackClient(HttpClient http)
{
    private readonly SpotifyHttp _spotifyHttp = new(http);

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
    [Obsolete("This method doesn't work anymore because api.spotifydown.com has changed")]
    public async ValueTask<string?> GetYoutubeIdAsync(
        TrackId trackId,
        CancellationToken cancellationToken = default)
    {
        var response = await http.ExecuteAsync(
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
        var url = string.Empty;

        try
        {
            url = await GetSpotifyDownUrlAsync(trackId, cancellationToken);
        }
        catch (SpotifyExplodeException)
        {
            if (!Debugger.IsAttached)
                throw;
        }
        catch
        {
        }

        // Fallback
        if (string.IsNullOrEmpty(url))
            url = await GetSpotifymateUrlAsync(trackId, cancellationToken);

        return url;
    }

    private async Task<KeyValuePair<string?, string?>> GetSpotifymateToken(CancellationToken cancellationToken)
    {
        var html = await http.ExecuteAsync(
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

        var response = await http.PostAsync(
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
    /// Gets download link from <see href="https://spotifydown.com/">spotifydown.com</see>
    /// </summary>
    public async ValueTask<string?> GetSpotifyDownUrlAsync(
        TrackId trackId,
        CancellationToken cancellationToken = default)
    {
        var response = await http.ExecuteAsync(
            $"https://api.spotifydown.com/download/{trackId}",
            new Dictionary<string, string>()
            {
                { "referer", "https://spotifydown.com/" },
                { "origin", "https://spotifydown.com" },
                { "host", "api.spotifydown.com" },
            },
            cancellationToken
        );

        if (string.IsNullOrEmpty(response))
            return null;

        var data = JsonNode.Parse(response)!;

        _ = bool.TryParse(data["success"]?.ToString(), out var success);

        if (!success)
            throw new SpotifyExplodeException(data["message"]!.ToString());

        return JsonNode.Parse(response)?["link"]!.ToString();
    }
}