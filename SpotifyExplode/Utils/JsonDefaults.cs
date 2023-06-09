using System.Text.Json;
using System.Text.Json.Serialization;
using SpotifyExplode.Albums;
using SpotifyExplode.Artists;
using SpotifyExplode.Playlists;
using SpotifyExplode.Tracks;
using SpotifyExplode.Users;

namespace SpotifyExplode.Utils;

internal static class JsonDefaults
{
    public static JsonSerializerOptions Options => GetOptions();

    private static JsonSerializerOptions GetOptions()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new TrackIdJsonConverter());
        options.Converters.Add(new PlaylistIdJsonConverter());
        options.Converters.Add(new AlbumIdJsonConverter());
        options.Converters.Add(new ArtistIdJsonConverter());
        options.Converters.Add(new UserIdJsonConverter());

        return options;
    }
}