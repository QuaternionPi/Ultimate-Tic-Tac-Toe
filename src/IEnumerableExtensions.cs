namespace UltimateTicTacToe;
public static class IEnumerableExtensions
{
    public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int batchSize)
    {
        return source
            .Select((value, i) => new { Value = value, Index = i })
            .GroupBy(value => value.Index / batchSize)
            .Select(value => value.Select(v => v.Value));
    }
    public static IEnumerable<(TSource, TSource)> Pairs<TSource>(this IEnumerable<TSource> source)
    {
        int i = 1;
        foreach (var item1 in source)
        {
            foreach (var item2 in source.Skip(i))
            {
                yield return (item1, item2);
            }
            i++;
        }
    }
    public static TSource RandomElement<TSource>(this IEnumerable<TSource> source, Random random)
    {
        int index = random.Next(source.Count());
        return source.ElementAt(index);
    }
    public static IEnumerable<TSource> RandomElements<TSource>(this IEnumerable<TSource> source, Random random, int limit)
    {
        for (int i = 0; i < limit; i++)
        {
            yield return source.RandomElement(random);
        }
    }
    public static TSource RandomElementWeighted<TSource>
    (
        this IEnumerable<TSource> source,
        IEnumerable<int> weights,
        Random random
    )
    {
        var randomIndex = random.NextDouble() * weights.Sum();
        double sum = 0;
        foreach ((TSource item, var weight) in Enumerable.Zip(source, weights))
        {
            sum += weight;
            if (sum > randomIndex)
            {
                return item;
            }
        }
        return source.Last();
    }
    public static TSource RandomElementWeighted<TSource>
    (
        this IEnumerable<TSource> source,
        IEnumerable<double> weights,
        Random random
    )
    {
        var randomIndex = random.NextDouble() * weights.Sum();
        double sum = 0;
        foreach ((TSource item, var weight) in Enumerable.Zip(source, weights))
        {
            sum += weight;
            if (sum > randomIndex)
            {
                return item;
            }
        }
        return source.Last();
    }
    public static IEnumerable<TSource> RandomElementsWeighted<TSource>
    (
        this IEnumerable<TSource> source,
        IEnumerable<int> weights,
        Random random,
        int limit
    )
    {
        for (int i = 0; i < limit; i++)
        {
            yield return source.RandomElementWeighted(weights, random);
        }
    }
    public static IEnumerable<TSource> RandomElementsWeighted<TSource>
    (
        this IEnumerable<TSource> source,
        IEnumerable<double> weights,
        Random random,
        int limit
    )
    {
        for (int i = 0; i < limit; i++)
        {
            yield return source.RandomElementWeighted(weights, random);
        }
    }
}