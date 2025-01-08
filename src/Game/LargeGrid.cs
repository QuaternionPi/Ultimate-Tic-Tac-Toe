using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe.Game;

public class LargeGrid<TCell> : IDrawable, IUpdatable, ITransitional, IBoard<TCell>, IClickableCell
where TCell : IDrawable, IUpdatable, ITransitional, IClickableCell, new()
{
    public LargeGrid()
    {
        Transform = new Transform2D(Vector2.Zero, 0, 1);
        Cells = new TCell[9];
        for (int i = 0; i < 9; i++)
        {
            Vector2 cellPosition = PixelPosition(new Address(i));
            Transform2D cellTransform = new Transform2D(cellPosition, 0, 1);
            TCell cell = (TCell)new TCell().Create(null, cellTransform, false);
            Cells[i] = cell;
            cell.Clicked += HandleClickedCell;
        }
        Player = null;
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, true, 0);
    }
    public LargeGrid(Player? player, Transform2D transform, bool placeable)
    {
        Transform = transform;
        Cells = new TCell[9];
        for (int i = 0; i < 9; i++)
        {
            Vector2 cellPosition = PixelPosition(new Address(i));
            Transform2D cellTransform = new Transform2D(cellPosition, 0, 1);
            TCell cell = (TCell)new TCell().Create(null, cellTransform, placeable);
            Cells[i] = cell;
            cell.Clicked += HandleClickedCell;
        }
        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, true, TransitionValue);
    }
    public LargeGrid(LargeGrid<TCell> original, bool placeable)
    {
        Transform = original.Transform;
        Cells = new TCell[9];
        for (int i = 0; i < 9; i++)
        {
            TCell cell = original.Cells[i];
            TCell newCell = (TCell)cell.DeepCopyPlacable(placeable);
            Cells[i] = newCell;
            newCell.Clicked += HandleClickedCell;
        }
        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, true, TransitionValue);
    }
    public LargeGrid(LargeGrid<TCell> original, IEnumerable<ICell> TCelltrace, Player player, bool placeable)
    {
        Debug.Assert(TCelltrace.Last().Placeable != false, "You Cannot place on that cell");
        Transform = original.Transform;
        Cells = new TCell[9];

        ICell TCelloReplace = TCelltrace.Last();
        ICell targetCell = TCelltrace.First();

        for (int i = 0; i < 9; i++)
        {
            TCell cell = original.Cells[i];
            if (cell.Equals(targetCell))
            {
                cell = (TCell)cell.Place(TCelltrace.Skip(1), player, placeable);
            }
            else
            {
                cell = (TCell)cell.DeepCopyPlacable(placeable);
            }
            Cells[i] = cell;
            cell.Clicked += HandleClickedCell;
        }

        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, true, TransitionValue);
        if (Cells[0] is Tile || Player != null)
        {
            return;
        }

        Address nextPlayableAddress = original.PathTo(TCelloReplace).Last();
        int index = nextPlayableAddress.Index;
        TCell nextCell = Cells[index];

        if (nextCell.Placeable == false)
        {
            Cells[index] = (TCell)nextCell.DeepCopyPlacable(false);
            return;
        }
        for (int i = 0; i < 9; i++)
        {
            TCell cell = original.Cells[i];
            bool cellPlaceable = (i == index) && (Player == null);
            if (cell.Equals(targetCell))
            {
                cell = (TCell)cell.Place(TCelltrace.Skip(1), player, cellPlaceable);
            }
            else
            {
                cell = (TCell)cell.DeepCopyPlacable(cellPlaceable);
            }
            Cells[i] = cell;
            cell.Clicked += HandleClickedCell;
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
            for (int i = 0; i < 9; i++)
                if (Cells[i].Placeable == true)
                    return true;
            return false;
        }
    }
    public event Action<IEnumerable<ICell>>? Clicked;
    [JsonInclude]
    //[JsonConverter(typeof(Json.Array2DConverter))]
    public TCell[] Cells { get; }
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
            for (int i = 0; i < 9; i++)
            {
                if (Cells[i].InTransition)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public float TransitionValue
    {
        get
        {
            var values = new float[9];
            for (int i = 0; i < 9; i++)
            {
                values[i] = Cells[i].TransitionValue;
            }
            float max = values[0];
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
        for (int i = 0; i < 9; i++)
        {
            gridCellInTransition |= Cells[i].InTransition;
        }
        if (Player != null && gridCellInTransition == false)
        {
            WinningPlayerTile.Draw();
            return;
        }
        DrawGrid();
        for (int i = 0; i < 9; i++)
        {
            Cells[i].Draw();
        }
    }
    public void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            Cells[i].Update();
        }
        bool gridCellInTransition = false;
        for (int i = 0; i < 9; i++)
        {
            gridCellInTransition |= Cells[i].InTransition;
        }
        if (gridCellInTransition == false)
            WinningPlayerTile.Update();
    }
    public ICell Create(Player? player, Transform2D transform, bool placeable)
    {
        return new Grid<TCell>(player, transform, placeable);
    }
    public ICell Place(IEnumerable<ICell> TCelltrace, Player player, bool placeable)
    {
        return new LargeGrid<TCell>(this, TCelltrace, player, placeable);
    }
    public ICell DeepCopyPlacable(bool placeable)
    {
        return new LargeGrid<TCell>(this, placeable);
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