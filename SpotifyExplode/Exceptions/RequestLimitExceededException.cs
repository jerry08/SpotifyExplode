namespace SpotifyExplode.Exceptions;

/// <summary>
/// Exception thrown when Spotify denies a request because the client has exceeded rate limit.
/// </summary>
public class RequestLimitExceededException : SpotifyExplodeException
{
    /// <summary>
    /// Initializes an instance of <see cref="RequestLimitExceededException"/>.
    /// </summary>
    public RequestLimitExceededException(string message) : base(message)
    {
    }
}