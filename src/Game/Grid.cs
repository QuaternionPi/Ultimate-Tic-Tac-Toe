using System.Diagnostics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;

public class Grid<TCell> : IBoard<TCell>
where TCell : ICell
{
    public Grid(IEnumerable<TCell> cells, TCell winningPlayerCell)
    {
        Cells = cells.ToArray();
        WinningPlayerCell = winningPlayerCell;
    }
    public Grid(Grid<TCell> original, Player.Player player, int index)
    {
        Debug.Assert(original.Cells[index].Player == null, "You Cannot place on that cell");
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
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerCell = (TCell)original.WinningPlayerCell.Place(Player);
    }
    [JsonInclude]
    public Transform2D Transform { get; }
    [JsonInclude]
    public Player.Player? Player { get; }
    public bool AnyPlaceable { get { return Cells.Any((x) => x.Player == null); } }
    [JsonInclude]
    //[JsonConverter(typeof(Json.Array2DConverter))]
    public TCell[] Cells { get; }
    public TCell WinningPlayerCell { get; }
    public int Location(TCell cell)
    {
        return Cells.ToList().IndexOf(cell);
    }
    public bool Contains(TCell cell)
    {
        return Cells.Any((x) => x.Equals(cell));
    }
    public IBoard<TCell> Place(Player.Player player, int index)
    {
        return new Grid<TCell>(this, player, index);
    }
}