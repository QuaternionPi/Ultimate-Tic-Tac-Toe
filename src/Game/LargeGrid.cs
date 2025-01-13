using System.Diagnostics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;

public class LargeGrid<TGrid, TCell> : ILargeBoard<TGrid, TCell>
where TGrid : IBoard<TCell>
where TCell : ICell
{
    public LargeGrid(IEnumerable<TGrid> cells, TCell winningPlayerCell)
    {
        Cells = cells.ToArray();
        Placeable = new bool[Cells.Length];
        for (int i = 0; i < 9; i++)
        {
            Placeable[i] = true;
        }
        WinningPlayerCell = winningPlayerCell;
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
        }

        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerCell = (TCell)original.WinningPlayerCell.Place(Player);
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
    public TCell WinningPlayerCell { get; }
    public bool[] Placeable { get; }
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
    public ILargeBoard<TGrid, TCell> Place(TGrid grid, TCell cell, Player.Player player)
    {
        return new LargeGrid<TGrid, TCell>(this, grid, cell, player);
    }
}