using System;
using System.Threading.Tasks;
using SpotifyExplode;
using SpotifyExplode.Search;
using SpotifyExplode.Tracks;

namespace SpotifyExplode.DemoConsole;

public static class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "SpotifyExplode Demo";

        var spotify = new SpotifyClient();

        // Get the track ID
        Console.Write("Enter Spotify track ID or URL: ");
        var trackId = TrackId.Parse(Console.ReadLine() ?? "");

        var track = await spotify.Tracks.GetAsync(trackId);

        Console.WriteLine($"Title: {track.Title}");
        Console.WriteLine($"Duration (milliseconds): {track.DurationMs}");
        Console.WriteLine($"{track.Album}");
        Console.ReadLine();
    }
}