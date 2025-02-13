using System.Diagnostics;

namespace UltimateTicTacToe.Game;

public class Grid<TCell> : IBoard<Grid<TCell>, TCell>
where TCell : ICell<TCell>
{
    public TCell[] Cells { get; }
    public TCell WinningPlayerCell { get; }
    public Player.Token? Winner { get; }
    public bool AnyPlaceable { get; }
    public int[] Moves { get; }
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

        Winner = this.Winner();
        AnyPlaceable = Winner == null && Cells.Any((cell) => cell.Placeable);
        Moves = [..
            from i in Enumerable.Range(0, 9)
            where Cells[i].Placeable
            select i
        ];

        WinningPlayerCell = winningPlayerCell;
    }
    public Grid(Grid<TCell> original, Player.Token token, int index)
    {
        Debug.Assert(original.Cells[index].Placeable, "You Cannot place on that cell");
        Cells = new TCell[9];

        for (int i = 0; i < 9; i++)
        {
            TCell originalCell = original.Cells[i];
            Cells[i] = i == index ? originalCell.Place(token) : originalCell;
        }

        Winner = original.Winner ?? UpdateWinner(token, index);
        AnyPlaceable = Winner == null && Cells.Any((cell) => cell.Placeable);
        Moves = [..
            from i in Enumerable.Range(0, 9)
            where Cells[i].Placeable
            select i
        ];

        WinningPlayerCell = original.WinningPlayerCell.Place(Winner);
    }
    public Grid<TCell> Place(Player.Token token, int index)
    {
        return new Grid<TCell>(this, token, index);
    }
    protected Player.Token? UpdateWinner(Player.Token token, int move)
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
        return winLines.Any(line => this[line].All(cell => cell.Owner == token)) ? token : null;
    }
}