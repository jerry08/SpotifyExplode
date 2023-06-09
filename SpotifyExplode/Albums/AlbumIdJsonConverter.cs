using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyExplode.Albums;

internal class AlbumIdJsonConverter : JsonConverter<AlbumId>
{
    public override AlbumId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var val = reader.GetString();
        var result = AlbumId.TryParse(val);

        return result ?? throw new InvalidOperationException($"Invalid JSON for type '{typeToConvert.FullName}'.");
    }

    public override void Write(Utf8JsonWriter writer, AlbumId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}