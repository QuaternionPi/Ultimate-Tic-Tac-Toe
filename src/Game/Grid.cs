using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Linq;

namespace UltimateTicTacToe.Game;

public class Grid<TCell> : IBoard<TCell>
where TCell : ICell<TCell>
{
    public Transform2D Transform { get; }
    [JsonInclude]
    public TCell[] Cells { get; }
    public TCell WinningPlayerCell { get; }
    public Player? Player { get; }
    public bool AnyPlaceable { get; }
    public IEnumerable<int> PlayableIndices { get; }
    public Grid(IEnumerable<TCell> cells, TCell winningPlayerCell)
    {
        Cells = cells.ToArray();
        Debug.Assert(Cells.Length == 9);

        Player = this.Winner();
        AnyPlaceable = Player == null && Cells.Any((x) => x.Placeable);
        PlayableIndices =
            from i in Enumerable.Range(0, 9)
            where Cells[i].Placeable
            select i;

        WinningPlayerCell = winningPlayerCell;
    }
    public Grid(Grid<TCell> original, Player player, int index)
    {
        Debug.Assert(original.Cells[index].Placeable, "You Cannot place on that cell");
        Transform = original.Transform;
        Cells = new TCell[9];

        for (int i = 0; i < 9; i++)
        {
            TCell originalCell = original.Cells[i];
            Cells[i] = i == index ? (TCell)originalCell.Place(player) : originalCell;
        }

        Player = this.Winner();
        AnyPlaceable = Player == null && Cells.Any((x) => x.Placeable);
        PlayableIndices =
            from i in Enumerable.Range(0, 9)
            where Cells[i].Placeable
            select i;

        WinningPlayerCell = (TCell)original.WinningPlayerCell.Place(Player);
    }
    public IBoard<TCell> Place(Player player, int index)
    {
        return new Grid<TCell>(this, player, index);
    }
}