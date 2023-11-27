using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public class SuperGrid : IBoard<Grid>
    {
        public SuperGrid(Vector2 position)
        {
            Transform = new LinearTransform(position, 0, 1);
            _validGrids = new List<Vector2>();
            Cells = new Grid[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int x = 200 * (i - 1) + (int)Transform.Position.X;
                    int y = 200 * (j - 1) + (int)Transform.Position.Y;
                    Grid grid = new Grid(new Vector2(x, y));
                    Cells[i, j] = grid;
                    grid.Clicked += HandleClickedGrid;
                    _validGrids.Add(new Vector2(i, j));
                }
            }
        }
        public List<Vector2> ValidPositions()
        {
            List<Vector2> validPositions = new List<Vector2>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j].Winner() != null)
                    {
                        validPositions.Add(new Vector2(i, j));
                    }
                }
            }
            return validPositions;
        }
        public bool IsValidPlacement(Vector2 position, Vector2 subPosition)
        {
            if (Team != null)
            {
                return false;
            }
            int x = (int)position.X;
            int y = (int)position.Y;
            if (x > 2 | x < 0 | y > 2 | y < 0)
            {
                throw new IndexOutOfRangeException("Position not in grid");
            }
            if (_validGrids.Contains(position))
            {
                Grid grid = Cells[x, y];
                if (grid.IsValidPlacement(subPosition))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        public Tile PlaceTile(Team team, Vector2 position, Vector2 subPosition)
        {
            if (IsValidPlacement(position, subPosition) == false)
            {
                throw new Exception("Cannot place tile");
            }
            int x = (int)position.X;
            int y = (int)position.Y;
            Grid grid = Cells[x, y];
            Tile tile = grid.PlaceTile(team, subPosition);

            _validGrids.Clear();
            int i = (int)subPosition.X;
            int j = (int)subPosition.Y;
            if (Cells[i, j].ValidPositions().Count() > 0)
            {
                _validGrids.Add(new Vector2(i, j));
            }
            else
            {
                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < 3; j++)
                    {
                        _validGrids.Add(new Vector2(i, j));
                    }
                }
            }
            return tile;
        }
        public Vector2 GridPosition(Grid grid)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j] == grid)
                    {
                        return new Vector2(i, j);
                    }
                }
            }
            throw new ArgumentException("Tile not found");
        }
        public void HandleClickedGrid(object? sender, Grid.ClickedEventArgs args)
        {
            if (sender == null)
            {
                return;
            }
            Tile tile = args._tile;
            Grid grid = (Grid)sender;
            ClickedEventArgs newArgs = new ClickedEventArgs(tile, grid);
            Clicked?.Invoke(this, newArgs);
        }
        public void DrawPossibilities()
        {
            foreach (Vector2 position in _validGrids)
            {
                int i = (int)position.X;
                int j = (int)position.Y;
                Cells[i, j].DrawPossibilities();
            }
        }
        public void Draw()
        {
            int lineGap = 200;
            int lineLength = 550;
            int lineWidth = 8;
            DrawRectangle((int)Transform.Position.X - lineWidth / 2 + lineGap / 2, (int)Transform.Position.Y - lineLength / 2, lineWidth, lineLength, Color.LIGHTGRAY);
            DrawRectangle((int)Transform.Position.X - lineWidth / 2 - lineGap / 2, (int)Transform.Position.Y - lineLength / 2, lineWidth, lineLength, Color.LIGHTGRAY);
            DrawRectangle((int)Transform.Position.X - lineLength / 2, (int)Transform.Position.Y - lineWidth / 2 + lineGap / 2, lineLength, lineWidth, Color.LIGHTGRAY);
            DrawRectangle((int)Transform.Position.X - lineLength / 2, (int)Transform.Position.Y - lineWidth / 2 - lineGap / 2, lineLength, lineWidth, Color.LIGHTGRAY);

            foreach (Grid grid in Cells)
            {
                grid.Draw();
            }
        }
        public void Update()
        {
            foreach (Grid grid in Cells)
            {
                grid.Update();
            }
        }
        public Tile.TileShape Shape
        {
            get; protected set;
        }
        public LinearTransform Transform { get; }
        public Team? Team { get { return this.Winner(); } }
        public event EventHandler<ClickedEventArgs>? Clicked;
        public class ClickedEventArgs
        {
            public ClickedEventArgs(Tile tile, Grid grid)
            {
                _tile = tile;
                _grid = grid;
            }
            public Tile _tile;
            public Grid _grid;
        }
        public Grid[,] Cells { get; protected set; }
        private List<Vector2> _validGrids;
    }
}