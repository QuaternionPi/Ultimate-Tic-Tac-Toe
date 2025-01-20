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
        float alpha = float.NegativeInfinity;
        float beta = float.PositiveInfinity;
        int depth = moves.Count() < 7 ? 6 : moves.Count() < 30 ? 5 : 4;

        var evaluatedMoves =
            from move in moves.AsParallel()
            let placedBoard = (LargeGrid<Grid<Tile>, Tile>)board.Place(
                        player,
                        move.Item1,
                        move.Item2)
            let score = Minimax(
                    placedBoard,
                    depth,
                    -beta,
                    -alpha,
                    opponent,
                    player)
            orderby score descending
            select (move, score);

        var bestMove = evaluatedMoves.First().move;
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
        float minEvaluation = float.PositiveInfinity;
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