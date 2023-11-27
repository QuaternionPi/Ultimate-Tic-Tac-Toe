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
        public SuperGrid(LinearTransform transform)
        {
            Transform = transform;
            _validGrids = new List<Address>();
            Cells = new Grid[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vector2 tilePosition = this.PixelPosition(new Address(i, j));
                    LinearTransform tileTransform = new LinearTransform(tilePosition, 0, 1);
                    Grid grid = new Grid(tileTransform);
                    Cells[i, j] = grid;
                    grid.Clicked += HandleClickedGrid;
                    _validGrids.Add(new Address(i, j));
                }
            }
        }
        public List<Address> ValidPositions()
        {
            List<Address> validPositions = new List<Address>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j].Winner() != null)
                    {
                        validPositions.Add(new Address(i, j));
                    }
                }
            }
            return validPositions;
        }
        public bool IsValidPlacement(Address address, Address subAddress)
        {
            if (Team != null)
            {
                return false;
            }
            int x = address.X;
            int y = address.Y;
            if (_validGrids.Contains(address))
            {
                Grid grid = Cells[x, y];
                if (grid.IsValidPlacement(subAddress))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        public Tile PlaceTile(Team team, Address address, Address subAddress)
        {
            if (IsValidPlacement(address, subAddress) == false)
            {
                throw new Exception("Cannot place tile");
            }
            int x = (int)address.X;
            int y = (int)address.Y;
            Grid grid = Cells[x, y];
            Tile tile = grid.PlaceTile(team, subAddress);

            _validGrids.Clear();
            int i = (int)subAddress.X;
            int j = (int)subAddress.Y;
            if (Cells[i, j].ValidPositions().Count() > 0)
            {
                _validGrids.Add(new Address(i, j));
            }
            else
            {
                for (i = 0; i < 3; i++)
                {
                    for (j = 0; j < 3; j++)
                    {
                        _validGrids.Add(new Address(i, j));
                    }
                }
            }
            return tile;
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
            foreach (Address address in _validGrids)
            {
                int x = address.X;
                int y = address.Y;
                Cells[x, y].DrawPossibilities();
            }
        }
        public void Draw()
        {
            int lineGap = (int)(50 * Transform.Scale);
            int lineLength = (int)(150 * Transform.Scale);
            int lineWidth = (int)(2 * Transform.Scale);

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
        private List<Address> _validGrids;
    }
}