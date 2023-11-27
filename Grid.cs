using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public class Grid : IBoard<Tile>
    {
        public Grid(LinearTransform transform)
        {
            Transform = transform;
            Cells = new Tile[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vector2 tilePosition = this.PixelPosition(new Address(i, j));
                    LinearTransform tileTransform = new LinearTransform(tilePosition, 0, 1);
                    Tile tile = new Tile(null, tileTransform);
                    Cells[i, j] = tile;
                    tile.Clicked += HandleClickedTile;
                }
            }
        }
        public List<Address> ValidPositions()
        {
            if (Team != null)
            {
                return new List<Address>();
            }
            List<Address> validPositions = new List<Address>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j].Winner() == null)
                    {
                        validPositions.Add(new Address(i, j));
                    }
                }
            }
            return validPositions;
        }
        public bool IsValidPlacement(Address address)
        {
            if (Team != null)
            {
                return false;
            }

            int x = address.X;
            int y = address.Y;
            if (Cells[x, y].Team != null)
            {
                return false;
            }
            return true;
        }
        public Tile PlaceTile(Team team, Address address)
        {
            if (IsValidPlacement(address) == false)
            {
                throw new Exception("Cannot place tile");
            }
            int x = address.X;
            int y = address.Y;
            LinearTransform transform = Cells[x, y].Transform;
            Tile tile = new Tile(team, transform);
            Cells[x, y] = tile;

            if (this.Winner() != null)
            {
                transform = new LinearTransform(Transform.Position, 0, 4);
                _victoryTile = new Tile(team, transform);
            }
            return tile;
        }
        public Address GridAddress(Tile tile)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j] == tile)
                    {
                        return new Address(i, j);
                    }
                }
            }
            throw new ArgumentException("Tile not found");
        }
        public void HandleClickedTile(object? sender, EventArgs eventArgs)
        {
            if (sender == null)
            {
                return;
            }
            ClickedEventArgs args = new ClickedEventArgs((Tile)sender);
            Clicked?.Invoke(this, args);
        }
        public void DrawPossibilities()
        {
            if (Team != null)
            {
                return;
            }
            foreach (Tile cell in Cells)
            {
                if (cell.Winner() != null)
                {
                    continue;
                }
                Vector2 position = cell.Transform.Position;
                int width = 20;
                DrawRectangle((int)position.X - width / 2, (int)position.Y - width / 2, width, width, Color.LIGHTGRAY);
            }
        }
        public void Draw()
        {
            if (Team != null)
            {
                _victoryTile?.Draw();
                return;
            }
            int lineGap = (int)(50 * Transform.Scale);
            int lineLength = (int)(150 * Transform.Scale);
            int lineWidth = (int)(2 * Transform.Scale);

            DrawRectangle((int)Transform.Position.X - lineWidth / 2 + lineGap / 2, (int)Transform.Position.Y - lineLength / 2, lineWidth, lineLength, Color.LIGHTGRAY);
            DrawRectangle((int)Transform.Position.X - lineWidth / 2 - lineGap / 2, (int)Transform.Position.Y - lineLength / 2, lineWidth, lineLength, Color.LIGHTGRAY);
            DrawRectangle((int)Transform.Position.X - lineLength / 2, (int)Transform.Position.Y - lineWidth / 2 + lineGap / 2, lineLength, lineWidth, Color.LIGHTGRAY);
            DrawRectangle((int)Transform.Position.X - lineLength / 2, (int)Transform.Position.Y - lineWidth / 2 - lineGap / 2, lineLength, lineWidth, Color.LIGHTGRAY);

            foreach (Tile tile in Cells)
            {
                tile.Draw();
            }
        }
        public void Update()
        {
            foreach (Tile tile in Cells)
            {
                tile.Update();
            }
        }
        public Team? Team { get { return this.Winner(); } }
        public LinearTransform Transform { get; }
        public event EventHandler<ClickedEventArgs>? Clicked;
        public class ClickedEventArgs : EventArgs
        {
            public ClickedEventArgs(Tile tile)
            {
                _tile = tile;
            }
            public Tile _tile;
        }
        public Tile[,] Cells { get; protected set; }
        private Tile? _victoryTile;
    }
}