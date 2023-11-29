using System;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Raylib_cs.Raymath;
using static Raylib_cs.KeyboardKey;

namespace UltimateTicTacToe
{
    public class Grid<CellT> : ICell where CellT : ICell, new()
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
            WinningTeamTile = new Tile(Team, victoryTileTransform, true, false);
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
            WinningTeamTile = new Tile(Team, victoryTileTransform, true, false);
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
            WinningTeamTile = new Tile(Team, victoryTileTransform, false, false);
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
        public void Draw()
        {
            if (Team != null)
            {
                WinningTeamTile?.Draw();
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
        public void HandleClickedTile(ICell cell, IEnumerable<Address> from, bool placeable)
        {
            Address address = this.FindAddress((CellT)cell);
            var newFrom = from.Prepend(address);
            Clicked?.Invoke(this, newFrom, placeable);
        }
        public Vector2 PixelPosition(Address address)
        {
            int i = address.X;
            int j = address.Y;
            int x = (int)(Transform.Position.X + (i - 1) * 50 * Transform.Scale);
            int y = (int)(Transform.Position.Y + (j - 1) * 50 * Transform.Scale);
            return new Vector2(x, y);
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
            int lineLength = (int)(150 * transform.Scale);
            int lineWidth = (int)(2 * transform.Scale);
            int x = (int)transform.Position.X;
            int y = (int)transform.Position.Y;
            Color color = Color.LIGHTGRAY;

            DrawRectangle(x - lineWidth / 2 + lineGap / 2, y - lineLength / 2, lineWidth, lineLength, color);
            DrawRectangle(x - lineWidth / 2 - lineGap / 2, y - lineLength / 2, lineWidth, lineLength, color);
            DrawRectangle(x - lineLength / 2, y - lineWidth / 2 + lineGap / 2, lineLength, lineWidth, color);
            DrawRectangle(x - lineLength / 2, y - lineWidth / 2 - lineGap / 2, lineLength, lineWidth, color);
        }
        public Team? Winner()
        {
            bool hasWinner;
            Team[,] cellWinners =
                from cell in Cells
                select cell.Team;

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
        public LinearTransform Transform { get; }
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
        public event ICell.ClickHandler? Clicked;
        public CellT[,] Cells { get; }
        protected Tile? WinningTeamTile;
    }
}