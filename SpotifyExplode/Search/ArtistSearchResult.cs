using SpotifyExplode.Artists;

namespace SpotifyExplode.Search;

public class ArtistSearchResult : Artist, ISearchResult
{
    public string? Url => Id;

    public string? Title => Name;
}