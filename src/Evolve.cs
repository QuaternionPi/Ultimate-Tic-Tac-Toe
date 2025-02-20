using Raylib_cs;
using UltimateTicTacToe.Game;
using UltimateTicTacToe.Genetics;

namespace UltimateTicTacToe;
public static class Evolve
{
    private static readonly Random Random = new(1);
    static void Main(string[] args)
    {
        var genomes = RandomLargeBoardEvaluators(9);

        var largeGrid = EmptyLargeGrid();

        var pool = new Pool<LargeBoardEvaluator>(genomes, (eval1, eval2) => Score(largeGrid, eval1, eval2), Random);

        for (int i = 0; i < 20; i++)
        {
            Console.WriteLine($"Generation number: {i}");
            pool.RunGeneration(1);
        }
    }
    static List<LargeBoardEvaluator> RandomLargeBoardEvaluators(int count)
    {
        var largeEvaluators = new List<LargeBoardEvaluator>();
        var evaluators = RandomBoardEvaluators(count * 3).GetEnumerator();
        evaluators.MoveNext();
        for (int i = 0; i < count; i++)
        {
            var centre = evaluators.Current;
            evaluators.MoveNext();
            var edge = evaluators.Current;
            evaluators.MoveNext();
            var corner = evaluators.Current;
            evaluators.MoveNext();
            var win = Random.NextDouble() * 1000;

            var evaluator = new LargeBoardEvaluator(centre, edge, corner, win);
            largeEvaluators.Add(evaluator);
        }
        return largeEvaluators;
    }
    static List<BoardEvaluator> RandomBoardEvaluators(int count)
    {
        var evaluators = new List<BoardEvaluator>();
        for (int i = 0; i < count; i++)
        {
            var centre = Random.NextDouble() * 1000;
            var edge = Random.NextDouble() * 1000;
            var corner = Random.NextDouble() * 1000;
            var win = Random.NextDouble() * 1000;

            var evaluator = new BoardEvaluator(centre, edge, corner, win);
            evaluators.Add(evaluator);
        }
        return evaluators;
    }
    static int Score(LargeGrid<Grid<Tile>, Tile> largeGrid, LargeBoardEvaluator evaluator1, LargeBoardEvaluator evaluator2)
    {
        var player1 = new Bot(evaluator1, Random, Player.Symbol.X, Color.RED, 0);
        var player2 = new Bot(evaluator2, Random, Player.Symbol.O, Color.BLUE, 0);
        var game = new Game.Game(0, player1, player2, largeGrid, largeGrid, new TimeSpan(0), new TimeSpan(0));
        bool foundWinner = false;
        Player? winner = null;
        game.GameOver += (sender, player) => { winner = player; foundWinner = true; };
        game.Start();

        TimeSpan sleepTime = new(0, 0, 0, 0, 10);
        while (!foundWinner)
        {
            Thread.Sleep(sleepTime);
            game.Update();
        }
        return winner == null ? 0 : winner == player1 ? 1 : -1;
    }
    private static LargeGrid<Grid<Tile>, Tile> EmptyLargeGrid()
    {
        var cells = new Grid<Tile>[9];
        for (int i = 0; i < 9; i++)
        {
            cells[i] = EmptyGrid();
        }
        var victoryTile = new Tile(null);
        return new LargeGrid<Grid<Tile>, Tile>(cells, victoryTile);
    }
    private static Grid<Tile> EmptyGrid()
    {
        var cells = new Tile[9];
        for (int i = 0; i < 9; i++)
        {
            cells[i] = new Tile(null);
        }
        var victoryTile = new Tile(null);
        return new Grid<Tile>(cells, victoryTile);
    }
}