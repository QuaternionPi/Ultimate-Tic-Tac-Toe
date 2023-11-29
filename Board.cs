using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public readonly struct Address
    {
        public Address(int x, int y)
        {
            if (x > 2 | x < 0 | y > 2 | y < 0)
            {
                throw new IndexOutOfRangeException("Position not in grid");
            }
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get; }
    }
    public interface IBoard<CellT> : IDrawable, IUpdateable, ICell where CellT : ICell, new()
    {
        public CellT[,] Cells { get; }
        public CellT? LastPlaced { get; }
    }
    public static class BoardExtensions
    {
        public static Vector2 PixelPosition<CellT>(this IBoard<CellT> board, Address address) where CellT : ICell, new()
        {
            int i = address.X;
            int j = address.Y;
            int x = (int)(board.Transform.Position.X + (i - 1) * 50 * board.Transform.Scale);
            int y = (int)(board.Transform.Position.Y + (j - 1) * 50 * board.Transform.Scale);
            return new Vector2(x, y);
        }
        public static Address FindAddress<CellT>(this IBoard<CellT> board, CellT cell) where CellT : ICell, new()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board.Cells[i, j].Equals(cell))
                    {
                        return new Address(i, j);
                    }
                }
            }
            throw new ArgumentException("Cell not found");
        }
        public static void DrawGrid<CellT>(this IBoard<CellT> board) where CellT : ICell, new()
        {
            LinearTransform transform = board.Transform;
            int lineGap = (int)(50 * transform.Scale);
            int lineLength = (int)(150 * transform.Scale);
            int lineWidth = (int)(2 * transform.Scale);
            int x = (int)transform.Position.X;
            int y = (int)transform.Position.Y;
            Color color = Color.LIGHTGRAY;

            DrawRectangle(x - lineWidth / 2 + lineGap / 2, y - lineLength / 2, lineWidth, lineLength, color);
            DrawRectangle(x - lineWidth / 2 - lineGap / 2, y - lineLength / 2, lineWidth, lineLength, color);
            DrawRectangle(x - lineLength / 2, y - lineWidth / 2 + lineGap / 2, lineLength, lineWidth, color);
            DrawRectangle(x - lineLength / 2, y - lineWidth / 2 - lineGap / 2, lineLength, lineWidth, color);
        }
        public static Team? Winner<CellT>(this IBoard<CellT> board) where CellT : ICell, new()
        {
            bool hasWinner;
            Team[,] cellWinners =
                from cell in board.Cells
                select cell.Team;

            // Column 0
            hasWinner = cellWinners[0, 0] != null
                && cellWinners[0, 0] == cellWinners[0, 1]
                && cellWinners[0, 1] == cellWinners[0, 2];
            if (hasWinner)
                return cellWinners[0, 0];
            // Column 1
            hasWinner = cellWinners[1, 0] != null
                && cellWinners[1, 0] == cellWinners[1, 1]
                && cellWinners[1, 1] == cellWinners[1, 2];
            if (hasWinner)
                return cellWinners[1, 0];
            // Column 2
            hasWinner = cellWinners[2, 0] != null
                && cellWinners[2, 0] == cellWinners[2, 1]
                && cellWinners[2, 1] == cellWinners[2, 2];
            if (hasWinner)
                return cellWinners[2, 0];

            // Row 0
            hasWinner = cellWinners[0, 0] != null
                && cellWinners[0, 0] == cellWinners[1, 0]
                && cellWinners[1, 0] == cellWinners[2, 0];
            if (hasWinner)
                return cellWinners[0, 0];
            // Row 1
            hasWinner = cellWinners[0, 1] != null
                && cellWinners[0, 1] == cellWinners[1, 1]
                && cellWinners[1, 1] == cellWinners[2, 1];
            if (hasWinner)
                return cellWinners[0, 1];
            // Row 2
            hasWinner = cellWinners[0, 2] != null
                && cellWinners[0, 2] == cellWinners[1, 2]
                && cellWinners[1, 2] == cellWinners[2, 2];
            if (hasWinner)
                return cellWinners[0, 2];

            // Diagonals
            hasWinner = cellWinners[1, 1] != null
                && cellWinners[0, 0] == cellWinners[1, 1]
                && cellWinners[1, 1] == cellWinners[2, 2];
            if (hasWinner)
                return cellWinners[1, 1];
            hasWinner = cellWinners[1, 1] != null
                && cellWinners[0, 2] == cellWinners[1, 1]
                && cellWinners[1, 1] == cellWinners[2, 0];
            if (hasWinner)
                return cellWinners[1, 1];

            return null;
        }
    }
}