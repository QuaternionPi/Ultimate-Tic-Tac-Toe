using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe.Game;

public class Grid<TCell> : IDrawable, IUpdateable, ITransitionable, IBoard<TCell>, IClickableCell
where TCell : IDrawable, IUpdateable, ITransitionable, IClickableCell, new()
{
    public Grid()
    {
        Transform = new Transform2D(Vector2.Zero, 0, 1);
        Cells = new TCell[3][];
        for (int i = 0; i < 3; i++)
        {
            Cells[i] = new TCell[3];
            for (int j = 0; j < 3; j++)
            {
                Vector2 cellPosition = PixelPosition(new Address(i, j));
                Transform2D cellTransform = new Transform2D(cellPosition, 0, 1);
                TCell cell = (TCell)new TCell().Create(null, cellTransform, false);
                Cells[i][j] = cell;
                cell.Clicked += HandleClickedCell;
            }
        }
        Player = null;
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, true, 0);
    }
    public Grid(Player? player, Transform2D transform, bool placeable)
    {
        Transform = transform;
        Cells = new TCell[3][];
        for (int i = 0; i < 3; i++)
        {
            Cells[i] = new TCell[3];
            for (int j = 0; j < 3; j++)
            {
                Vector2 cellPosition = PixelPosition(new Address(i, j));
                Transform2D cellTransform = new Transform2D(cellPosition, 0, 1);
                TCell cell = (TCell)new TCell().Create(null, cellTransform, placeable);
                Cells[i][j] = cell;
                cell.Clicked += HandleClickedCell;
            }
        }
        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, true, TransitionValue);
    }
    public Grid(Grid<TCell> original, bool placeable)
    {
        Transform = original.Transform;
        Cells = new TCell[3][];
        for (int i = 0; i < 3; i++)
        {
            Cells[i] = new TCell[3];
            for (int j = 0; j < 3; j++)
            {
                TCell cell = original.Cells[i][j];
                TCell newCell = (TCell)cell.DeepCopyPlacable(placeable);
                Cells[i][j] = newCell;
                newCell.Clicked += HandleClickedCell;
            }
        }
        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, true, TransitionValue);
    }
    public Grid(Grid<TCell> original, IEnumerable<ICell> TCellrace, Player player, bool placeable)
    {
        if (TCellrace.Last().Placeable == false)
        {
            throw new Exception("You Cannot place on that cell");
        }
        Transform = original.Transform;
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
                cell.Clicked += HandleClickedCell;
            }
        }

        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, true, TransitionValue);
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
                TCell cell = original.Cells[i][j];
                bool cellPlaceable = (i == nextX) && (j == nextY) && (Player == null);
                if (cell.Equals(targetCell))
                {
                    cell = (TCell)cell.Place(TCellrace.Skip(1), player, cellPlaceable);
                }
                else
                {
                    cell = (TCell)cell.DeepCopyPlacable(cellPlaceable);
                }
                Cells[i][j] = cell;
                cell.Clicked += HandleClickedCell;
            }
        }
    }
    [JsonInclude]
    public Transform2D Transform { get; }
    [JsonInclude]
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
    public event IClickableCell.ClickHandler? Clicked;
    [JsonInclude]
    //[JsonConverter(typeof(Json.Array2DConverter))]
    public TCell[][] Cells { get; }
    [JsonInclude]
    public Tile WinningPlayerTile { get; }
    public bool InTransition
    {
        get
        {
            if (WinningPlayerTile.InTransition)
            {
                return true;
            }
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Cells[i][j].InTransition)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    public float TransitionValue
    {
        get
        {
            var values = new float[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    values[i, j] = Cells[i][j].TransitionValue;
                }
            }
            float max = values[0, 0];
            foreach (var value in values)
            {
                max = Math.Max(value, max);
            }
            if (WinningPlayerTile != null)
                max = Math.Max(max, WinningPlayerTile.TransitionValue);
            return max;
        }
    }
    public List<Address> PathTo(ICell cell) => this.PathToCell(cell);
    public bool Contains(ICell cell) => this.ContainsCell(cell);
    public void Draw()
    {
        bool gridCellInTransition = false;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                gridCellInTransition |= Cells[i][j].InTransition;
            }
        }
        if (Player != null && gridCellInTransition == false)
        {
            WinningPlayerTile.Draw();
            return;
        }
        DrawGrid();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Cells[i][j].Draw();
            }
        }
    }
    public void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Cells[i][j].Update();
            }
        }
        bool gridCellInTransition = false;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                gridCellInTransition |= Cells[i][j].InTransition;
            }
        }
        if (gridCellInTransition == false)
            WinningPlayerTile.Update();
    }
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
    public void HandleClickedCell(IEnumerable<ICell> cells)
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
    public void DrawGrid()
    {
        Transform2D transform = Transform;
        int lineGap = (int)(50 * transform.Scale);
        int lineLength = (int)(130 * transform.Scale);
        int lineWidth = (int)(2 * transform.Scale);
        int x = (int)transform.Position.X;
        int y = (int)transform.Position.Y;
        Color color = Color.LIGHTGRAY;

        Graphics.Draw.Rectangle(x - lineWidth / 2 + lineGap / 2, y - lineLength / 2, lineWidth, lineLength, color);
        Graphics.Draw.Rectangle(x - lineWidth / 2 - lineGap / 2, y - lineLength / 2, lineWidth, lineLength, color);
        Graphics.Draw.Rectangle(x - lineLength / 2, y - lineWidth / 2 + lineGap / 2, lineLength, lineWidth, color);
        Graphics.Draw.Rectangle(x - lineLength / 2, y - lineWidth / 2 - lineGap / 2, lineLength, lineWidth, color);
    }
}