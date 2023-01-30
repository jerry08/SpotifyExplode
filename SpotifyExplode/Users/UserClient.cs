using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyExplode.Users;

/// <summary>
/// Operations related to Spotify users.
/// </summary>
public class UserClient
{
    private readonly SpotifyHttp _spotifyHttp;

    /// <summary>
    /// Initializes an instance of <see cref="UserClient" />.
    /// </summary>
    public UserClient(HttpClient http) =>
        _spotifyHttp = new SpotifyHttp(http);

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

        return JsonConvert.DeserializeObject<User>(response)!;
    }
}