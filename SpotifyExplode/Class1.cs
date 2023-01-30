using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using SpotifyAPI.Web;
//using SpotifyExplode.Utils;
using Leaf.xNet;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

namespace SpotifyExplode
{
    public class Class1
    {
        string clientId = "694d8bf4f6ec420fa66ea7fb4c68f89d";
        string clientSecret = "02ca2d4021a7452dae2328b47a6e8fe8";

        public Class1()
        {

        }

        string trackUrl = "https://open.spotify.com/track/0VjIjW4GlUZAMYd2vXMi3b";
        string albumUrl = "https://open.spotify.com/album/4yP0hdKOZPNshxUOjY0cZj";
        //string playlistUrl = "https://open.spotify.com/playlist/37i9dQZF1E8UXBoz02kGID";
        string playlistUrl = "https://open.spotify.com/playlist/0tSYjDUflcozy78WwUFe6y";
        string artistUrl = "https://open.spotify.com/artist/1fZAAHNWdSM5gqbi9o5iEA";
        string searchQuery = "The Weeknd - Blinding Lights";

        public void Test1()
        {
            //string url = "https://open.spotify.com/playlist/0tSYjDUflcozy78WwUFe6y?si=3870dcfb32bf4ab8";
            //string url = "https://open.spotify.com/track/1rKWyi67TbzcHHrsbENamE?si=3ee87b7e00014a27";
            var playlistId = "0tSYjDUflcozy78WwUFe6y";
            //string playlist = Http.Get("https://api.spotify.com/v1/playlists/" + url2 + "/tracks?offset=0&limit=100").ToString();

            HttpRequest tokenRequest = new HttpRequest();
            tokenRequest.UserAgent = Http.ChromeUserAgent();
            var tokenJson = tokenRequest.Get("https://open.spotify.com/get_access_token?reason=transport&productType=web_player").ToString();

            var spotifyJsonToken = JObject.Parse(tokenJson);

            var spotifyToken = spotifyJsonToken.SelectToken("accessToken").ToString();

            HttpRequest getSpotifyPlaylist = new HttpRequest();
            getSpotifyPlaylist.AddHeader("Authorization", "Bearer " + spotifyToken);
            var playlist = getSpotifyPlaylist.Get("https://api.spotify.com/v1/playlists/" + playlistId + "/tracks?offset=0&limit=100").ToString();
            //string trackTest = SpotifyExplode.Utils.Http.GetHtml("https://api.spotify.com/v1/tracks/1rKWyi67TbzcHHrsbENamE",
            //    new WebHeaderCollection()
            //    {
            //        { "Accept", "application/json" },
            //        { "Content-Type", "application/json" },
            //        { "Authorization", "Bearer " + spotifyToken },
            //    }
            //);

            JObject jobject = JObject.Parse(playlist);

            var playlistArray = JArray.Parse(jobject.SelectToken("items").ToString());

            for (int i = 0; i < playlistArray.Count; i++)
            {
                HttpRequest w = new HttpRequest();
                //listView_SongsList.Items.Add(playlistArray[i].SelectToken("track").SelectToken("name").ToString(), i);
                try
                {
                    var response = w.Get(playlistArray[i].SelectToken("track").SelectToken("album").SelectToken("images")[0].SelectToken("url").ToString());
                    byte[] imageBytes = response.ToBytes();
                    MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                    //Image imgs = Image.FromStream(memoryStream, true);
                    //dowloadedImages.Images.Add(imgs);
                }
                catch
                {
                    var response = w.Get("https://upload.wikimedia.org/wikipedia/commons/thumb/a/ac/No_image_available.svg/600px-No_image_available.svg.png");
                    byte[] imageBytes = response.ToBytes();
                    MemoryStream memoryStream = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    memoryStream.Write(imageBytes, 0, imageBytes.Length);
                    //Image imgs = Image.FromStream(memoryStream, true);
                    //dowloadedImages.Images.Add(imgs);
                }

                var downloadPath = Environment.CurrentDirectory + @"\Downloads";
                var client = new WebClient();
                client.DownloadFile(playlistArray[i].SelectToken("track").SelectToken("album").SelectToken("images")[0].SelectToken("url").ToString(), downloadPath + @"\thumb.jpg");

                string songName = playlistArray[i].SelectToken("track").SelectToken("name").ToString();
                string artists = playlistArray[i].SelectToken("track").SelectToken("artists")[0].SelectToken("name").ToString();
                string songAlbum = playlistArray[i].SelectToken("track").SelectToken("album").SelectToken("name").ToString();

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                process.StartInfo.FileName = Environment.CurrentDirectory + @"\\youtube-dl.exe";

                process.StartInfo.Arguments = "-x --no-continue " + "\"" + "ytsearch1: " + songName + " " + artists + "\" " + "--audio-format mp3 --audio-quality 0 -o " + "/Downloads/" + "\"" + songName + " - " + songAlbum + "\"" + "." + "%(ext)s";
                process.Start();
                process.WaitForExit();
            }
        }
    }
}