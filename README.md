
<h1 align="center">
    SpotifyExplode
</h1>

<p align="center">
   <a href="https://discord.gg/mhxsSMy2Nf"><img src="https://img.shields.io/badge/Discord-7289DA?style=for-the-badge&logo=discord&logoColor=white"></a>
   <a href="https://nuget.org/packages/SpotifyExplode"><img src="https://img.shields.io/nuget/dt/SpotifyExplode.svg?label=Downloads&color=%233DDC84&logo=nuget&logoColor=%23fff&style=for-the-badge"></a>
</p>

**SpotifyExplode** is a library that provides an interface to query metadata of Spotify tracks, playlists, albums, artists and users as well as to download audio.

### 🌟STAR THIS REPOSITORY TO SUPPORT THE DEVELOPER AND ENCOURAGE THE DEVELOPMENT OF THIS PROJECT!


## Install

- 📦 [NuGet](https://nuget.org/packages/SpotifyExplode): `dotnet add package SpotifyExplode`

## Usage

**SpotifyExplode** exposes its functionality through a single entry point — the `SpotifyClient` class.
Create an instance of this class and use the provided operations to send requests.

### Tracks

#### Retrieving track metadata

To retrieve the metadata associated with a Spotify track, call `Tracks.GetAsync(...)`:

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

var track = await spotify.Tracks.GetAsync(
    "https://open.spotify.com/track/0VjIjW4GlUZAMYd2vXMi3b"
);

var title = track.Title;
var duration = track.DurationMs;
```

### Playlists

#### Retrieving playlist metadata

You can get the metadata associated with a Spotify playlist by calling `Playlists.GetAsync(...)` method:

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

//Get playlist info
var playlist = await spotify.Playlists.GetAsync(
    "https://open.spotify.com/playlist/0tSYjDUflcozy78WwUFe6y"
);

var title = playlist.Name;
var artworkUrl = playlist.Followers;
var tracks = playlist.Tracks;
...
```

#### Getting tracks included in a playlist

To get the tracks included in a playlist, call `Playlists.GetTracksAsync(...)`:

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

// Get all tracks in a playlist
var tracks = await spotify.Playlists.GetAllTracksAsync(
    "https://open.spotify.com/playlist/0tSYjDUflcozy78WwUFe6y"
);

// Get only the first 20 playlist tracks
var tracksSubset = await spotify.Playlists.GetTracksAsync(
    "https://open.spotify.com/playlist/0tSYjDUflcozy78WwUFe6y",
    limit: 20
);

//Setting offset
var tracksSubset = await spotify.Playlists.GetTracksAsync(
    "https://open.spotify.com/playlist/0tSYjDUflcozy78WwUFe6y",
    offset: 3,
    limit: 20
);
```

### Albums

#### Retrieving album metadata

You can get the metadata associated with a Spotify album by calling `Albums.GetAsync(...)` method:

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

//Get album info with all tracks
var album = await spotify.Albums.GetAsync(
    "https://open.spotify.com/album/336m0kejdM5Fkw2HUX46Bw?si=549f3fdb0cfd46e6"
);

var title = album.Name;
var artists = album.Artists;
var tracks = album.Tracks;
...
```

#### Getting tracks included in an album

To get the tracks included in a album, call `Albums.GetTracksAsync(...)`:

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

// Get all tracks in a album
var tracks = await spotify.Albums.GetAllTracksAsync(
    "https://open.spotify.com/album/336m0kejdM5Fkw2HUX46Bw?si=549f3fdb0cfd46e6"
);

// Get only the first 20 album tracks
var tracksSubset = await spotify.Albums.GetTracksAsync(
    "https://open.spotify.com/album/336m0kejdM5Fkw2HUX46Bw?si=549f3fdb0cfd46e6",
    limit: 20
);

//Setting offset
var tracksSubset = await spotify.Albums.GetTracksAsync(
    "https://open.spotify.com/album/336m0kejdM5Fkw2HUX46Bw?si=549f3fdb0cfd46e6",
    offset: 3,
    limit: 20
);
```

### Artists

#### Retrieving artist metadata

You can get the metadata associated with a Spotify artist by calling `Artists.GetAsync(...)` method:

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

//Get artist info
var artist = await spotify.Artists.GetAsync(
    "https://open.spotify.com/artist/0bAsR2unSRpn6BQPEnNlZm?si=d3b6e78f96ce45b9"
);

var title = artist.Name;
var albums = artist.Albums;
```

#### Getting tracks included in an artist

To get the tracks included in an artist, call `Artists.GetAlbumsAsync(...)`:

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

// Get all albums in an artist
var albums = await spotify.Artists.GetAllAlbumsAsync(
    "https://open.spotify.com/artist/0bAsR2unSRpn6BQPEnNlZm?si=d3b6e78f96ce45b9"
);

// Get only the first 20 artist albums
var albumsSubset = await spotify.Artists.GetAlbumsAsync(
    "https://open.spotify.com/artist/0bAsR2unSRpn6BQPEnNlZm?si=d3b6e78f96ce45b9",
    limit: 20
);

//Setting offset
var albumsSubset = await spotify.Artists.GetAlbumsAsync(
    "https://open.spotify.com/artist/0bAsR2unSRpn6BQPEnNlZm?si=d3b6e78f96ce45b9",
    offset: 3,
    limit: 20
);
```

### Users

#### Retrieving user metadata

You can get the metadata associated with a Spotify user by calling `Users.GetAsync(...)` method:

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

//Get user info
var user = await spotify.Users.GetAsync(
    "https://open.spotify.com/user/xxu0yww90v07gbh9veqta7ze0"
);

var name = user.DisplayName;
var followers = user.Followers;
var images = user.Images;
```

### Searching
You can execute a search query and get its results by calling `Search.GetResultsAsync(...)`. Each result may represent either an album, artist, track or playlist, so you need to apply pattern matching to handle the corresponding cases:

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

foreach (var result in await spotify.Search.GetResultsAsync("banda neira"))
{
    // Use pattern matching to handle different results (albums, artists, tracks, playlists)
    switch (result)
    {
        case TrackSearchResult track:
            {
                var id = track.Id;
                var title = track.Title;
                var duration = track.DurationMs;
                break;
            }
        case PlaylistSearchResult playlist:
            {
                var id = playlist.Id;
                var title = playlist.Name;
                break;
            }
        case AlbumSearchResult album:
            {
                var id = album.Id;
                var title = album.Name;
                var artists = album.Artists;
                var tracks = album.Tracks;
                break;
            }
	case ArtistSearchResult artist:
            {
                var id = artist.Id;
                var title = artist.Name;
                break;
            }
    }
}
```

### Downloading
You can get the download url from a track by calling `Tracks.GetDownloadUrlAsync(...)`.

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

var downloadUrl = await spotify.Tracks.GetDownloadUrlAsync(
    "https://open.spotify.com/track/0VjIjW4GlUZAMYd2vXMi3b"
);
// Start download from the url provided...
```

### Extras
You can get a Youtube ID from a track by calling `Tracks.GetYoutubeIdAsync(...)`.

```csharp
using SpotifyExplode;

var spotify = new SpotifyClient();

var youtubeId = await spotify.Tracks.GetYoutubeIdAsync(
    "https://open.spotify.com/track/0VjIjW4GlUZAMYd2vXMi3b"
);

var youtubeUrl = "https://youtube.com/watch?v=" + youtubeId;
```
