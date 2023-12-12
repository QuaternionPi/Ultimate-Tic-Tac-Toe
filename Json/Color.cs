using System.Numerics;
using System.Text.Json;
using Raylib_cs;

namespace UltimateTicTacToe
{
    namespace Json
    {
        public class ColorConverter : System.Text.Json.Serialization.JsonConverter<Color>
        {
            public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.String)
                    throw new JsonException("Expected to decode a string");
                string text = reader.GetString() ?? throw new JsonException("String cannot be null");
                if (text.Length != 7 && text.Length != 9) throw new JsonException($"Colors must be 7 or 9, not: {text}");

                string rText = text[1..3];
                string gText = text[3..5];
                string bText = text[5..7];
                int r = int.Parse(rText, System.Globalization.NumberStyles.HexNumber);
                int g = int.Parse(gText, System.Globalization.NumberStyles.HexNumber);
                int b = int.Parse(bText, System.Globalization.NumberStyles.HexNumber);
                int a = 255;
                if (text.Length == 9)
                {
                    string aText = text[7..9];
                    a = int.Parse(aText, System.Globalization.NumberStyles.HexNumber);
                }
                Color color = new(r, g, b, a);
                return color;
            }
            public override void Write(Utf8JsonWriter writer, Color color, JsonSerializerOptions options)
            {
                string text = $"#{color.r:X}{color.g:X}{color.b:X}{color.a:X}";
                writer.WriteStringValue(text);
            }
        }
    }
}