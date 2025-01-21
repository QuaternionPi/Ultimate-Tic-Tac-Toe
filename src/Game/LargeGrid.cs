using System.Diagnostics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;

public class LargeGrid<TGrid, TCell> : ILargeBoard<TGrid, TCell>
where TGrid : IBoard<TGrid, TCell>
where TCell : ICell<TCell>
{
    [JsonInclude]
    public Transform2D Transform { get; }
    [JsonInclude]
    public bool[] Placeable { get; }
    public TGrid[] Grids { get; }
    public TCell WinningPlayerCell { get; }
    [JsonInclude]
    public Player? Player { get; }
    public bool AnyPlaceable { get; }
    public IEnumerable<(int, int)> PlayableIndices { get; }
    public LargeGrid(IEnumerable<TGrid> grids, TCell winningPlayerCell)
    {
        Grids = [.. grids];
        Debug.Assert(Grids.Length == 9);
        Placeable = new bool[Grids.Length];
        for (int i = 0; i < 9; i++)
        {
            Placeable[i] = Grids[i].AnyPlaceable;
        }
        Player = this.Winner();
        AnyPlaceable = Player == null && Grids.Any((x) => x.AnyPlaceable);
        PlayableIndices =
            from i in Enumerable.Range(0, 9)
            where Placeable[i]
            from j in Grids[i].PlayableIndices
            select (i, j);

        WinningPlayerCell = winningPlayerCell;
    }
    public LargeGrid(LargeGrid<TGrid, TCell> original, Player player, int index, int innerIndex)
    {
        Debug.Assert(original.Placeable[index], "You Cannot place on that cell");
        Transform = original.Transform;
        Grids = new TGrid[9];

        for (int i = 0; i < 9; i++)
        {
            TGrid originalGrid = original.Grids[i];
            Grids[i] = i == index ? (TGrid)originalGrid.Place(player, innerIndex) : originalGrid;
        }
        Placeable = new bool[9];
        TGrid nextGrid = Grids[innerIndex];
        if (!nextGrid.AnyPlaceable)
        {
            for (int i = 0; i < 9; i++)
            {
                Placeable[i] = Grids[i].AnyPlaceable;
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
        Player = this.Winner();
        AnyPlaceable = Player == null && Grids.Any((x) => x.AnyPlaceable);
        PlayableIndices =
            from i in Enumerable.Range(0, 9)
            where Placeable[i]
            from j in Grids[i].PlayableIndices
            select (i, j);

        WinningPlayerCell = original.WinningPlayerCell.Place(Player);
    }
    public ILargeBoard<TGrid, TCell> Place(Player player, int index, int innerIndex)
    {
        return new LargeGrid<TGrid, TCell>(this, player, index, innerIndex);
    }
}