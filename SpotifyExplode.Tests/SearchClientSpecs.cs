using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SpotifyExplode.Search;
using Xunit;

namespace SpotifyExplode.Tests;

public class SearchClientSpecs
{
    [Theory]
    [InlineData("This Is The Weeknd", "37i9dQZF1DX6bnzK9KPvrz")]
    [InlineData("Linkin Park Greatest Hits", "0aoCbwrqubaRzYJYZXYcBt")]
    public async Task I_can_search_for_playlists(string query, string id)
    {
        // Arrange
        var spotify = new SpotifyClient();

        // Act
        List<PlaylistSearchResult> results = await spotify.Search.GetPlaylistsAsync(query, 0, 50);

        var playList = results.FirstOrDefault(playList => playList.Id == id);

        // Assert
        playList.Should().NotBeNull();
    }

    [Theory]
    [InlineData("The Weeknd After Hours", "4yP0hdKOZPNshxUOjY0cZj")]
    public async Task I_can_search_for_albums(string query, string id)
    {
        // Arrange
        var spotify = new SpotifyClient();

        // Act
        List<AlbumSearchResult> results = await spotify.Search.GetAlbumsAsync(query, 0, 50);

        var album = results.FirstOrDefault(album => album.Id == id);

        // Assert
        album.Should().NotBeNull();
    }

    [Theory]
    [InlineData("The Weeknd", "1Xyo4u8uXC1ZmMpatF05PJ")]
    public async Task I_can_search_for_artists(string query, string id)
    {
        // Arrange
        var spotify = new SpotifyClient();

        // Act
        List<ArtistSearchResult> results = await spotify.Search.GetArtistsAsync(query, 0, 50);

        var artist = results.FirstOrDefault(artist => artist.Id == id);

        // Assert
        artist.Should().NotBeNull();
    }

    [Theory]
    [InlineData("The Weeknd", "0VjIjW4GlUZAMYd2vXMi3b")]
    public async Task I_can_search_for_tracks(string query, string id)
    {
        // Arrange
        var spotify = new SpotifyClient();

        // Act
        List<TrackSearchResult> results = await spotify.Search.GetTracksAsync(query, 0, 50);

        var track = results.FirstOrDefault(artist => artist.Id == id);

        // Assert
        track.Should().NotBeNull();
    }
}
