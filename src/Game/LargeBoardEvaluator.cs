namespace UltimateTicTacToe.Game;

public class LargeBoardEvaluator
{
    private BoardEvaluator Centre { get; }
    private BoardEvaluator Edge { get; }
    private BoardEvaluator Corner { get; }
    private float Win { get; }
    public LargeBoardEvaluator(BoardEvaluator centre, BoardEvaluator edge, BoardEvaluator corner, float win)
    {
        Centre = centre;
        Edge = edge;
        Corner = corner;
        Win = win;
    }
    public float Evaluate<TBoard, TCell>(ILargeBoard<TBoard, TCell> largeBoard, Player player, Player opponent) where TBoard : IBoard<TCell> where TCell : ICell
    {
        // Centre
        float evaluation = Centre.Evaluate(largeBoard.Grids[4], player, opponent);

        // Edges
        evaluation += Edge.Evaluate(largeBoard.Grids[0], player, opponent);
        evaluation += Edge.Evaluate(largeBoard.Grids[2], player, opponent);
        evaluation += Edge.Evaluate(largeBoard.Grids[6], player, opponent);
        evaluation += Edge.Evaluate(largeBoard.Grids[8], player, opponent);

        // Corners
        evaluation += Corner.Evaluate(largeBoard.Grids[0], player, opponent);
        evaluation += Corner.Evaluate(largeBoard.Grids[2], player, opponent);
        evaluation += Corner.Evaluate(largeBoard.Grids[6], player, opponent);
        evaluation += Corner.Evaluate(largeBoard.Grids[8], player, opponent);

        // Win
        evaluation += Award(Win, largeBoard.Player, player, opponent);
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