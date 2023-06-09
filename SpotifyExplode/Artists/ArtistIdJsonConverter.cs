using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyExplode.Artists;

internal class ArtistIdJsonConverter : JsonConverter<ArtistId>
{
    public override ArtistId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var val = reader.GetString();
        var result = ArtistId.TryParse(val);

        return result ?? throw new InvalidOperationException($"Invalid JSON for type '{typeToConvert.FullName}'.");
    }

    public override void Write(Utf8JsonWriter writer, ArtistId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}