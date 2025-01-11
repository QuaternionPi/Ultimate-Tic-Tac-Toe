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
        Placeable = new bool[Cells.Length];
        for (int i = 0; i < 9; i++)
        {
            Placeable[i] = true;
        }
        foreach (var cell in cells)
        {
            cell.Clicked += HandleClickedCell;
        }
        Transform = transform;
        WinningPlayerTile = winningPlayerTile;
    }
    public LargeGrid(LargeGrid<TGrid, TCell> original, TGrid targetGrid, TCell targetCell, Player.Player player)
    {
        Debug.Assert(original.Placeable[original.Location(targetCell).Item1], "You Cannot place on that cell");
        Transform = original.Transform;
        Cells = new TGrid[9];

        for (int i = 0; i < 9; i++)
        {
            TGrid originalCell = original.Cells[i];
            TGrid cell;
            if (originalCell.Equals(targetGrid))
            {
                cell = (TGrid)originalCell.Place(targetCell, player);
            }
            else
            {
                cell = (TGrid)originalCell.Place(targetCell, originalCell.Player);
            }
            Cells[i] = cell;
            cell.Clicked += HandleClickedCell;
        }

        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerTile = new Tile(Player, victoryTileTransform, TransitionValue);
        Placeable = new bool[9];
        if (Player != null)
        {
            for (int i = 0; i < 9; i++)
            {
                Placeable[i] = false;
            }
            return;
        }

        int nextPlayableAddress = original.Location(targetCell).Item2;
        TGrid nextCell = Cells[nextPlayableAddress];

        if (nextCell.AnyPlaceable == false || nextCell.Player != null)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Cells[i].AnyPlaceable == false || Cells[i].Player != null)
                {
                    Placeable[i] = false;
                }
                else
                {
                    Placeable[i] = true;
                }
            }
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                Placeable[i] = false;
            }
            Placeable[nextPlayableAddress] = true;
        }
    }
    [JsonInclude]
    public Transform2D Transform { get; }
    [JsonInclude]
    public Player.Player? Player { get; }
    public bool AnyPlaceable
    {
        get
        {
            if (Player != null)
                return false;
            for (int i = 0; i < 9; i++)
                if (Cells[i].AnyPlaceable == true)
                    return true;
            return false;
        }
    }
    public event Action<ILargeBoard<TGrid, TCell>, TGrid, TCell>? Clicked;
    [JsonInclude]
    //[JsonConverter(typeof(Json.Array2DConverter))]
    public TGrid[] Cells { get; }
    public bool[] Placeable { get; }
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
    public (int, int) Location(TCell cell)
    {
        for (int i = 0; i < 9; i++)
        {
            if (Cells[i].Contains(cell))
            {
                int innerAddress = Cells[i].Location(cell);
                return (i, innerAddress);
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
    public ILargeBoard<TGrid, TCell> Place(TGrid grid, TCell cell, Player.Player player)
    {
        return new LargeGrid<TGrid, TCell>(this, grid, cell, player);
    }
    public ICell Place(IEnumerable<ICell> TCelltrace, Player.Player player)
    {
        throw new NotImplementedException();
        //return (ICell)new LargeGrid<TGrid, TCell>(this, TCelltrace, player, placeable);
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