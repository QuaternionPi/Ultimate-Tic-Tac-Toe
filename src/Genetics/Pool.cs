using System.Diagnostics;

namespace UltimateTicTacToe.Genetics;

public class Pool<TGenome>
where TGenome : class, new()
{
    private static int TournamentSize { get; } = 3;
    private TGenome[] Genomes { get; set; }
    private Crossover Crossover { get; set; }
    private Random Random { get; }
    public Pool(IEnumerable<TGenome> genomes, Random? random = null)
    {
        Debug.Assert(genomes.Count() > 2);
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
    public IEnumerable<(TGenome, TGenome)> Tournament(Func<TGenome, TGenome, int> rank, int replications)
    {
        List<(TGenome, TGenome)> genomes = [];
        foreach (var batch in Genomes.OrderBy(order => Random.Next()).Batch(TournamentSize))
        {
            if (batch.Count() != TournamentSize)
            {
                break;
            }
            Dictionary<TGenome, int> scores = [];
            foreach (var genome in batch)
            {
                scores[genome] = 0;
            }
            foreach (var pair in batch.Pairs())
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
            genomes.Add((bestGenome, secondBestGenome));
        }
        return genomes;
    }
    public void RunGeneration(Func<TGenome, TGenome, int> rank, int replications)
    {
        var pairs = Enumerable
            .Range(0, TournamentSize)
            .SelectMany((x) => Tournament(rank, replications))
            .ToArray();
        var newGenomes =
            from pair in pairs
            let genome1 = pair.Item1
            let genome2 = pair.Item2
            select Crossover.Combine(genome1, genome2);
        Genomes = [.. newGenomes];
    }
}