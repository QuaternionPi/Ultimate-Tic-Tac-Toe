using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    namespace Game
    {
        public interface IBoard<TCell> : ICell where TCell : ICell, new()
        {
            public TCell[,] Cells { get; }
        }
        public static class BoardExtensions
        {
            public static List<Address> PathToCell<TCell>(this IBoard<TCell> board, ICell cell) where TCell : ICell, new()
            {
                if (board.Contains(cell) == false)
                {
                    throw new Exception($"Cell: {cell} is not contained. There is no path to it");
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board.Cells[i, j].Contains(cell))
                        {
                            Address address = new(i, j);
                            return board.Cells[i, j].PathTo(cell).Prepend(address).ToList();
                        }
                    }
                }
                throw new Exception($"Cell: {cell} was not found");
            }
            public static bool ContainsCell<TCell>(this IBoard<TCell> board, ICell cell) where TCell : ICell, new()
            {
                bool contains = false;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        contains |= board.Cells[i, j].Equals(cell);
                        contains |= board.Cells[i, j].Contains(cell);
                    }
                }
                return contains;
            }
            public static bool HasWinner<TCell>(this IBoard<TCell> board) where TCell : ICell, new()
            {
                bool hasWinner = false;
                Player[,] cellWinners =
                    from cell in board.Cells
                    select cell.Player;

                Player topLeft = cellWinners[0, 0];
                Player topCenter = cellWinners[0, 1];
                Player topRight = cellWinners[0, 2];

                Player leftCenter = cellWinners[1, 0];
                Player trueCenter = cellWinners[1, 1];
                Player rightCenter = cellWinners[1, 2];

                Player bottomLeft = cellWinners[2, 0];
                Player bottomCenter = cellWinners[2, 1];
                Player bottomRight = cellWinners[2, 2];

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
            public static Player? Winner<TCell>(this IBoard<TCell> board) where TCell : ICell, new()
            {
                if (board.HasWinner() == false)
                {
                    return null;
                }

                Player[,] cellWinners =
                    from cell in board.Cells
                    select cell.Player;

                Player topLeft = cellWinners[0, 0];
                Player topCenter = cellWinners[0, 1];
                Player topRight = cellWinners[0, 2];

                Player leftCenter = cellWinners[1, 0];
                Player trueCenter = cellWinners[1, 1];
                Player rightCenter = cellWinners[1, 2];

                Player bottomLeft = cellWinners[2, 0];
                Player bottomCenter = cellWinners[2, 1];
                Player bottomRight = cellWinners[2, 2];

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
    }
}