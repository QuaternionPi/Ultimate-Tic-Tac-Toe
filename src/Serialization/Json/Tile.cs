using System.Text.Json;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe.Serialization.Json;
public class TileConverter : System.Text.Json.Serialization.JsonConverter<Tile>
{
    public override Tile Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException("Expected to decode a json object");
        reader.Read();
        if (reader.TokenType != JsonTokenType.PropertyName)
            throw new JsonException("Expected property name");
        var property = reader.GetString();
        if (property == null || property != "Owner")
        {
            throw new JsonException($"Tile requires exactly one property, 'Owner', not '{property}'");
        }
        reader.Read();
        var owner = JsonSerializer.Deserialize<Player.Token>(ref reader, options);
        reader.Read();
        return new Tile(owner);
    }
    public override void Write(Utf8JsonWriter writer, Tile tile, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Owner");
        JsonSerializer.Serialize(writer, tile.Owner, options);
        writer.WriteEndObject();
    }
}