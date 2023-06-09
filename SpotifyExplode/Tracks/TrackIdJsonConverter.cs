using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyExplode.Tracks;

internal class TrackIdJsonConverter : JsonConverter<TrackId>
{
    public override TrackId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var val = reader.GetString();
        var result = TrackId.TryParse(val);

        return result ?? throw new InvalidOperationException($"Invalid JSON for type '{typeToConvert.FullName}'.");
    }

    public override void Write(Utf8JsonWriter writer, TrackId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}