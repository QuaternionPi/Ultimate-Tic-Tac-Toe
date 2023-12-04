using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public class Bot : Player
    {
        public Bot(Symbol symbol, Color color) : base(symbol, color)
        {
        }
        protected Grid<Grid<Tile>>? Board;
        public override void BeginTurn(Grid<Grid<Tile>> board)
        {
            Board = board;
            List<(Grid<Tile>, Tile)> posibleMoves = new();
            foreach (Grid<Tile> grid in board.Cells)
            {
                foreach (Tile tile in grid.Cells)
                {
                    if (tile.Placeable)
                    {
                        posibleMoves.Add((grid, tile));
                    }
                }
            }
            Console.WriteLine($"There are {posibleMoves.Count} posible moves");
            var move = posibleMoves[0];
            MakeMove(move.Item1, move.Item2);
        }
        public override void EndTurn()
        {
            if (Board == null)
            {
                return;
            }
            Board = null;
        }
        public override void Update()
        {

        }
        protected static int Evaluate(Grid<Grid<Tile>> board)
        {
            return -1;
        }
        protected void MakeMove(Grid<Tile> grid, Tile tile)
        {
            if (Board == null)
            {
                throw new Exception("Board can't be null when youre playing a move");
            }
            InvokePlayTurn(this, new List<ICell>() { Board, grid, tile });
        }
    }
}