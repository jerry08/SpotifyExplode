using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SpotifyExplode;

internal class SpotifyHttp
{
    private readonly HttpClient _http;

    public SpotifyHttp(HttpClient http)
    {
        _http = http;
    }

    public async ValueTask<string> GetAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        var accessToken = await GetAccessTokenAsync();
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
        return await SendHttpRequestAsync(request, cancellationToken);
    }

    public async ValueTask<string> SendHttpRequestAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        // User-agent
        if (!request.Headers.Contains("User-Agent"))
        {
            request.Headers.Add(
                "User-Agent",
                Http.ChromeUserAgent()
            );
        }

        using var response = await _http.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Response status code does not indicate success: {(int)response.StatusCode} ({response.StatusCode})." +
                Environment.NewLine +
                "Request:" +
                Environment.NewLine +
                request
            );
        }

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    private async ValueTask<string> GetAccessTokenAsync(
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "https://open.spotify.com/get_access_token?reason=transport&productType=web_player"
        );

        var tokenJson = await SendHttpRequestAsync(request, cancellationToken);

        var spotifyJsonToken = JObject.Parse(tokenJson);

        return spotifyJsonToken.SelectToken("accessToken")!.ToString();
    }
}