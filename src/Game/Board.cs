namespace UltimateTicTacToe.Game;
/*
The 3 by 3 objects that Ultimate Tic Tac Toe is played on
*/
public interface IBoard<TCell>
where TCell : ICell
{
    public TCell[] Cells { get; }
    public TCell WinningPlayerCell { get; }
    public Player? Player { get; }
    public bool AnyPlaceable { get; }
    public IEnumerable<int> Moves { get; }
}

public interface IBoard<TSelf, TCell> : IBoard<TCell>
where TSelf : IBoard<TSelf, TCell>
where TCell : ICell<TCell>
{
    public TSelf Place(Player player, int index);
}
public static class BoardExtensions
{
    public static Player? Winner<TSelf, TCell>(this IBoard<TSelf, TCell> board)
    where TCell : ICell<TCell>
    where TSelf : IBoard<TSelf, TCell>
    {
        Player?[] cellWinners = new Player?[9];
        for (int i = 0; i < 9; i++)
        {
            cellWinners[i] = board.Cells[i]?.Player;
        }

        Player? topLeft = cellWinners[0];
        Player? topCenter = cellWinners[1];
        Player? topRight = cellWinners[2];

        Player? leftCenter = cellWinners[3];
        Player? trueCenter = cellWinners[4];
        Player? rightCenter = cellWinners[5];

        Player? bottomLeft = cellWinners[6];
        Player? bottomCenter = cellWinners[7];
        Player? bottomRight = cellWinners[8];

        if (trueCenter != null)
        {
            bool winnerFound =
            // Column 1
            (leftCenter == trueCenter && trueCenter == rightCenter)
            // Row 1
            || (topCenter == trueCenter && trueCenter == bottomCenter)
            // Diagonals
            || (topLeft == trueCenter && trueCenter == bottomRight)
            || (topRight == trueCenter && trueCenter == bottomLeft);
            if (winnerFound)
                return trueCenter;
        }
        if (topLeft != null)
        {
            bool winnerFound =
            // Column 0
            (topLeft == topCenter && topCenter == topRight)
            // Row 0
            || (topLeft == leftCenter && leftCenter == bottomLeft);
            if (winnerFound)
                return topLeft;
        }
        if (bottomRight != null)
        {
            bool winnerFound =
            // Column 2
             (bottomLeft == bottomCenter && bottomCenter == bottomRight)
            // Row 2
            || (topRight == rightCenter && rightCenter == bottomRight);
            if (winnerFound)
                return bottomRight;
        }
        return null;
    }
}