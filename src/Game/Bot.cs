using System.Diagnostics;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe.Game;
public class Bot : Player
{
    [JsonIgnore]
    private Func<LargeGrid<Grid<Tile>, Tile>, Token, Token, double> Evaluate => Evaluator.Evaluate;
    [JsonInclude]
    private LargeBoardEvaluator Evaluator { get; }
    [JsonInclude]
    private Random Random { get; }
    [JsonConstructor]
    public Bot
    (
        LargeBoardEvaluator evaluator,
        Random random,
        Symbol shape,
        Color color,
        int score
    ) : base(shape, color, score)
    {
        Evaluator = evaluator;
        Random = random;
    }
    public override void BeginTurn
    (
        LargeGrid<Grid<Tile>, Tile> board,
        UI.LargeBoard<Grid<Tile>, Tile> largeBoard,
        Player opponent
    )
    {
        var possibleMoves = board.Moves;
        var move = BestMove(board, possibleMoves, GetToken(), opponent.GetToken());
        MakeMove(board, move);
    }
    public override void EndTurn() { }
    public override void Update() { }
    protected (int, int) BestMove
    (
        LargeGrid<Grid<Tile>, Tile> board,
        IEnumerable<(int, int)> moves,
        Token player,
        Token opponent
    )
    {
        Debug.Assert(moves.Any(), "Cannot choose best move from no moves");
        var alpha = double.NegativeInfinity;
        var beta = double.PositiveInfinity;
        int depth = 4;//moves.Count() < 7 ? 6 : moves.Count() < 30 ? 5 : 4;

        var bestMove = moves.First();
        var minScore = double.PositiveInfinity;
        var randomOrderMoves = from move in moves orderby Random.Next() descending select move;
        foreach (var move in randomOrderMoves)
        {
            var placedBoard = board.Place(player, move);
            var score = -Minimax(placedBoard, depth - 1, -beta, -alpha, opponent, player);
            beta = Math.Min(beta, score);
            if (score < minScore)
            {
                minScore = score;
                bestMove = move;
            }
        }
        return bestMove;
    }
    protected double Minimax
    (
        LargeGrid<Grid<Tile>, Tile> board,
        double depth,
        double alpha,
        double beta,
        Token player,
        Token opponent
    )
    {
        // The base case of the recursion or board is winning position
        if (depth == 0 || !board.AnyPlaceable)
        {
            return Evaluate(board, opponent, player);
        }

        var moves = board.Moves;
        double minScore = double.PositiveInfinity;
        foreach (var move in moves)
        {
            var placedBoard = board.Place(player, move);
            var score = -Minimax(placedBoard, depth - 1, -beta, -alpha, opponent, player);
            minScore = Math.Min(minScore, score);
            beta = Math.Min(beta, score);
            if (beta <= alpha)
                break;
        }
        return minScore;
    }
    protected void MakeMove(LargeGrid<Grid<Tile>, Tile> board, (int, int) move)
    {
        InvokePlayTurn(this, board, move);
    }
}