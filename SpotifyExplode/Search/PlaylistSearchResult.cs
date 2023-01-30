using SpotifyExplode.Playlists;

namespace SpotifyExplode.Search;

public class PlaylistSearchResult : Playlist, ISearchResult
{
    public string? Url => Id;

    public string? Title => Name;
}