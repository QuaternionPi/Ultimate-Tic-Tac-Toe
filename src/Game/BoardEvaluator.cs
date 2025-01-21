namespace UltimateTicTacToe.Game;

public class BoardEvaluator
{
    private float Centre { get; }
    private float Edge { get; }
    private float Corner { get; }
    private float Win { get; }
    public BoardEvaluator(float centre, float edge, float corner, float win)
    {
        Centre = centre;
        Edge = edge;
        Corner = corner;
        Win = win;
    }
    public float Evaluate<TCell>(IBoard<TCell> board, Player player, Player opponent) where TCell : ICell<TCell>
    {
        // Centre
        float evaluation = Award(Centre, board.Cells[4].Player, player, opponent);

        // Edges
        evaluation += Award(Edge, board.Cells[0].Player, player, opponent);
        evaluation += Award(Edge, board.Cells[2].Player, player, opponent);
        evaluation += Award(Edge, board.Cells[6].Player, player, opponent);
        evaluation += Award(Edge, board.Cells[8].Player, player, opponent);

        // Corners
        evaluation += Award(Corner, board.Cells[0].Player, player, opponent);
        evaluation += Award(Corner, board.Cells[2].Player, player, opponent);
        evaluation += Award(Corner, board.Cells[6].Player, player, opponent);
        evaluation += Award(Corner, board.Cells[8].Player, player, opponent);

        // Win
        evaluation += Award(Win, board.Player, player, opponent);
        return evaluation;
    }
    protected static float Award<T>(float amount, T? compare, T positive, T negative) where T : class
    {
        if (positive.Equals(compare))
            return amount;
        if (negative.Equals(compare))
            return -amount;
        return 0;
    }
}