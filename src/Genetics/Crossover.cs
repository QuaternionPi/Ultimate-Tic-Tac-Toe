using System.Reflection;
using System.Diagnostics;

namespace UltimateTicTacToe.Genetics;

public class Crossover
{
    private readonly Type _genomeType;
    private readonly Random _random;
    private readonly Dictionary<Type, Crossover> _cachedCrossover;
    private readonly PropertyInfo[] _doubleProperties;
    private readonly PropertyInfo[] _objectProperties;
    public Crossover(Type genomeType, Random? random = null)
    {
        _genomeType = genomeType;
        var genes = genomeType
            .GetProperties()
            .Where(property => property.IsDefined(typeof(Gene), false));
        _doubleProperties = [.. genes.Where(property => property.PropertyType == typeof(double))];
        _objectProperties = [.. genes.Where(property => property.PropertyType != typeof(double))];

        _random = random ?? new Random(0);

        _cachedCrossover = [];
        foreach (var objectProperty in _objectProperties)
        {
            Type geneType = objectProperty.PropertyType;
            var crossover = new Crossover(geneType, _random);
            _cachedCrossover[geneType] = crossover;
        }
    }
    public TGenome Combine<TGenome>(TGenome genome1, TGenome genome2) where TGenome : new()
    {
        Debug.Assert(typeof(TGenome) == _genomeType, $"TGenome must be the same type as _genomeType. {typeof(TGenome)} != {_genomeType}");
        TGenome result = new();
        foreach (var property in _doubleProperties)
        {
            var gene1Value = (double)(property.GetValue(genome1) ?? 0);
            var gene2Value = (double)(property.GetValue(genome2) ?? 0);
            Gene gene = property.GetCustomAttribute<Gene>()!;
            var resultGene = Combine(gene1Value, gene2Value, gene);
            property.SetValue(result, resultGene);
        }
        foreach (var property in _objectProperties)
        {
            dynamic? gene1Value = property.GetValue(genome1);
            dynamic? gene2Value = property.GetValue(genome2);

            dynamic? resultGene;
            if (gene1Value is not null && gene2Value is not null)
            {
                var geneType = property.PropertyType;
                var crossover = _cachedCrossover[geneType];
                resultGene = crossover.Combine(gene1Value, gene2Value);
            }
            else
            {
                resultGene = gene1Value ?? gene2Value;
            }

            property.SetValue(result, resultGene);
        }
        return result;
    }
    protected double Combine(double gene1Value, double gene2Value, Gene gene)
    {
        var minChance = 0.025;
        var maxChance = 0.025;
        var averageChance = 0.05;
        var copyGene1Chance = 0.45;
        var role = _random.NextDouble();
        if (role <= minChance && gene.RangeType is not null)
        {
            return (double)gene.Minimum!;
        }
        role -= minChance;
        if (role <= maxChance && gene.RangeType is not null)
        {
            return (double)gene.Minimum!;
        }
        role -= maxChance;
        if (role <= averageChance)
        {
            return (gene1Value + gene2Value) / 2;
        }
        role -= averageChance;
        if (role <= copyGene1Chance)
        {
            return gene1Value;
        }
        return gene2Value;
    }
}