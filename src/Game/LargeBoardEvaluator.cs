using UltimateTicTacToe.Genetics;

namespace UltimateTicTacToe.Game;

public class LargeBoardEvaluator
{
    [Gene]
    public BoardEvaluator Centre { get; set; }
    [Gene]
    public BoardEvaluator Edge { get; set; }
    [Gene]
    public BoardEvaluator Corner { get; set; }
    [Gene]
    public double Win { get; set; }
    public LargeBoardEvaluator()
    {
        Win = 0;
        Centre = new();
        Edge = new();
        Corner = new();
    }
    public LargeBoardEvaluator(BoardEvaluator centre, BoardEvaluator edge, BoardEvaluator corner, double win)
    {
        Centre = centre;
        Edge = edge;
        Corner = corner;
        Win = win;
    }
    public double Evaluate<TBoard, TCell>(ILargeBoard<TBoard, TCell> largeBoard, Player.Token player, Player.Token opponent)
    where TBoard : IBoard<TBoard, TCell>
    where TCell : ICell<TCell>
    {
        // Centre
        double evaluation = Centre.Evaluate(largeBoard.Grids[4], player, opponent);

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
        evaluation += Award(Win, largeBoard.Winner, player, opponent);
        return evaluation;

    }
    protected static double Award<T>(double amount, T? compare, T positive, T negative) where T : class
    {
        if (positive.Equals(compare))
            return amount;
        if (negative.Equals(compare))
            return -amount;
        return 0;
    }
}