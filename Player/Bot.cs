using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    public partial class Bot : Player
    {
        public Bot(Symbol symbol, Color color, int score) : base(symbol, color, score)
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
        protected static Grid<Grid<Tile>> Convert(Game.Grid<Game.Grid<Game.Tile>> board)
        {
            Grid<Tile>[,] newGrids = new Grid<Tile>[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Game.Grid<Game.Tile> grid = board.Cells[i, j];
                    Tile[,] newTiles = new Tile[3, 3];
                    for (int k = 0; k < 3; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            Game.Tile tile = grid.Cells[k, l];
                            Player? player = tile.Player;
                            bool placeable = tile.Placeable;
                            Tile newTile = new(player, placeable);
                            newTiles[k, l] = newTile;
                        }
                    }
                    Grid<Tile> newGrid = new(newTiles);
                    newGrids[i, j] = newGrid;
                }
            }
            return new(newGrids);
        }
        protected static (Game.Grid<Game.Tile>, Game.Tile) BestMove(
            Game.Grid<Game.Grid<Game.Tile>> board,
            IEnumerable<(Game.Grid<Game.Tile>, Game.Tile)> moves,
            Player player,
            Player opponent
            )
        {
            if (moves.Any() == false)
            {
                throw new Exception("Cannot choose best move from no moves");
            }
            int alpha = -100000;
            int beta = 100000;
            int depth;
            if (moves.Count() < 7)
            {
                depth = 6;
            }
            else if (moves.Count() < 30)
            {
                depth = 5;
            }
            else
            {
                depth = 4;
            }

            var evaluatedMoves =
                from move in moves.AsParallel()
                select (move,
                    -Minimax(
                        Convert((Game.Grid<Game.Grid<Game.Tile>>)board.Place(
                            new List<ICell>() { move.Item1, move.Item2 },
                            player,
                            true)),
                        depth,
                        -beta,
                        -alpha,
                        opponent,
                        player));

            int minEvaluation = 10000;
            (Game.Grid<Game.Tile>, Game.Tile) bestMove = moves.First();

            foreach (var (move, evaluation) in evaluatedMoves)
            {
                if (minEvaluation > evaluation)
                {
                    minEvaluation = evaluation;
                    bestMove = move;
                }
            }
            return bestMove;
        }
        protected static int Minimax(
            Grid<Grid<Tile>> board,
            int depth,
            int alpha,
            int beta,
            Player player,
            Player opponent)
        {
            // The base case of the recursion or board is winning position
            if (depth == 0 || board.Player != null)
            {
                return Evaluate(board, opponent, player);
            }

            var posibleMoves = PosibleMoves(board);
            int minEvaluation = 10000;
            foreach (var move in posibleMoves)
            {
                var cellTrace = new List<ICell>() { move.Item1, move.Item2 };
                var placedBoard = (Grid<Grid<Tile>>)board.Place(cellTrace, player, true);
                var evaluation = -Minimax(placedBoard, depth - 1, -beta, -alpha, opponent, player);
                minEvaluation = Math.Min(minEvaluation, evaluation);
                beta = Math.Min(beta, evaluation);
                if (beta <= alpha)
                    break;
            }
            return minEvaluation;
        }
        protected static int Award<T>(int amount, T? compair, T positive, T negative) where T : class
        {
            if (positive.Equals(compair))
                return amount;
            if (negative.Equals(compair))
                return -amount;
            return 0;
        }
        protected static int Evaluate(Grid<Grid<Tile>> board, Player player, Player opponent)
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
            evaluation -= NumPosibleMoves(board);
            foreach (Grid<Tile> grid in board.Cells)
            {
                evaluation += Award(100, grid.Player, player, opponent);
                evaluation += Award(10, grid.Cells[1, 1].Player, player, opponent);
                evaluation += Award(5, grid.Cells[0, 0].Player, player, opponent);
                evaluation += Award(5, grid.Cells[0, 2].Player, player, opponent);
                evaluation += Award(5, grid.Cells[2, 0].Player, player, opponent);
                evaluation += Award(5, grid.Cells[2, 2].Player, player, opponent);
            }
            evaluation += Award(50, board.Cells[1, 1].Player, player, opponent);
            evaluation += Award(25, board.Cells[0, 0].Player, player, opponent);
            evaluation += Award(25, board.Cells[0, 2].Player, player, opponent);
            evaluation += Award(25, board.Cells[2, 0].Player, player, opponent);
            evaluation += Award(25, board.Cells[2, 2].Player, player, opponent);
            return evaluation;
        }
        protected static int NumPosibleMoves(Grid<Grid<Tile>> board)
        {
            int count = 0;
            foreach (Grid<Tile> grid in board.Cells)
            {
                if (grid.Placeable)
                    foreach (Tile tile in grid.Cells)
                    {
                        count++;
                    }
            }
            return count;
        }
        protected static List<(Grid<Tile>, Tile)> PosibleMoves(Grid<Grid<Tile>> board)
        {
            List<(Grid<Tile>, Tile)> posibleMoves = new();
            foreach (Grid<Tile> grid in board.Cells)
            {
                if (grid.Placeable)
                    foreach (Tile tile in grid.Cells)
                    {
                        if (tile.Placeable)
                            posibleMoves.Add((grid, tile));
                    }
            }
            return posibleMoves;
        }
        protected static List<(Game.Grid<Game.Tile>, Game.Tile)> PosibleMoves(Game.Grid<Game.Grid<Game.Tile>> board)
        {
            List<(Game.Grid<Game.Tile>, Game.Tile)> posibleMoves = new();
            foreach (Game.Grid<Game.Tile> grid in board.Cells)
            {
                if (grid.Placeable)
                    foreach (Game.Tile tile in grid.Cells)
                    {
                        if (tile.Placeable)
                            posibleMoves.Add((grid, tile));
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
            InvokePlayTurn(this, new List<ICell>() { Board, grid, tile });
        }
    }
}