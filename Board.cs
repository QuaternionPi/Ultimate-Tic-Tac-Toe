using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public interface IBoard<CellT> : IDrawable, IUpdateable, ICell where CellT : ICell
    {
        public CellT[,] Cells { get; }
    }
    public static class BoardExtensions
    {
        public static Team? Winner<CellT>(this IBoard<CellT> board) where CellT : ICell
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