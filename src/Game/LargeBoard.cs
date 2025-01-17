namespace UltimateTicTacToe.Game;
/*
The 3 by 3 objects that Ultimate Tic Tac Toe is played on
*/
public interface ILargeBoard<TGrid, TCell> where TCell : ICell where TGrid : IBoard<TCell>
{
    public TGrid[] Grids { get; }
    public TCell WinningPlayerCell { get; }
    public Player? Player { get; }
    public bool AnyPlaceable { get; }
    public IEnumerable<(int, int)> PlayableIndices { get; }
    public ILargeBoard<TGrid, TCell> Place(Player player, int index, int innerIndex);
}
public static class LargeBoardExtensions
{
    public static bool HasWinner<TGrid, TCell>(this ILargeBoard<TGrid, TCell> board) where TCell : ICell where TGrid : IBoard<TCell>
    {
        bool hasWinner = false;

        Player?[] cellWinners = new Player?[9];
        for (int i = 0; i < 9; i++)
        {
            cellWinners[i] = board.Grids[i]?.Player;
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

        // Diagonals
        hasWinner |= trueCenter != null
            && topLeft == trueCenter
            && trueCenter == bottomRight;
        hasWinner |= trueCenter != null
            && topRight == trueCenter
            && trueCenter == bottomLeft;

        // Column 0
        hasWinner |= topLeft != null
            && topLeft == topCenter
            && topCenter == topRight;
        // Column 1
        hasWinner |= leftCenter != null
            && leftCenter == trueCenter
            && trueCenter == rightCenter;
        // Column 2
        hasWinner |= bottomLeft != null
            && bottomLeft == bottomCenter
            && bottomCenter == bottomRight;

        // Row 0
        hasWinner |= topLeft != null
            && topLeft == leftCenter
            && leftCenter == bottomLeft;
        // Row 1
        hasWinner |= topCenter != null
            && topCenter == trueCenter
            && trueCenter == bottomCenter;
        // Row 2
        hasWinner |= topRight != null
            && topRight == rightCenter
            && rightCenter == bottomRight;
        return hasWinner;
    }
    public static Player? Winner<TGrid, TCell>(this ILargeBoard<TGrid, TCell> board) where TCell : ICell where TGrid : IBoard<TCell>
    {
        if (board.HasWinner() == false)
        {
            return null;
        }

        Player?[] cellWinners = new Player?[9];
        for (int i = 0; i < 9; i++)
        {
            cellWinners[i] = board.Grids[i]?.Player;
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
            bool winnerFound = false;
            // Column 1
            winnerFound |= leftCenter == trueCenter && trueCenter == rightCenter;
            // Row 1
            winnerFound |= topCenter == trueCenter && trueCenter == bottomCenter;

            // Diagonals
            winnerFound |= topLeft == trueCenter && trueCenter == bottomRight;
            winnerFound |= topRight == trueCenter && trueCenter == bottomLeft;

            if (winnerFound)
                return trueCenter;
        }
        if (topLeft != null)
        {
            bool winnerFound = false;
            // Column 0
            winnerFound |= topLeft == topCenter && topCenter == topRight;

            // Row 0
            winnerFound |= topLeft == leftCenter && leftCenter == bottomLeft;
            if (winnerFound)
                return topLeft;
        }
        if (bottomRight != null)
        {
            // Column 2
            bool winnerFound = false;
            winnerFound |= bottomLeft == bottomCenter && bottomCenter == bottomRight;
            // Row 2
            winnerFound |= topRight == rightCenter && rightCenter == bottomRight;
            if (winnerFound)
                return bottomRight;
        }
        return null;
    }
}