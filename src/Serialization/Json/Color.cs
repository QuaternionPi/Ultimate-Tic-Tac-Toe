using System.Text.Json;
using Raylib_cs;

namespace UltimateTicTacToe.Serialization.Json
{
    public class ColorConverter : System.Text.Json.Serialization.JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.String)
                throw new JsonException("Expected to decode a string");
            string text = reader.GetString() ?? throw new JsonException("String cannot be null");
            if (text.Length != 7 && text.Length != 9) throw new JsonException($"Colors must be of length 7 (RBG) or 9 (RGBA), not: {text.Length}");

            return text.Length == 7 ? ReadRGB(text) : ReadRGBA(text);
        }
        public override void Write(Utf8JsonWriter writer, Color color, JsonSerializerOptions options)
        {
            string text = $"#{color.r:X2}{color.g:X2}{color.b:X2}{color.a:X2}";
            writer.WriteStringValue(text);
        }
        private static Color ReadRGB(string text)
        {
            if (text.Length != 7)
                throw new ArgumentException($"Reading RGB from a string requires a string of length 7 not {text.Length}");
            string rText = text[1..3];
            string gText = text[3..5];
            string bText = text[5..7];
            int r = int.Parse(rText, System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(gText, System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(bText, System.Globalization.NumberStyles.HexNumber);
            int a = 255;
            return new Color(r, g, b, a);
        }
        private static Color ReadRGBA(string text)
        {
            if (text.Length != 9)
                throw new ArgumentException($"Reading RGB from a string requires a string of length 9 not {text.Length}");
            string rText = text[1..3];
            string gText = text[3..5];
            string bText = text[5..7];
            string aText = text[7..9];
            int r = int.Parse(rText, System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(gText, System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(bText, System.Globalization.NumberStyles.HexNumber);
            int a = int.Parse(aText, System.Globalization.NumberStyles.HexNumber);
            return new Color(r, g, b, a);
        }
    }
}