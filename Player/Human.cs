using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    public class Human : Player
    {
        public Human(Symbol symbol, Color color, int score) : base(symbol, color, score)
        {
        }
        protected Game.Grid<Game.Grid<Game.Tile>>? Board;
        public override void BeginTurn(Game.Grid<Game.Grid<Game.Tile>> board, Player opponent)
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
        protected void HandleClickedBoard(IEnumerable<ICell> cells)
        {
            if (cells.Last().Placeable == false)
            {
                return;
            }
            if (!cells.First().Equals(Board))
            {
                throw new Exception("Board click not from board");
            }
            InvokePlayTurn(this, cells);
        }
    }
}