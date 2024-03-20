namespace SpotifyExplode.Exceptions;

/// <summary>
/// Exception thrown when Spotify denies a request because the client has exceeded rate limit.
/// </summary>
/// <remarks>
/// Initializes an instance of <see cref="RequestLimitExceededException"/>.
/// </remarks>
public class RequestLimitExceededException(string message) : SpotifyExplodeException(message)
{
}