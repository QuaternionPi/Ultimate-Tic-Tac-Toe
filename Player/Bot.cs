using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    public class Bot : Player
    {
        public Bot(Symbol symbol, Color color) : base(symbol, color)
        {
        }
        protected Game.Grid<Game.Grid<Game.Tile>>? Board;
        public override void BeginTurn(Game.Grid<Game.Grid<Game.Tile>> board, Player opponent)
        {
            Board = board;
            List<(Game.Grid<Game.Tile>, Game.Tile)> posibleMoves = PosibleMoves(board);
            var move = BestMove(board, posibleMoves, this, opponent);
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
        protected static (Game.Grid<Game.Tile>, Game.Tile) BestMove(
            Game.Grid<Game.Grid<Game.Tile>> board,
            IEnumerable<(Game.Grid<Game.Tile>, Game.Tile)> moves,
            Player player,
            Player opponent
            )
        {
            if (moves.Count() == 0)
            {
                throw new Exception("Cannot choose best move from no moves");
            }
            (Game.Grid<Game.Tile>, Game.Tile) bestMove = moves.First();
            int bestEvaluation = 10000;
            foreach (var move in moves)
            {
                var cellTrace = new List<Game.ICell>() { move.Item1, move.Item2 };
                var futureBoard = (Game.Grid<Game.Grid<Game.Tile>>)board.Place(cellTrace, player, true);
                int evaluation = -Minimax(futureBoard, 4, opponent, player);
                if (bestEvaluation > evaluation)
                {
                    bestEvaluation = evaluation;
                    bestMove = move;
                }
            }
            return bestMove;
        }
        protected static int Minimax(
            Game.Grid<Game.Grid<Game.Tile>> board,
            int depth,
            Player player,
            Player opponent)
        {
            // The base case of the recursion
            if (depth == 0)
            {
                return Evaluate(board, opponent, player);
            }
            var posibleMoves = PosibleMoves(board);
            // If the board is a winning position
            if (board.Player == player)
            {
                return -1500;
            }
            else if (board.Player == opponent)
            {
                return 1500;
            }

            if (posibleMoves.Count() > 30)
            {
                depth = Math.Max(depth - 1, 1);
            }

            int bestEvaluation = 10000;
            foreach (var move in posibleMoves)
            {
                var cellTrace = new List<Game.ICell>() { move.Item1, move.Item2 };
                var futureBoard = (Game.Grid<Game.Grid<Game.Tile>>)board.Place(cellTrace, player, true);
                int evaluation = -Minimax(futureBoard, depth - 1, opponent, player);

                bestEvaluation = Math.Min(bestEvaluation, evaluation);
            }
            return bestEvaluation;
        }
        protected static int Award<T>(int amount, T? compair, T positive, T negative) where T : class
        {
            if (positive.Equals(compair))
                return amount;
            if (negative.Equals(compair))
                return -amount;
            return 0;
        }
        protected static int Evaluate(Game.Grid<Game.Grid<Game.Tile>> board, Player player, Player opponent)
        {
            if (board.Player == player)
            {
                return 1000;
            }
            else if (board.Player == opponent)
            {
                return -1000;
            }
            int evaluation = 0;
            evaluation -= PosibleMoves(board).Count;
            foreach (Game.Grid<Game.Tile> grid in board.Cells)
            {
                evaluation += Award(100, grid.Player, player, opponent);
                evaluation += Award(15, grid.Cells[1, 1].Player, player, opponent);
            }
            evaluation += Award(100, board.Cells[1, 1].Player, player, opponent);
            return evaluation;
        }
        protected static List<(Game.Grid<Game.Tile>, Game.Tile)> PosibleMoves(Game.Grid<Game.Grid<Game.Tile>> board)
        {
            List<(Game.Grid<Game.Tile>, Game.Tile)> posibleMoves = new();
            foreach (Game.Grid<Game.Tile> grid in board.Cells)
            {
                foreach (Game.Tile tile in grid.Cells)
                {
                    if (tile.Placeable && grid.Placeable)
                    {
                        posibleMoves.Add((grid, tile));
                    }
                }
            }
            return posibleMoves;
        }
        protected void MakeMove(Game.Grid<Game.Tile> grid, Game.Tile tile)
        {
            if (Board == null)
            {
                throw new Exception("Board can't be null when youre playing a move");
            }
            InvokePlayTurn(this, new List<Game.ICell>() { Board, grid, tile });
        }
    }
}