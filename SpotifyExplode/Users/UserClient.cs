using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using SpotifyExplode.Utils;

namespace SpotifyExplode.Users;

/// <summary>
/// Operations related to Spotify users.
/// </summary>
/// <remarks>
/// Initializes an instance of <see cref="UserClient" />.
/// </remarks>
public class UserClient(HttpClient http)
{
    private readonly SpotifyHttp _spotifyHttp = new(http);

    /// <summary>
    /// Gets the metadata associated with the specified user.
    /// </summary>
    public async ValueTask<User> GetAsync(
        UserId userId,
        CancellationToken cancellationToken = default)
    {
        var response = await _spotifyHttp.GetAsync(
            $"https://api.spotify.com/v1/users/{userId}",
            cancellationToken
        );

        return JsonSerializer.Deserialize<User>(response, JsonDefaults.Options)!;
    }
}