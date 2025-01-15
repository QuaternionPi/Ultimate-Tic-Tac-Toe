using System.Diagnostics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;

public class LargeGrid<TGrid, TCell> : ILargeBoard<TGrid, TCell>
where TGrid : IBoard<TCell>
where TCell : ICell
{
    [JsonInclude]
    public Transform2D Transform { get; }
    [JsonInclude]
    public bool[] Placeable { get; }
    public TGrid[] Grids { get; }
    public TCell WinningPlayerCell { get; }
    [JsonInclude]
    public Player.Player? Player { get; }
    public bool AnyPlaceable { get; }
    public IEnumerable<(int, int)> PlayableIndices { get; }
    public LargeGrid(IEnumerable<TGrid> grids, TCell winningPlayerCell)
    {
        Grids = grids.ToArray();
        Debug.Assert(Grids.Length == 9);
        Placeable = new bool[Grids.Length];
        for (int i = 0; i < 9; i++)
        {
            Placeable[i] = true;
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
    public LargeGrid(LargeGrid<TGrid, TCell> original, Player.Player player, int index, int innerIndex)
    {
        Debug.Assert(original.Placeable[index], "You Cannot place on that cell");
        Transform = original.Transform;
        Grids = new TGrid[9];

        for (int i = 0; i < 9; i++)
        {
            TGrid originalGrid = original.Grids[i];
            TGrid grid;
            if (i == index)
            {
                grid = (TGrid)originalGrid.Place(player, innerIndex);
            }
            else
            {
                grid = originalGrid;
            }
            Grids[i] = grid;
        }
        Placeable = new bool[9];
        TGrid nextGrid = Grids[innerIndex];
        if (nextGrid.AnyPlaceable == false || nextGrid.Player != null)
        {
            for (int i = 0; i < 9; i++)
            {
                if (Grids[i].AnyPlaceable == false)
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
        Player = this.Winner();
        AnyPlaceable = Player == null && Grids.Any((x) => x.AnyPlaceable);
        PlayableIndices =
            from i in Enumerable.Range(0, 9)
            where Placeable[i]
            from j in Grids[i].PlayableIndices
            select (i, j);

        WinningPlayerCell = (TCell)original.WinningPlayerCell.Place(Player);
    }
    public ILargeBoard<TGrid, TCell> Place(Player.Player player, int index, int innerIndex)
    {
        return new LargeGrid<TGrid, TCell>(this, player, index, innerIndex);
    }
}