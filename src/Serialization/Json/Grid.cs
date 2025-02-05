using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe.Serialization.Json
{
    public class GridOfTConverter : JsonConverterFactory
    {
        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type cellType = typeToConvert.GetGenericArguments()[0];
            var converter = (JsonConverter)Activator.CreateInstance(
                typeof(GridOfTConverterInner<>).MakeGenericType(
                    [cellType]),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: [options],
                culture: null)!;
            return converter;
        }

        public override bool CanConvert(Type typeToConvert)
        {

            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            return typeToConvert.GetGenericTypeDefinition() == typeof(Grid<>);
        }
        private class GridOfTConverterInner<TCell> : JsonConverter<Grid<TCell>>
        where TCell : class, ICell<TCell>
        {
            private JsonConverter<TCell> CellConverter { get; }
            public GridOfTConverterInner(JsonSerializerOptions options)
            {
                CellConverter = (JsonConverter<TCell>)options.GetConverter(typeof(TCell));
            }
            public override Grid<TCell>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException($"Expected a Start Object not {reader.TokenType}");
                TCell[]? cells = null;
                TCell? winningPlayerCell = null;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        if (cells is null || winningPlayerCell is null)
                            throw new JsonException($"Requires properties 'Cells' and 'WinningPlayerCell'");
                        return new Grid<TCell>(cells, winningPlayerCell);
                    }

                    // Get the key.
                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        throw new JsonException();
                    }

                    string? propertyName = reader.GetString();
                    reader.Read();

                    switch (propertyName)
                    {
                        case "Cells":
                            cells = JsonSerializer.Deserialize<TCell[]>(ref reader, options);
                            break;
                        case "WinningPlayerCell":
                            winningPlayerCell = JsonSerializer.Deserialize<TCell>(ref reader, options);
                            break;
                        default:
                            throw new JsonException($"Expects properties 'Cells' or 'WinningPlayerCell' not {propertyName}");
                    }
                }
                throw new JsonException("Expected an End of Object");
            }

            public override void Write(Utf8JsonWriter writer, Grid<TCell> grid, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("Cells");
                writer.WriteStartArray();
                foreach (TCell cell in grid.Cells)
                {
                    JsonSerializer.Serialize(writer, cell, options);
                }
                writer.WriteEndArray();

                writer.WritePropertyName("WinningPlayerCell");
                JsonSerializer.Serialize(writer, grid.WinningPlayerCell, options);

                writer.WriteEndObject();
            }
        }
    }
}