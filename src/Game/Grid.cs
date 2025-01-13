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
    public Grid(Grid<TCell> original, TCell targetCell, Player.Player player)
    {
        Debug.Assert(targetCell.Player == null, "You Cannot place on that cell");
        Transform = original.Transform;
        Cells = new TCell[9];

        for (int i = 0; i < 9; i++)
        {
            TCell originalCell = original.Cells[i];
            TCell cell;
            if (originalCell.Equals(targetCell))
            {
                cell = (TCell)originalCell.Place(player);
            }
            else
            {
                cell = (TCell)originalCell.Place(originalCell.Player);
            }
            Cells[i] = cell;
        }

        Player = this.Winner();
        Transform2D victoryTileTransform = new(Transform.Position, 0, Transform.Scale * 4);
        WinningPlayerCell = (TCell)original.WinningPlayerCell.Place(player);
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
    public IBoard<TCell> Place(TCell cell, Player.Player player)
    {
        return new Grid<TCell>(this, cell, player);
    }
    public ICell Place(IEnumerable<ICell> TCelltrace, Player.Player player)
    {
        return (ICell)new Grid<TCell>(this, (TCell)TCelltrace.First(), player);
    }
}