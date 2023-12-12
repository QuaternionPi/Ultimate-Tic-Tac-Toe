using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe
{
    namespace Json
    {
        public class Transform2DConverter : System.Text.Json.Serialization.JsonConverter<Transform2D>
        {
            public override Transform2D Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException("Expected the start of an object");
                reader.Read();
                if (reader.TokenType != JsonTokenType.PropertyName)
                    throw new JsonException("Expected property name");

                Vector2 position = Vector2.Zero;
                float rotation = 0;
                float scale = 0;
                while (reader.TokenType != JsonTokenType.EndObject)
                {
                    String propertyName = reader.GetString() ?? throw new JsonException("Property name cannot be null");
                    reader.Read();
                    switch (propertyName)
                    {
                        case "Position":
                            position = reader.GetVector2();
                            break;
                        case "Rotation":
                            rotation = reader.GetSingle();
                            break;
                        case "Scale":
                            scale = reader.GetSingle();
                            break;
                        default:
                            throw new JsonException($"Unkown property: {propertyName}");
                    }
                    reader.Read();
                }
                Transform2D transform = new(position, rotation, scale);
                return transform;
            }
            public override void Write(Utf8JsonWriter writer, Transform2D transform, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteVector2("Position", transform.Position);
                writer.WriteNumber("Rotation", transform.Rotation);
                writer.WriteNumber("Scale", transform.Scale);
                writer.WriteEndObject();
            }
        }
    }
}