using Newtonsoft.Json;

namespace SpotifyExplode.Common;

public class Image
{
    [JsonProperty("url")]
    public string Url { get; set; } = default!;

    [JsonProperty("height")]
    public int? Height { get; set; }

    [JsonProperty("width")]
    public int? Width { get; set; }
}