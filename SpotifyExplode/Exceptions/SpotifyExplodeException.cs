using System;

namespace SpotifyExplode.Exceptions;

/// <summary>
/// Exception thrown within <see cref="SpotifyExplode"/>.
/// </summary>
public class SpotifyExplodeException : Exception
{
    /// <summary>
    /// Initializes an instance of <see cref="SpotifyExplodeException"/>.
    /// </summary>
    /// <param name="message"></param>
    public SpotifyExplodeException(string message) : base(message)
    {
    }
}