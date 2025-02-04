using UltimateTicTacToe.Genetics;

namespace UltimateTicTacToe.Game;

public class BoardEvaluator
{
    [Gene]
    public double Centre { get; set; }
    [Gene]
    public double Edge { get; set; }
    [Gene]
    public double Corner { get; set; }
    [Gene]
    public double Win { get; set; }
    public BoardEvaluator() { }
    public BoardEvaluator(double centre, double edge, double corner, double win)
    {
        Centre = centre;
        Edge = edge;
        Corner = corner;
        Win = win;
    }
    public double Evaluate<TCell>(IBoard<TCell> board, Player.Token player, Player.Token opponent) where TCell : ICell
    {
        // Centre
        double evaluation = Award(Centre, board.Cells[4].Owner, player, opponent);

        // Edges
        evaluation += Award(Edge, board.Cells[0].Owner, player, opponent);
        evaluation += Award(Edge, board.Cells[2].Owner, player, opponent);
        evaluation += Award(Edge, board.Cells[6].Owner, player, opponent);
        evaluation += Award(Edge, board.Cells[8].Owner, player, opponent);

        // Corners
        evaluation += Award(Corner, board.Cells[0].Owner, player, opponent);
        evaluation += Award(Corner, board.Cells[2].Owner, player, opponent);
        evaluation += Award(Corner, board.Cells[6].Owner, player, opponent);
        evaluation += Award(Corner, board.Cells[8].Owner, player, opponent);

        // Win
        evaluation += Award(Win, board.Winner, player, opponent);
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