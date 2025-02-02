using System.Diagnostics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;

public class Grid<TCell> : IBoard<Grid<TCell>, TCell>
where TCell : ICell<TCell>
{
    public Transform2D Transform { get; }
    [JsonInclude]
    public TCell[] Cells { get; }
    public TCell WinningPlayerCell { get; }
    public Player? Player { get; }
    public bool AnyPlaceable { get; }
    public IEnumerable<int> Moves { get; }
    public TCell this[int index]
    {
        get { return Cells[index]; }
        private set { Cells[index] = value; }
    }
    public IEnumerable<TCell> this[IEnumerable<int> indices]
    {
        get { foreach (var index in indices) yield return Cells[index]; }
    }
    public Grid(IEnumerable<TCell> cells, TCell winningPlayerCell)
    {
        Cells = [.. cells];
        Debug.Assert(Cells.Length == 9);

        Player = this.Winner();
        AnyPlaceable = Player == null && Cells.Any((cell) => cell.Placeable);
        Moves =
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
            Cells[i] = i == index ? originalCell.Place(player) : originalCell;
        }

        Player = original.Player ?? Winner(player, index);
        AnyPlaceable = Player == null && Cells.Any((cell) => cell.Placeable);
        Moves =
            from i in Enumerable.Range(0, 9)
            where Cells[i].Placeable
            select i;

        WinningPlayerCell = original.WinningPlayerCell.Place(Player);
    }
    public Grid<TCell> Place(Player player, int index)
    {
        return new Grid<TCell>(this, player, index);
    }
    protected Player? Winner(Player player, int move)
    {
        int[][] winLines = move switch
        {
            0 => [[0, 1, 2], [0, 3, 6], [0, 4, 8]],
            1 => [[0, 1, 2], [1, 4, 7]],
            2 => [[0, 1, 2], [2, 4, 6], [2, 5, 8]],
            3 => [[0, 3, 6], [3, 4, 5]],
            4 => [[0, 4, 8], [1, 4, 7], [2, 4, 6], [3, 4, 5]],
            5 => [[2, 5, 8], [3, 4, 5]],
            6 => [[0, 3, 6], [2, 4, 6], [6, 7, 8]],
            7 => [[1, 4, 7], [6, 7, 8]],
            8 => [[0, 4, 8], [2, 5, 8], [6, 7, 8]],
            _ => throw new ArgumentOutOfRangeException(nameof(move)),
        };
        return winLines.Any(line => this[line].All(cell => cell.Player == player)) ? player : null;
    }
}