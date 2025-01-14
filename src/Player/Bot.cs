using System.Diagnostics;
using UltimateTicTacToe.Game;
using Raylib_cs;

namespace UltimateTicTacToe.Player;
public partial class Bot : Player
{
    public Bot(Symbol symbol, Color color, int score) : base(symbol, color, score)
    {
    }
    public override void BeginTurn(LargeGrid<Grid<Tile>, Tile> board, UI.LargeBoard<Grid<Tile>, Tile> largeBoard, Player opponent)
    {
        IEnumerable<(int, int)> possibleMoves = PossibleMoves(board);
        var move = BestMove(board, possibleMoves, this, opponent);
        MakeMove(board, move.Item1, move.Item2);
    }
    public override void EndTurn()
    {

    }
    public override void Update()
    {

    }
    protected static (int, int) BestMove(
        LargeGrid<Grid<Tile>, Tile> board,
        IEnumerable<(int, int)> moves,
        Player player,
        Player opponent
        )
    {
        Debug.Assert(moves.Any() != false, "Cannot choose best move from no moves");
        int alpha = -100000;
        int beta = 100000;
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
            from move in moves//.AsParallel()
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

        int minEvaluation = 10000;
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
    protected static int Minimax(
        LargeGrid<Grid<Tile>, Tile> board,
        int depth,
        int alpha,
        int beta,
        Player player,
        Player opponent)
    {
        // The base case of the recursion or board is winning position
        if (depth == 0 || board.Player != null)
        {
            return Evaluate(board, opponent, player);
        }

        var possibleMoves = PossibleMoves(board);
        int minEvaluation = 10000;
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
    protected static int Award<T>(int amount, T? compare, T positive, T negative) where T : class
    {
        if (positive.Equals(compare))
            return amount;
        if (negative.Equals(compare))
            return -amount;
        return 0;
    }
    protected static int Evaluate(LargeGrid<Grid<Tile>, Tile> board, Player player, Player opponent)
    {
        if (board.Player == player)
        {
            return 1000;
        }
        else if (board.Player == opponent)
        {
            return -1000;
        }
        int evaluation = 0;
        evaluation -= NumPossibleMoves(board);
        for (int i = 0; i < 9; i++)
        {
            var grid = board.Cells[i];
            evaluation += Award(100, grid.Player, player, opponent);
            evaluation += Award(10, grid.Cells[4].Player, player, opponent);
            evaluation += Award(5, grid.Cells[0].Player, player, opponent);
            evaluation += Award(5, grid.Cells[2].Player, player, opponent);
            evaluation += Award(5, grid.Cells[6].Player, player, opponent);
            evaluation += Award(5, grid.Cells[8].Player, player, opponent);
        }
        evaluation += Award(50, board.Cells[4].Player, player, opponent);
        evaluation += Award(25, board.Cells[0].Player, player, opponent);
        evaluation += Award(25, board.Cells[2].Player, player, opponent);
        evaluation += Award(25, board.Cells[6].Player, player, opponent);
        evaluation += Award(25, board.Cells[8].Player, player, opponent);
        return evaluation;
    }
    protected static int NumPossibleMoves(LargeGrid<Grid<Tile>, Tile> board)
    {
        int count = 0;
        for (int i = 0; i < 9; i++)
            if (board.Placeable[i] && board.Cells[i].Player == null)
                for (int k = 0; k < 9; k++)
                    if (board.Cells[i].Cells[k].Player == null)
                        count++;

        return count;
    }
    protected static IEnumerable<(int, int)> PossibleMoves(LargeGrid<Grid<Tile>, Tile> board)
    {
        List<(int, int)> results = [];
        for (int i = 0; i < 9; i++)
            if (board.Placeable[i])
                for (int k = 0; k < 9; k++)
                    if (board.Cells[i].Cells[k].Player == null)
                        results.Add((i, k));
        return results;
    }
    protected void MakeMove(LargeGrid<Grid<Tile>, Tile> board, int index, int innerIndex)
    {
        InvokePlayTurn(this, board, index, innerIndex);
    }
}