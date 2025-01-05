using System.Numerics;
using System.Text.Json;

namespace UltimateTicTacToe.Json;
public class Vector2Converter : System.Text.Json.Serialization.JsonConverter<Vector2>
{
    public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected the start of an object");
        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException("Expected property name");

        float x = 0;
        float y = 0;
        while (reader.TokenType != JsonTokenType.EndObject)
        {
            String propertyName = reader.GetString() ?? throw new JsonException("Property name cannot be null");
            reader.Read();
            switch (propertyName)
            {
                case "X":
                    x = reader.GetSingle();
                    break;
                case "Y":
                    y = reader.GetSingle();
                    break;
                default:
                    throw new JsonException($"Unknown property: {propertyName}");
            }
            reader.Read();
        }
        Vector2 vector2 = new(x, y);
        return vector2;
    }
    public override void Write(Utf8JsonWriter writer, Vector2 vector, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("X", vector.X);
        writer.WriteNumber("Y", vector.Y);
        writer.WriteEndObject();
    }
}