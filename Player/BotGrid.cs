using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe
{
    public partial class Bot
    {
        public class Grid<TCell> : IBoard<TCell>, ICell
            where TCell : ICell, new()
        {
            public Grid()
            {
                Cells = new TCell[3][];
                for (int i = 0; i < 3; i++)
                {
                    Cells[i] = new TCell[3];
                    for (int j = 0; j < 3; j++)
                    {
                        Transform2D cellTransform = new Transform2D(Vector2.Zero, 0, 1);
                        TCell cell = (TCell)new TCell().Create(null, cellTransform, false);
                        Cells[i][j] = cell;
                    }
                }
                Player = null;
            }
            public Grid(Player? player, Transform2D transform, bool placeable)
            {
                TCell baseCell = new TCell();
                Transform2D baseTransform = new(Vector2.Zero, 0, 0);
                Cells = new TCell[3][];
                for (int i = 0; i < 3; i++)
                {
                    Cells[i] = new TCell[3];
                    for (int j = 0; j < 3; j++)
                    {
                        TCell cell = (TCell)baseCell.Create(null, baseTransform, placeable);
                        Cells[i][j] = cell;
                    }
                }
                Player = this.Winner();
            }
            public Grid(Grid<TCell> original, bool placeable)
            {
                Cells = new TCell[3][];
                for (int i = 0; i < 3; i++)
                {
                    Cells[i] = new TCell[3];
                    for (int j = 0; j < 3; j++)
                    {
                        TCell cell = original.Cells[i][j];
                        TCell newCell = (TCell)cell.DeepCopyPlacable(placeable);
                        Cells[i][j] = newCell;
                    }
                }
                Player = this.Winner();
            }
            public Grid(TCell[][] cells)
            {
                Cells = cells;
                Player = this.Winner();
            }
            public Grid(Grid<TCell> original, IEnumerable<ICell> TCellrace, Player player, bool placeable)
            {
                if (TCellrace.Last().Placeable == false)
                {
                    throw new Exception("You Cannot place on that cell");
                }
                Cells = new TCell[3][];

                ICell TCelloReplace = TCellrace.Last();
                ICell targetCell = TCellrace.First();

                for (int i = 0; i < 3; i++)
                {
                    Cells[i] = new TCell[3];
                    for (int j = 0; j < 3; j++)
                    {
                        TCell cell = original.Cells[i][j];
                        if (cell.Equals(targetCell))
                        {
                            cell = (TCell)cell.Place(TCellrace.Skip(1), player, placeable);
                        }
                        else
                        {
                            cell = (TCell)cell.DeepCopyPlacable(placeable);
                        }
                        Cells[i][j] = cell;
                    }
                }

                Player = this.Winner();
                if (Cells[0][0] is Tile || Player != null)
                {
                    return;
                }

                Address nextPlayableAddress = original.PathTo(TCelloReplace).Last();
                (int nextX, int nextY) = nextPlayableAddress.XY;
                TCell nextCell = Cells[nextX][nextY];

                if (nextCell.Placeable == false)
                {
                    Cells[nextX][nextY] = (TCell)nextCell.DeepCopyPlacable(false);
                    return;
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        bool cellPlaceable = (i == nextX) && (j == nextY) && (Player == null);
                        TCell cell = (TCell)Cells[i][j].DeepCopyPlacable(cellPlaceable);

                        Cells[i][j] = cell;
                    }
                }
            }
            public Player? Player { get; }
            public bool Placeable
            {
                get
                {
                    if (Player != null)
                        return false;
                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                            if (Cells[i][j].Placeable == true)
                                return true;
                    return false;
                }
            }
            [JsonIgnore]
            public TCell[][] Cells { get; private set; }
            public List<Address> PathTo(ICell cell) => this.PathToCell(cell);
            public bool Contains(ICell cell) => this.ContainsCell(cell);
            public ICell Create(Player? player, Transform2D transform, bool placeable)
            {
                return new Grid<TCell>(player, transform, placeable);
            }
            public ICell Place(IEnumerable<ICell> TCellrace, Player player, bool placeable)
            {
                return new Grid<TCell>(this, TCellrace, player, placeable);
            }
            public ICell DeepCopyPlacable(bool placeable)
            {
                return new Grid<TCell>(this, placeable);
            }
        }
    }
}