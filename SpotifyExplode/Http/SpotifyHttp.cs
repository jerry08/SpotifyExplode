using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using SpotifyExplode.Utils.Extensions;

namespace SpotifyExplode;

internal class SpotifyHttp(HttpClient http)
{
    public async ValueTask<string> GetAsync(
        string url,
        CancellationToken cancellationToken = default)
    {
        var accessToken = await GetAccessTokenAsync();
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
        return await http.ExecuteAsync(request, cancellationToken);
    }

    private async ValueTask<string> GetAccessTokenAsync(
        CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            "https://open.spotify.com/get_access_token?reason=transport&productType=web_player"
        );

        var tokenJson = await http.ExecuteAsync(request, cancellationToken);

        var spotifyJsonToken = JsonNode.Parse(tokenJson)!;

        return spotifyJsonToken["accessToken"]!.ToString();
    }
}