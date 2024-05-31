using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
        var results = await spotify.Search.GetPlaylistsAsync(query, 0, 50);

        var playList = results.FirstOrDefault(playList => playList.Id == id);

        // Assert
        playList.Should().NotBeNull();
    }
}
