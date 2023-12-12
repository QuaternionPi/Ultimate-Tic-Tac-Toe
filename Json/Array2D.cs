using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe
{
    namespace Json
    {
        public class Array2DConverter : JsonConverter<object>
        {
            public override void Write(Utf8JsonWriter writer, object values, JsonSerializerOptions options)
            {
                Type type = values.GetType();


                writer.WriteStartArray();/*
                for (int i = 0; i < values.GetLength(0); i++)
                {
                    writer.WriteStartArray();
                    for (int j = 0; j < values.GetLength(1); j++)
                    {
                        object value = values[i, j];
                        JsonSerializer.Serialize<object>(writer, value, options);
                    }
                    writer.WriteEndArray();
                }*/
                writer.WriteEndArray();
            }
            public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
            public override bool CanConvert(Type typeToConvert)
            {
                return true;//base.CanConvert(typeToConvert);
            }
        }
    }
}