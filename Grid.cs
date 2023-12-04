using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;
using System.Runtime.InteropServices.Marshalling;

namespace UltimateTicTacToe
{
    public class Grid<CellT> : ICell where CellT : ICell, new()
    {
        public Grid()
        {
            Transform = new LinearTransform(Vector2.Zero, 0, 1);
            Cells = new CellT[3, 3];
        }
        public Grid(Player? player, LinearTransform transform, bool placeable)
        {
            Transform = transform;
            Cells = new CellT[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vector2 tilePosition = PixelPosition(new Address(i, j));
                    LinearTransform tileTransform = new LinearTransform(tilePosition, 0, 1);
                    CellT cell = (CellT)new CellT().Create(null, tileTransform, placeable);
                    Cells[i, j] = cell;
                    cell.Clicked += HandleClickedTile;
                }
            }
            Player = Winner();
            LinearTransform victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
            WinningPlayerTile = new Tile(Player, victoryTileTransform, true);
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
                    CellT newCell = (CellT)cell.DeepCopyPlacable(placeable);
                    Cells[i, j] = newCell;
                    newCell.Clicked += HandleClickedTile;
                }
            }
            Player = Winner();
            LinearTransform victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
            WinningPlayerTile = new Tile(Player, victoryTileTransform, true);
        }
        public Grid(Grid<CellT> original, IEnumerable<ICell> cellTrace, Player player, bool placeable)
        {
            if (cellTrace.Last().Placeable == false)
            {
                throw new Exception("You Cannot place on that cell");
            }
            Transform = original.Transform;
            Cells = new CellT[3, 3];

            ICell cellToReplace = cellTrace.Last();
            ICell targetCell = cellTrace.First();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    CellT cell = original.Cells[i, j];
                    if (cell.Equals(targetCell))
                    {
                        cell = (CellT)cell.Place(cellTrace.Skip(1), player, placeable);
                    }
                    else
                    {
                        cell = (CellT)cell.DeepCopyPlacable(placeable);
                    }
                    Cells[i, j] = cell;
                    cell.Clicked += HandleClickedTile;
                }
            }

            Player = Winner();
            LinearTransform victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
            WinningPlayerTile = new Tile(Player, victoryTileTransform, false);
            if (Cells[0, 0] is Tile || Player != null)
            {
                return;
            }

            Address nextPlayableAddress = original.PathTo(cellToReplace).Last();
            (int nextX, int nextY) = nextPlayableAddress.XY;
            CellT nextCell = Cells[nextX, nextY];

            if (nextCell.Placeable == false)
            {
                Cells[nextX, nextY] = (CellT)nextCell.DeepCopyPlacable(false);
                return;
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    CellT cell = original.Cells[i, j];
                    bool cellPlaceable = i == nextX && j == nextY;
                    if (cell.Equals(targetCell))
                    {
                        cell = (CellT)cell.Place(cellTrace.Skip(1), player, cellPlaceable);
                    }
                    else
                    {
                        cell = (CellT)cell.DeepCopyPlacable(cellPlaceable);
                    }
                    Cells[i, j] = cell;
                    cell.Clicked += HandleClickedTile;
                }
            }
        }
        public LinearTransform Transform { get; }
        public Player? Player { get; }
        public bool Placeable
        {
            get
            {
                return Player == null && (
                    from CellT cell in Cells
                    where cell.Placeable == true
                    select cell
                    ).Any();
            }
        }
        public event ICell.ClickHandler? Clicked;
        public CellT[,] Cells { get; }
        protected Tile? WinningPlayerTile { get; }
        public void Draw()
        {
            if (Player != null)
            {
                WinningPlayerTile?.Draw();
                return;
            }
            DrawGrid();

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
        public ICell Create(Player? player, LinearTransform transform, bool placeable)
        {
            return new Grid<CellT>(player, transform, placeable);
        }
        public ICell Place(IEnumerable<ICell> cellTrace, Player player, bool placeable)
        {
            return new Grid<CellT>(this, cellTrace, player, placeable);
        }
        public ICell DeepCopyPlacable(bool placeable)
        {
            return new Grid<CellT>(this, placeable);
        }
        public void HandleClickedTile(IEnumerable<ICell> cells)
        {
            IEnumerable<ICell> newCells = cells.Prepend(this).ToList();
            Clicked?.Invoke(newCells);
        }
        public Vector2 PixelPosition(Address address)
        {
            (int i, int j) = address.XY;
            int x = (int)(Transform.Position.X + (i - 1) * 50 * Transform.Scale);
            int y = (int)(Transform.Position.Y + (j - 1) * 50 * Transform.Scale);
            return new Vector2(x, y);
        }
        public IEnumerable<Address> PathTo(ICell cell)
        {
            if (Contains(cell) == false)
            {
                throw new Exception($"Cell: {cell} is not contained. There is no path to it");
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j].Contains(cell))
                    {
                        Address address = new(i, j);
                        return Cells[i, j].PathTo(cell).Prepend(address);
                    }
                }
            }
            throw new Exception($"Cell: {cell} was not found");
        }
        public bool Contains(ICell cell)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j].Equals(cell))
                    {
                        return true;
                    }
                    else if (Cells[i, j].Contains(cell))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public Address FindAddress(CellT cell)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i, j].Equals(cell))
                    {
                        return new Address(i, j);
                    }
                }
            }
            throw new ArgumentException("Cell not found");
        }
        public void DrawGrid()
        {
            LinearTransform transform = Transform;
            int lineGap = (int)(50 * transform.Scale);
            int lineLength = (int)(130 * transform.Scale);
            int lineWidth = (int)(2 * transform.Scale);
            int x = (int)transform.Position.X;
            int y = (int)transform.Position.Y;
            Color color = Color.LIGHTGRAY;

            DrawRectangle(x - lineWidth / 2 + lineGap / 2, y - lineLength / 2, lineWidth, lineLength, color);
            DrawRectangle(x - lineWidth / 2 - lineGap / 2, y - lineLength / 2, lineWidth, lineLength, color);
            DrawRectangle(x - lineLength / 2, y - lineWidth / 2 + lineGap / 2, lineLength, lineWidth, color);
            DrawRectangle(x - lineLength / 2, y - lineWidth / 2 - lineGap / 2, lineLength, lineWidth, color);
        }
        public Player? Winner()
        {
            bool hasWinner;
            Player[,] cellWinners =
                from cell in Cells
                select cell.Player;

            // Column 0
            hasWinner = cellWinners[0, 0] != null
                && cellWinners[0, 0] == cellWinners[0, 1]
                && cellWinners[0, 1] == cellWinners[0, 2];
            if (hasWinner)
                return cellWinners[0, 0];
            // Column 1
            hasWinner = cellWinners[1, 0] != null
                && cellWinners[1, 0] == cellWinners[1, 1]
                && cellWinners[1, 1] == cellWinners[1, 2];
            if (hasWinner)
                return cellWinners[1, 0];
            // Column 2
            hasWinner = cellWinners[2, 0] != null
                && cellWinners[2, 0] == cellWinners[2, 1]
                && cellWinners[2, 1] == cellWinners[2, 2];
            if (hasWinner)
                return cellWinners[2, 0];

            // Row 0
            hasWinner = cellWinners[0, 0] != null
                && cellWinners[0, 0] == cellWinners[1, 0]
                && cellWinners[1, 0] == cellWinners[2, 0];
            if (hasWinner)
                return cellWinners[0, 0];
            // Row 1
            hasWinner = cellWinners[0, 1] != null
                && cellWinners[0, 1] == cellWinners[1, 1]
                && cellWinners[1, 1] == cellWinners[2, 1];
            if (hasWinner)
                return cellWinners[0, 1];
            // Row 2
            hasWinner = cellWinners[0, 2] != null
                && cellWinners[0, 2] == cellWinners[1, 2]
                && cellWinners[1, 2] == cellWinners[2, 2];
            if (hasWinner)
                return cellWinners[0, 2];

            // Diagonals
            hasWinner = cellWinners[1, 1] != null
                && cellWinners[0, 0] == cellWinners[1, 1]
                && cellWinners[1, 1] == cellWinners[2, 2];
            if (hasWinner)
                return cellWinners[1, 1];
            hasWinner = cellWinners[1, 1] != null
                && cellWinners[0, 2] == cellWinners[1, 1]
                && cellWinners[1, 1] == cellWinners[2, 0];
            if (hasWinner)
                return cellWinners[1, 1];

            return null;
        }
    }
}