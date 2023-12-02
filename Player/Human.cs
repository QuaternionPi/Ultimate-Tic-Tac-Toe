using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public class Human : Player
    {
        public Human(Symbol symbol, Color color) : base(symbol, color)
        {
        }
        protected Grid<Grid<Tile>>? Board;
        public override void BeginTurn(Grid<Grid<Tile>> board)
        {
            Board = board;
            Board.Clicked += HandleClickedBoard;
        }
        public override void EndTurn()
        {
            if (Board == null)
            {
                return;
            }
            Board.Clicked -= HandleClickedBoard;
            Board = null;
        }
        public override void Update()
        {

        }
        protected void HandleClickedBoard(ICell cell, IEnumerable<Address> from, bool placeable)
        {
            if (placeable == false)
            {
                return;
            }
            if (!cell.Equals(Board))
            {
                throw new Exception("Board click not from board");
            }
            InvokePlayTurn(this, from);
        }
    }
}