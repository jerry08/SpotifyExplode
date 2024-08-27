using SpotifyExplode.Utils;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyExplode.Users;

/// <summary>
/// Operations related to Spotify users.
/// </summary>
/// <remarks>
/// Initializes an instance of <see cref="UserClient" />.
/// </remarks>
public class UserClient(HttpClient http) {
    private static readonly Regex UserUrlRegex = new Regex(@"^(https?://)?open.spotify.com/user/([^\/]*)/?");

    private readonly SpotifyHttp _spotifyHttp = new(http);

    /// <summary>
    /// Gets the metadata associated with the specified user.
    /// </summary>
    public async ValueTask<User> GetAsync(
        string userIdOrUrl,
        CancellationToken cancellationToken = default) {
        // Regular URL
        // https://open.spotify.com/user/xxu0yww90v07gbh9veqta7ze0
        Match match = UserUrlRegex.Match(userIdOrUrl);
        if (match.Success)
            userIdOrUrl = Uri.UnescapeDataString(match.Groups[match.Groups.Count - 1].Value);

        var response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/users/{userIdOrUrl}",
            cancellationToken
        );

        return JsonSerializer.Deserialize<User>(response, JsonDefaults.Options)!;
    }
}