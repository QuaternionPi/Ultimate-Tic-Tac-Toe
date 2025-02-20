using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using UltimateTicTacToe.Genetics;

namespace UltimateTicTacToe.Serialization.Json;
public class PoolOfTConverter : JsonConverterFactory
{
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type genomeType = typeToConvert.GetGenericArguments()[0];
        var converter = (JsonConverter)Activator.CreateInstance(
            typeof(PoolOfTConverterInner<>).MakeGenericType([genomeType]),
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

        return typeToConvert.GetGenericTypeDefinition() == typeof(Pool<>);
    }
    private class PoolOfTConverterInner<TGenome> : JsonConverter<Pool<TGenome>>
    where TGenome : class, new()
    {
        private JsonConverter<TGenome[]> GenomeArrayConverter { get; }
        private JsonConverter<Random> RandomConverter { get; }
        public PoolOfTConverterInner(JsonSerializerOptions options)
        {
            GenomeArrayConverter = (JsonConverter<TGenome[]>)options.GetConverter(typeof(TGenome[]));
            RandomConverter = (JsonConverter<Random>)options.GetConverter(typeof(Random));
        }
        public override Pool<TGenome>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException($"Expected a Start Object not {reader.TokenType}");
            TGenome[]? genomes = null;
            Random? random = null;
            int generation = 0;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    if (genomes is null)
                        throw new JsonException($"Requires property 'Genomes'");
                    return new Pool<TGenome>(generation, genomes, random);
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
                    case "Generation":
                        generation = reader.GetInt32();
                        break;
                    case "Random":
                        random = RandomConverter.Read(ref reader, typeof(Random), options);
                        break;
                    case "Genomes":
                        genomes = GenomeArrayConverter.Read(ref reader, typeof(TGenome[]), options);
                        break;
                    default:
                        throw new JsonException($"Expects properties 'Cells' or 'WinningPlayerCell' not {propertyName}");
                }
            }
            throw new JsonException("Expected an End of Object");
        }

        public override void Write(Utf8JsonWriter writer, Pool<TGenome> pool, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Generation");
            writer.WriteNumberValue(pool.Generation);

            writer.WritePropertyName("Random");
            RandomConverter.Write(writer, pool.Random, options);

            writer.WritePropertyName("Genomes");
            GenomeArrayConverter.Write(writer, pool.Genomes, options);


            writer.WriteEndObject();
        }
    }
}