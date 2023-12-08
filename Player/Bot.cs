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
            Player maximizer,
            Player minimizer
            )
        {
            if (moves.Count() == 0)
            {
                throw new Exception("Cannot choose best move from no moves");
            }
            (Game.Grid<Game.Tile>, Game.Tile) bestMove = moves.First();
            int bestEvaluation = -10000;
            foreach (var move in moves)
            {
                var cellTrace = new List<Game.ICell>() { move.Item1, move.Item2 };
                var futureBoard = (Game.Grid<Game.Grid<Game.Tile>>)board.Place(cellTrace, maximizer, true);
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
            Game.Grid<Game.Grid<Game.Tile>> board,
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
                    return -1500;
                }
                else if (board.Player == minimizer)
                {
                    return 1500;
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
            }
            else
            {
                bestEvaluation = -10000;
            }
            foreach (var move in posibleMoves)
            {
                var cellTrace = new List<Game.ICell>() { move.Item1, move.Item2 };
                var futureBoard = (Game.Grid<Game.Grid<Game.Tile>>)board.Place(cellTrace, maximizer, true);
                int evaluation = -Minimax(futureBoard, depth - 1, minimizer, maximizer, !minimize);

                if (minimize)
                    bestEvaluation = Math.Min(bestEvaluation, evaluation);
                else
                    bestEvaluation = Math.Max(bestEvaluation, evaluation);
            }
            return bestEvaluation;
        }
        protected static int Evaluate(Game.Grid<Game.Grid<Game.Tile>> board, Player maximizer, Player minimizer)
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
            foreach (Game.Grid<Game.Tile> grid in board.Cells)
            {
                if (grid.Player == maximizer)
                {
                    evaluation += 100;
                    continue;
                }
                else if (grid.Player == minimizer)
                {
                    evaluation -= 100;
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
            if (board.Cells[1, 1].Player == maximizer)
            {
                evaluation += 150;
            }
            if (board.Cells[1, 1].Player == minimizer)
            {
                evaluation -= 150;
            }
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