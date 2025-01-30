using System.Reflection;
using System.Diagnostics;

namespace UltimateTicTacToe.Genetics;

public class Crossover
{
    private readonly Type _genomeType;
    private readonly Random _random;
    private readonly Dictionary<Type, Crossover> _cachedCrossover;
    private readonly PropertyInfo[] _doubleGenes;
    private readonly PropertyInfo[] _objectGenes;
    public Crossover(Type genomeType, Random? random = null)
    {
        _genomeType = genomeType;
        var genes = genomeType
            .GetProperties()
            .Where(property => property.IsDefined(typeof(Gene), false));
        _doubleGenes = [.. genes.Where(property => property.PropertyType == typeof(double))];
        _objectGenes = [.. genes.Where(property => property.PropertyType != typeof(double))];

        _random = random ?? new Random(0);

        _cachedCrossover = [];
        foreach (var objectGene in _objectGenes)
        {
            Type geneType = objectGene.PropertyType;
            var crossover = new Crossover(geneType, _random);
            _cachedCrossover[geneType] = crossover;
        }
    }
    public TGenome Combine<TGenome>(TGenome genome1, TGenome genome2) where TGenome : new()
    {
        Debug.Assert(typeof(TGenome) == _genomeType, $"TGenome must be the same type as _genomeType. {typeof(TGenome)} != {_genomeType}");
        TGenome result = new();
        foreach (var gene in _doubleGenes)
        {
            var gene1 = (double)(gene.GetValue(genome1) ?? 0);
            var gene2 = (double)(gene.GetValue(genome2) ?? 0);
            var resultGene = Combine(gene1, gene2);
            gene.SetValue(result, resultGene);
        }
        foreach (var gene in _objectGenes)
        {
            dynamic? gene1 = gene.GetValue(genome1);
            dynamic? gene2 = gene.GetValue(genome2);

            dynamic? resultGene;
            if (gene1 is not null && gene2 is not null)
            {
                var geneType = gene.PropertyType;
                var crossover = _cachedCrossover[geneType];
                resultGene = crossover.Combine(gene1, gene2);
            }
            else
            {
                resultGene = gene1 ?? gene2;
            }

            gene.SetValue(result, resultGene);
        }
        return result;
    }
    protected double Combine(double genome1, double genome2)
    {
        var preserveChance = 0.9;
        var preserveRole = _random.NextDouble();
        if (preserveChance <= preserveRole)
        {
            var genome1Change = 0.5;
            var genomeRole = _random.NextDouble();
            return genome1Change <= genomeRole ? genome1 : genome2;
        }
        return (genome1 + genome2) / 2;
    }
}