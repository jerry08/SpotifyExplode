using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyExplode.Playlists;

internal class PlaylistIdJsonConverter : JsonConverter<PlaylistId>
{
    public override PlaylistId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var val = reader.GetString();
        var result = PlaylistId.TryParse(val);

        return result ?? throw new InvalidOperationException($"Invalid JSON for type '{typeToConvert.FullName}'.");
    }

    public override void Write(Utf8JsonWriter writer, PlaylistId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}