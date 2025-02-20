using System.Text.Json;
using Raylib_cs;

namespace UltimateTicTacToe.Serialization.Json;
public class RandomConverter : System.Text.Json.Serialization.JsonConverter<Random>
{
    public override Random Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.Number)
            throw new JsonException("Expected to decode a number");
        var seed = reader.GetInt32();
        return new Random(seed);
    }
    public override void Write(Utf8JsonWriter writer, Random random, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(random.Next());
    }
}