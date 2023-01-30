using SpotifyExplode.Albums;

namespace SpotifyExplode.Search;

public class AlbumSearchResult : Album, ISearchResult
{
    public string? Title => Name;
}