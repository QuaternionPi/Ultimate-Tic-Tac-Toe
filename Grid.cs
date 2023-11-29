using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public class Grid<CellT> : IBoard<CellT> where CellT : ICell, new()
    {
        public Grid()
        {
            Transform = new LinearTransform(Vector2.Zero, 0, 1);
            Cells = new CellT[3, 3];
        }
        public Grid(Team? team, LinearTransform transform, bool placeable, bool drawGray)
        {
            Transform = transform;
            Cells = new CellT[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vector2 tilePosition = this.PixelPosition(new Address(i, j));
                    LinearTransform tileTransform = new LinearTransform(tilePosition, 0, 1);
                    CellT cell = (CellT)new CellT().Create(null, tileTransform, placeable, false);
                    Cells[i, j] = cell;
                    cell.Clicked += HandleClickedTile;
                }
            }
            LinearTransform victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
            _victoryTile = new Tile(Team, victoryTileTransform, true, false);
        }
        public Grid(Grid<CellT> original, bool placeable)
        {
            Transform = original.Transform;
            Cells = new CellT[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    CellT cell = original.Cells[i, j];
                    CellT newCell = (CellT)cell.Clone(placeable);
                    Cells[i, j] = newCell;
                    newCell.Clicked += HandleClickedTile;
                }
            }
            LinearTransform victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
            _victoryTile = new Tile(Team, victoryTileTransform, true, false);
        }
        public Grid(Grid<CellT> original, IEnumerable<Address> path, Team team, bool placeable, bool isRoot)
        {
            Address address = path.First();
            Transform = original.Transform;
            Cells = new CellT[3, 3];

            int x = address.X;
            int y = address.Y;
            CellT targetCell = original.Cells[x, y];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    CellT cell = original.Cells[i, j];
                    CellT newCell;
                    if (cell.Equals(targetCell))
                    {
                        newCell = (CellT)cell.Place(path.Skip(1), team, placeable, false);
                    }
                    else
                    {
                        newCell = (CellT)cell.Clone(placeable);
                    }
                    Cells[i, j] = newCell;
                }
            }

            LinearTransform victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
            _victoryTile = new Tile(Team, victoryTileTransform, false, false);
            if (path.Count() == 1 || Team != null)
            {
                foreach (CellT cell in Cells)
                {
                    cell.Clicked += HandleClickedTile;
                }
                return;
            }

            Address nextPlayableAddress = path.Skip(1).First();
            int nextX = nextPlayableAddress.X;
            int nextY = nextPlayableAddress.Y;

            CellT nextCell = Cells[nextX, nextY];

            if (nextCell.Placeable == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        CellT cell = original.Cells[i, j];
                        CellT newCell;
                        bool cellPlaceable = i == nextX && j == nextY;
                        if (cell.Equals(targetCell))
                        {
                            newCell = (CellT)cell.Place(path.Skip(1), team, cellPlaceable, false);
                        }
                        else
                        {
                            newCell = (CellT)cell.Clone(cellPlaceable);
                        }
                        Cells[i, j] = newCell;
                    }
                }
            }

            foreach (CellT cell in Cells)
            {
                cell.Clicked += HandleClickedTile;
            }
        }
        public ICell Create(Team? team, LinearTransform transform, bool placeable, bool drawGray)
        {
            return new Grid<CellT>(team, transform, placeable, drawGray);
        }
        public ICell Place(IEnumerable<Address> path, Team team, bool placeable, bool isRoot)
        {
            return new Grid<CellT>(this, path, team, placeable, isRoot);
        }
        public ICell Clone(bool placeable)
        {
            return new Grid<CellT>(this, placeable);
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
        public void HandleClickedTile(ICell cell, IEnumerable<Address> from, bool placeable)
        {
            Address address = this.FindAddress((CellT)cell);
            var newFrom = from.Prepend(address);
            Clicked?.Invoke(this, newFrom, placeable && IsValidPlacement(address));
        }
        public void Draw()
        {
            if (Team != null)
            {
                _victoryTile?.Draw();
                return;
            }
            this.DrawGrid();

            foreach (CellT cell in Cells)
            {
                cell.Draw();
            }
        }
        public void Update()
        {
            foreach (CellT cell in Cells)
            {
                cell.Update();
            }
        }
        public Team? Team { get { return this.Winner(); } }
        public bool Placeable
        {
            get
            {
                return Team == null && (
                    from CellT cell in Cells
                    where cell.Placeable == true
                    select cell
                    ).Any();
            }
        }
        public LinearTransform Transform { get; }
        public event ICell.ClickHandler? Clicked;
        public CellT[,] Cells { get; }
        public CellT? LastPlaced { get; }
        private Tile? _victoryTile;
    }
}