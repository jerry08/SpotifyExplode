using Newtonsoft.Json;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyExplode.Tracks;

/// <summary>
/// Operations related to Spotify tracks.
/// </summary>
public class TrackClient
{
    private readonly SpotifyHttp _spotifyHttp;

    /// <summary>
    /// Initializes an instance of <see cref="TrackClient" />.
    /// </summary>
    public TrackClient(HttpClient http) =>
        _spotifyHttp = new SpotifyHttp(http);

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

        return JsonConvert.DeserializeObject<Track>(response)!;
    }
}