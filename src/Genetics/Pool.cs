using System.Diagnostics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Genetics;

public class Pool<TGenome>
where TGenome : class, new()
{
    private static int TournamentSize { get; } = 3;
    public TGenome[] Genomes { get; private set; }
    private Crossover Crossover { get; set; }
    public Random Random { get; }
    public int Generation { get; private set; }
    public Pool(int generation, IEnumerable<TGenome> genomes, Random? random = null)
    {
        Debug.Assert(genomes.Count() > 2);
        Generation = generation;
        Genomes = [.. genomes];
        Random = random ?? new Random(0);
        Crossover = new Crossover(typeof(TGenome));
    }
    public static IEnumerable<(TGenome, TGenome)> RouletteWheelSelection
    (
        IEnumerable<TGenome> genomes,
        IEnumerable<int> weights,
        Random random,
        int limit
    )
    {
        var randomGenomes = genomes.RandomElementsWeighted(weights, random, limit * 2).GetEnumerator();
        for (var i = 0; i < limit; i++)
        {
            var genome1 = randomGenomes.Current;
            if (!randomGenomes.MoveNext())
            {
                yield break;
            }

            var genome2 = randomGenomes.Current;
            yield return (genome1, genome2);
            if (!randomGenomes.MoveNext())
            {
                yield break;
            }
        }
    }
    public static IEnumerable<(TGenome, TGenome)> RouletteWheelSelection
    (
        Dictionary<TGenome, int> scores,
        Random random,
        int limit
    )
    {
        IEnumerable<TGenome> genomes = scores.Keys;
        IEnumerable<int> weights = scores.Values;
        return RouletteWheelSelection(genomes, weights, random, limit);
    }
    private static (TGenome, TGenome) BestRanked(Func<TGenome, TGenome, int> rank, IEnumerable<TGenome> genomes, int replications)
    {
        Dictionary<TGenome, int> scores = [];
        foreach (var genome in genomes)
        {
            scores[genome] = 0;
        }
        foreach (var pair in genomes.Pairs())
        {
            var genome1 = pair.Item1;
            var genome2 = pair.Item2;
            var score = Enumerable
                .Range(0, replications)
                .Select((x) => rank(genome1, genome2))
                .Sum();
            scores[genome1] += score;
            scores[genome2] -= score;
        }
        var orderedGenomes =
            from score in scores.AsEnumerable()
            orderby score.Value descending
            select score.Key;
        var bestGenome = orderedGenomes.First();
        var secondBestGenome = orderedGenomes.Skip(1).First();
        return (bestGenome, secondBestGenome);
    }
    private IEnumerable<(TGenome, TGenome)> Tournament(Func<TGenome, TGenome, int> rank, int replications)
    {
        List<(TGenome, TGenome)> genomes = [..
            Genomes.OrderBy(order => Random.Next())
            .Batch(TournamentSize)
            .Where(batch => batch.Count() == TournamentSize)
            .Select(batch => BestRanked(rank, batch, replications))
        ];
        return genomes;
    }
    public void RunGeneration(Func<TGenome, TGenome, int> rank, int replications)
    {
        var pairs = Tournament(rank, replications);

        var newGenomeBatches =
            from pair in pairs
            let genome1 = pair.Item1
            let genome2 = pair.Item2
            select Enumerable.Range(0, TournamentSize).Select(x => Crossover.Combine(genome1, genome2));

        var newGenomes = newGenomeBatches.SelectMany(batch => batch);
        Genomes = [.. newGenomes];
        Generation++;
    }
}