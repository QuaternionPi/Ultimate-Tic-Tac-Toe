using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToe.Serialization.Json;
public class LargeGridOfTConverter : JsonConverterFactory
{
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type gridType = typeToConvert.GetGenericArguments()[0];
        Type cellType = typeToConvert.GetGenericArguments()[1];
        var converter = (JsonConverter)Activator.CreateInstance(
            typeof(LargeGridOfTConverterInner<,>).MakeGenericType(
                [gridType, cellType]),
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

        return typeToConvert.GetGenericTypeDefinition() == typeof(LargeGrid<,>);
    }
    private class LargeGridOfTConverterInner<TGrid, TCell> : JsonConverter<LargeGrid<TGrid, TCell>>
    where TGrid : class, IBoard<TGrid, TCell>
    where TCell : class, ICell<TCell>
    {
        private JsonConverter<TCell> CellConverter { get; }
        private JsonConverter<TGrid[]> GridArrayConverter { get; }
        public LargeGridOfTConverterInner(JsonSerializerOptions options)
        {
            CellConverter = (JsonConverter<TCell>)options.GetConverter(typeof(TCell));
            GridArrayConverter = (JsonConverter<TGrid[]>)options.GetConverter(typeof(TGrid[]));
        }
        public override LargeGrid<TGrid, TCell>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"Expected a Start Object not {reader.TokenType}");
            TGrid[]? grids = null;
            TCell? winningPlayerCell = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    if (grids is null || winningPlayerCell is null)
                        throw new JsonException($"Requires properties 'Cells' and 'WinningPlayerCell'");
                    return new LargeGrid<TGrid, TCell>(grids, winningPlayerCell);
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
                    case "Grids":
                        grids = GridArrayConverter.Read(ref reader, typeof(TGrid[]), options);
                        break;
                    case "WinningPlayerCell":
                        winningPlayerCell = CellConverter.Read(ref reader, typeof(TCell), options);
                        break;
                    default:
                        throw new JsonException($"Expects properties 'Cells' or 'WinningPlayerCell' not {propertyName}");
                }
            }
            throw new JsonException("Expected an End of Object");
        }

        public override void Write(Utf8JsonWriter writer, LargeGrid<TGrid, TCell> largeGrid, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Grids");
            GridArrayConverter.Write(writer, largeGrid.Grids, options);

            writer.WritePropertyName("WinningPlayerCell");
            CellConverter.Write(writer, largeGrid.WinningPlayerCell, options);

            writer.WriteEndObject();
        }
    }
}