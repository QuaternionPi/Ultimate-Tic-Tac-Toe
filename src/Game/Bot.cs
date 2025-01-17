using System.Diagnostics;
using Raylib_cs;

namespace UltimateTicTacToe.Game;
public class Bot : Player
{
    private Func<LargeGrid<Grid<Tile>, Tile>, Player, Player, float> Evaluate { get; }
    public Bot(Func<LargeGrid<Grid<Tile>, Tile>, Player, Player, float> evaluate, Symbol symbol, Color color, int score) : base(symbol, color, score)
    {
        Evaluate = evaluate;
    }
    public override void BeginTurn(LargeGrid<Grid<Tile>, Tile> board, UI.LargeBoard<Grid<Tile>, Tile> largeBoard, Player opponent)
    {
        var possibleMoves = board.PlayableIndices;
        var move = BestMove(board, possibleMoves, this, opponent);
        MakeMove(board, move.Item1, move.Item2);
    }
    public override void EndTurn()
    {

    }
    public override void Update()
    {

    }
    protected (int, int) BestMove(
        LargeGrid<Grid<Tile>, Tile> board,
        IEnumerable<(int, int)> moves,
        Player player,
        Player opponent
        )
    {
        Debug.Assert(moves.Any() != false, "Cannot choose best move from no moves");
        float alpha = -100000;
        float beta = 100000;
        int depth;
        if (moves.Count() < 7)
        {
            depth = 6;
        }
        else if (moves.Count() < 30)
        {
            depth = 5;
        }
        else
        {
            depth = 4;
        }

        var evaluatedMoves =
            from move in moves.AsParallel()
            select (move,
                -Minimax(
                    (LargeGrid<Grid<Tile>, Tile>)board.Place(
                        player,
                        move.Item1,
                        move.Item2),
                    depth,
                    -beta,
                    -alpha,
                    opponent,
                    player));

        float minEvaluation = 10000;
        (int, int) bestMove = moves.First();

        foreach (var (move, evaluation) in evaluatedMoves)
        {
            if (minEvaluation > evaluation)
            {
                minEvaluation = evaluation;
                bestMove = move;
            }
        }
        return bestMove;
    }
    protected float Minimax(
        LargeGrid<Grid<Tile>, Tile> board,
        float depth,
        float alpha,
        float beta,
        Player player,
        Player opponent)
    {
        // The base case of the recursion or board is winning position
        if (depth == 0 || !board.AnyPlaceable)
        {
            return Evaluate(board, opponent, player);
        }

        var possibleMoves = board.PlayableIndices;
        float minEvaluation = 10000;
        foreach (var move in possibleMoves)
        {
            var placedBoard = (LargeGrid<Grid<Tile>, Tile>)board.Place(player, move.Item1, move.Item2);
            var evaluation = -Minimax(placedBoard, depth - 1, -beta, -alpha, opponent, player);
            minEvaluation = Math.Min(minEvaluation, evaluation);
            beta = Math.Min(beta, evaluation);
            if (beta <= alpha)
                break;
        }
        return minEvaluation;
    }
    protected void MakeMove(LargeGrid<Grid<Tile>, Tile> board, int index, int innerIndex)
    {
        InvokePlayTurn(this, board, index, innerIndex);
    }
}