using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;
using Microsoft.VisualBasic;

namespace UltimateTicTacToe
{
    public class Bot : Player
    {
        public Bot(Symbol symbol, Color color) : base(symbol, color)
        {
        }
        protected Grid<Grid<Tile>>? Board;
        public override void BeginTurn(Grid<Grid<Tile>> board, Player opponent)
        {
            Board = board;
            List<(Grid<Tile>, Tile)> posibleMoves = PosibleMoves(board);
            Console.WriteLine($"There are {posibleMoves.Count} posible moves");
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
        protected static (Grid<Tile>, Tile) BestMove(
            Grid<Grid<Tile>> board,
            IEnumerable<(Grid<Tile>, Tile)> moves,
            Player maximizer,
            Player minimizer
            )
        {
            if (moves.Count() == 0)
            {
                throw new Exception("Cannot choose best move from no moves");
            }

            var posibleMoves = PosibleMoves(board);
            (Grid<Tile>, Tile) bestMove = posibleMoves[0];
            int bestEvaluation = -10000;
            foreach (var move in posibleMoves)
            {
                var cellTrace = new List<ICell>() { move.Item1, move.Item2 };
                var futureBoard =
                    (Grid<Grid<Tile>>)board.Place(
                    cellTrace,
                    maximizer,
                    true);
                int evaluation = Minimax(futureBoard, 2, maximizer, minimizer);
                if (bestEvaluation < evaluation)
                {
                    bestEvaluation = evaluation;
                    bestMove = move;
                }
            }
            return bestMove;
        }
        protected static int Minimax(Grid<Grid<Tile>> board, int depth, Player maximizer, Player minimizer)
        {
            // The base case of the recursion
            if (depth == 0)
            {
                Console.WriteLine("Minimax Evaluate Called");
                return Evaluate(board, maximizer);
            }
            var posibleMoves = PosibleMoves(board);
            int bestEvaluation = -10000;
            foreach (var move in posibleMoves)
            {
                var cellTrace = new List<ICell>() { move.Item1, move.Item2 };
                var futureBoard =
                    (Grid<Grid<Tile>>)board.Place(
                    cellTrace,
                    maximizer,
                    true);
                int evaluation = Minimax(futureBoard, depth - 1, minimizer, maximizer);
                bestEvaluation = Math.Max(bestEvaluation, evaluation);
            }
            return bestEvaluation;
        }
        protected static int Evaluate(Grid<Grid<Tile>> board, Player player)
        {
            if (board.Player == null)
            {
                return 0;
            }
            else if (board.Player == player)
            {
                return 100;
            }
            else
            {
                return -100;
            }
        }
        protected static List<(Grid<Tile>, Tile)> PosibleMoves(Grid<Grid<Tile>> board)
        {
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
            return posibleMoves;
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