using System.Diagnostics;
using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe.Game;

public class LargeGrid<TGrid, TCell> : IDrawable, IUpdatable, ITransitional, ILargeBoard<TGrid, TCell>
where TGrid : IDrawable, IUpdatable, ITransitional, IBoard<TCell>
where TCell : IDrawable, IUpdatable, ITransitional, ICell
{
    public LargeGrid(IEnumerable<TGrid> cells, Tile winningPlayerTile, Transform2D transform)
    {
        Cells = cells.ToArray();
        foreach (var cell in cells)
        {
            cell.Clicked += HandleClickedCell;
        }
        Transform = transform;
        WinningPlayerTile = winningPlayerTile;
    }
    public LargeGrid(LargeGrid<TGrid, TCell> original, bool placeable)
    {
        Transform = original.Transform;
        Cells = new TGrid[9];
        for (int i = 0; i < 9; i++)
        {
            TGrid cell = original.Cells[i];
            TGrid newCell = (TGrid)cell.DeepCopyPlacable(placeable);
            Cells[i] = newCell;
            newCell.Clicked += HandleClickedCell;
        }
        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, true, TransitionValue);
    }
    public LargeGrid(LargeGrid<TGrid, TCell> original, TGrid targetGrid, TCell targetCell, Player player, bool placeable)
    {
        Debug.Assert(targetCell.Placeable != false, "You Cannot place on that cell");
        Transform = original.Transform;
        Cells = new TGrid[9];

        for (int i = 0; i < 9; i++)
        {
            TGrid cell = original.Cells[i];
            if (cell.Equals(targetGrid))
            {
                cell = (TGrid)cell.Place(targetCell, player, placeable);
            }
            else
            {
                cell = (TGrid)cell.DeepCopyPlacable(placeable);
            }
            Cells[i] = cell;
            cell.Clicked += HandleClickedCell;
        }

        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, true, TransitionValue);
        if (Player != null)
        {
            return;
        }

        Address nextPlayableAddress = original.Location(targetCell).Item2;
        int index = nextPlayableAddress.Index;
        TGrid nextCell = Cells[index];

        if (nextCell.Placeable == false)
        {
            Cells[index] = (TGrid)nextCell.DeepCopyPlacable(false);
            return;
        }
        for (int i = 0; i < 9; i++)
        {
            TGrid cell = original.Cells[i];
            bool cellPlaceable = (i == index) && (Player == null);
            if (cell.Equals(targetGrid))
            {
                cell = (TGrid)cell.Place(targetCell, player, cellPlaceable);
            }
            else
            {
                cell = (TGrid)cell.DeepCopyPlacable(cellPlaceable);
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
    public event Action<ILargeBoard<TGrid, TCell>, TGrid, TCell>? Clicked;
    [JsonInclude]
    //[JsonConverter(typeof(Json.Array2DConverter))]
    public TGrid[] Cells { get; }
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
    public (Address, Address) Location(TCell cell)
    {
        for (int i = 0; i < 9; i++)
        {
            if (Cells[i].Contains(cell))
            {
                Address address = new(i);
                Address innerAddress = Cells[i].Location(cell);
                return (address, innerAddress);
            }
        }
        throw new Exception($"Cell: {cell} was not found");
    }
    public bool Contains(TCell cell)
    {
        return Cells.Any((x) => x.Contains(cell));
    }
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
    public ILargeBoard<TGrid, TCell> Place(TGrid grid, TCell cell, Player player, bool placeable)
    {
        return new LargeGrid<TGrid, TCell>(this, grid, cell, player, placeable);
    }
    public ICell Place(IEnumerable<ICell> TCelltrace, Player player, bool placeable)
    {
        throw new NotImplementedException();
        //return (ICell)new LargeGrid<TGrid, TCell>(this, TCelltrace, player, placeable);
    }
    public ICell DeepCopyPlacable(bool placeable)
    {
        return (ICell)new LargeGrid<TGrid, TCell>(this, placeable);
    }
    public void HandleClickedCell(IBoard<TCell> board, TCell cell)
    {
        Clicked?.Invoke(this, (TGrid)board, (TCell)cell);
    }
    public static Vector2 PixelPosition(Transform2D transform, int i, int j)
    {
        int x = (int)(transform.Position.X + (i - 1) * 50 * transform.Scale);
        int y = (int)(transform.Position.Y + (j - 1) * 50 * transform.Scale);
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