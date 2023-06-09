using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpotifyExplode.Users;

internal class UserIdJsonConverter : JsonConverter<UserId>
{
    public override UserId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var val = reader.GetString();
        var result = UserId.TryParse(val);

        return result ?? throw new InvalidOperationException($"Invalid JSON for type '{typeToConvert.FullName}'.");
    }

    public override void Write(Utf8JsonWriter writer, UserId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}