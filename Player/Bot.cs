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
                var futureBoard = (Grid<Grid<Tile>>)board.Place(cellTrace, maximizer, true);
                int evaluation = Minimax(futureBoard, 4, minimizer, maximizer, true);
                if (bestEvaluation < evaluation)
                {
                    bestEvaluation = evaluation;
                    bestMove = move;
                }
            }
            return bestMove;
        }
        protected static int Minimax(
            Grid<Grid<Tile>> board,
            int depth,
            Player maximizer,
            Player minimizer,
            bool minimize)
        {
            // The base case of the recursion
            if (depth == 0)
            {
                return Evaluate(board, minimizer, maximizer);
            }
            var posibleMoves = PosibleMoves(board);
            // If the board is a winning position
            if (posibleMoves.Count == 0)
            {
                if (board.Player == maximizer)
                {
                    return 1000;
                }
                else if (board.Player == minimizer)
                {
                    return -1000;
                }
                return 0;
            }

            if (posibleMoves.Count() > 30)
            {
                depth = Math.Max(depth - 1, 1);
            }

            int bestEvaluation;
            if (minimize)
            {
                bestEvaluation = 10000;
                foreach (var move in posibleMoves)
                {
                    var cellTrace = new List<ICell>() { move.Item1, move.Item2 };
                    var futureBoard =
                        (Grid<Grid<Tile>>)board.Place(
                        cellTrace,
                        maximizer,
                        true);
                    int evaluation = -Minimax(futureBoard, depth - 1, minimizer, maximizer, true);
                    bestEvaluation = Math.Min(bestEvaluation, evaluation);
                }
            }
            else
            {
                bestEvaluation = -10000;
                foreach (var move in posibleMoves)
                {
                    var cellTrace = new List<ICell>() { move.Item1, move.Item2 };
                    var futureBoard =
                        (Grid<Grid<Tile>>)board.Place(
                        cellTrace,
                        maximizer,
                        true);
                    int evaluation = -Minimax(futureBoard, depth - 1, minimizer, maximizer, false);
                    bestEvaluation = Math.Max(bestEvaluation, evaluation);
                }
            }
            return bestEvaluation;
        }
        protected static int Evaluate(Grid<Grid<Tile>> board, Player maximizer, Player minimizer)
        {
            if (board.Player == maximizer)
            {
                return 1000;
            }
            else if (board.Player == minimizer)
            {
                return -1000;
            }
            int evaluation = 0;
            evaluation -= PosibleMoves(board).Count();
            foreach (Grid<Tile> grid in board.Cells)
            {
                if (grid.Player == maximizer)
                {
                    evaluation += 50;
                    continue;
                }
                else if (grid.Player == minimizer)
                {
                    evaluation -= 50;
                    continue;
                }
                if (grid.Cells[1, 1].Player == maximizer)
                {
                    evaluation += 15;
                }
                else if (grid.Cells[1, 1].Player == minimizer)
                {
                    evaluation -= 15;
                }
            }
            return evaluation;
        }
        protected static List<(Grid<Tile>, Tile)> PosibleMoves(Grid<Grid<Tile>> board)
        {
            List<(Grid<Tile>, Tile)> posibleMoves = new();
            foreach (Grid<Tile> grid in board.Cells)
            {
                foreach (Tile tile in grid.Cells)
                {
                    if (tile.Placeable && grid.Placeable)
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