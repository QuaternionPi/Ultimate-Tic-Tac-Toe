using System.Diagnostics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;

public class Grid<TCell> : IBoard<TCell>
where TCell : ICell
{
    [JsonInclude]
    public Transform2D Transform { get; }
    [JsonInclude]
    public Player.Player? Player { get; }
    public bool AnyPlaceable { get; }
    [JsonInclude]
    public TCell[] Cells { get; }
    public TCell WinningPlayerCell { get; }
    public IBoard<TCell> Place(Player.Player player, int index)
    {
        return new Grid<TCell>(this, player, index);
    }
    public Grid(IEnumerable<TCell> cells, TCell winningPlayerCell)
    {
        Cells = cells.ToArray();
        Debug.Assert(Cells.Length == 9);

        Player = this.Winner();
        AnyPlaceable = Player == null && Cells.Any((x) => x.Placeable);
        WinningPlayerCell = winningPlayerCell;
    }
    public Grid(Grid<TCell> original, Player.Player player, int index)
    {
        Debug.Assert(original.Cells[index].Placeable, "You Cannot place on that cell");
        Transform = original.Transform;
        Cells = new TCell[9];

        for (int i = 0; i < 9; i++)
        {
            TCell originalCell = original.Cells[i];
            TCell cell;
            if (i == index)
            {
                cell = (TCell)originalCell.Place(player);
            }
            else
            {
                cell = originalCell;
            }
            Cells[i] = cell;
        }

        Player = this.Winner();
        AnyPlaceable = Player == null && Cells.Any((x) => x.Placeable);
        WinningPlayerCell = (TCell)original.WinningPlayerCell.Place(Player);
    }
}