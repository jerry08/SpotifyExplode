using System;

namespace SpotifyExplode.Exceptions;

/// <summary>
/// Exception thrown within <see cref="SpotifyExplode"/>.
/// </summary>
/// <remarks>
/// Initializes an instance of <see cref="SpotifyExplodeException"/>.
/// </remarks>
/// <param name="message"></param>
public class SpotifyExplodeException(string message) : Exception(message)
{
}