using System.Threading.Tasks;
using FluentAssertions;
using SpotifyExplode.Users;
using Xunit;

namespace SpotifyExplode.Tests;

public class IdSpecs
{
    /*
     * https://open.spotify.com/user/kasper.spotify
     * Some users have dots on their userId, moreover,
     * there are weird characters allowed ... like 'ð' in jloðbrók
     * https://open.spotify.com/user/jlo%C3%B0br%C3%B3k
     */
    [Theory]
    [InlineData("xxu0yww90v07gbh9veqta7ze0")]
    [InlineData("https://open.spotify.com/user/xxu0yww90v07gbh9veqta7ze0")]
    [InlineData("kasper.spotify")]
    [InlineData("https://open.spotify.com/user/kasper.spotify")]
    [InlineData("jloðbrók")]
    [InlineData("https://open.spotify.com/user/jlo%C3%B0br%C3%B3k")]
    public async Task I_can_parse_user_ids(string userIdStr)
    {
        // Act
        UserId? userId = UserId.TryParse(userIdStr);

        // Assert
        userId.Should().NotBeNull();

        userId!.Value.Value.Should().NotBeNullOrWhiteSpace();
    }
}
