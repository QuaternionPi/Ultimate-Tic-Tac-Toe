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
        Debug.Assert(Cells.Length == 9);
        Placeable = new bool[Cells.Length];
        for (int i = 0; i < 9; i++)
        {
            Placeable[i] = true;
        }
        WinningPlayerCell = winningPlayerCell;
    }
    public LargeGrid(LargeGrid<TGrid, TCell> original, Player.Player player, int index, int innerIndex)
    {
        Debug.Assert(original.Placeable[index], "You Cannot place on that cell");
        Transform = original.Transform;
        Cells = new TGrid[9];

        for (int i = 0; i < 9; i++)
        {
            TGrid originalCell = original.Cells[i];
            TGrid cell;
            if (i == index)
            {
                cell = (TGrid)originalCell.Place(player, innerIndex);
            }
            else
            {
                cell = originalCell;
            }
            Cells[i] = cell;
        }

        Player = this.Winner();
        AnyPlaceable = Player == null && Cells.Any((x) => x.AnyPlaceable);
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

        TGrid nextCell = Cells[innerIndex];
        if (nextCell.AnyPlaceable == false || nextCell.Player != null)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Cells[i].AnyPlaceable == false)
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
            Placeable[innerIndex] = true;
        }
    }
    [JsonInclude]
    public Transform2D Transform { get; }
    [JsonInclude]
    public Player.Player? Player { get; }
    public bool AnyPlaceable { get; }
    [JsonInclude]
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
    public ILargeBoard<TGrid, TCell> Place(Player.Player player, int index, int innerIndex)
    {
        return new LargeGrid<TGrid, TCell>(this, player, index, innerIndex);
    }
}