using System.Diagnostics;
using System.Text.Json.Serialization;

namespace UltimateTicTacToe.Game;

public class LargeGrid<TGrid, TCell> : ILargeBoard<LargeGrid<TGrid, TCell>, TGrid, TCell>
where TGrid : IBoard<TGrid, TCell>
where TCell : ICell<TCell>
{
    public Transform2D Transform { get; }
    public bool[] Placeable { get; }
    public TGrid[] Grids { get; }
    public TCell WinningPlayerCell { get; }
    public Player.Token? Winner { get; }
    public bool AnyPlaceable { get; }
    public (int, int)[] Moves { get; }
    public TGrid this[int index]
    {
        get { return Grids[index]; }
        private set { Grids[index] = value; }
    }
    public IEnumerable<TGrid> this[IEnumerable<int> indices]
    {
        get { foreach (var index in indices) yield return Grids[index]; }
    }
    public LargeGrid(IEnumerable<TGrid> grids, TCell winningPlayerCell)
    {
        Grids = [.. grids];
        Debug.Assert(Grids.Length == 9);
        Placeable = new bool[Grids.Length];
        for (int i = 0; i < 9; i++)
        {
            Placeable[i] = Grids[i].AnyPlaceable;
        }
        Winner = this.Winner();
        AnyPlaceable = Winner == null && Grids.Any((x) => x.AnyPlaceable);
        Moves = [..
            from i in Enumerable.Range(0, 9)
            where Placeable[i]
            from j in Grids[i].Moves
            select (i, j)
        ];

        WinningPlayerCell = winningPlayerCell;
    }
    public LargeGrid(LargeGrid<TGrid, TCell> original, Player.Token token, (int, int) move)
    {
        var index = move.Item1;
        var innerIndex = move.Item2;
        Debug.Assert(original.Placeable[index], "You Cannot place on that cell");
        Transform = original.Transform;
        Grids = new TGrid[9];

        for (int i = 0; i < 9; i++)
        {
            TGrid originalGrid = original.Grids[i];
            Grids[i] = i == index ? originalGrid.Place(token, innerIndex) : originalGrid;
        }
        Placeable = new bool[9];
        TGrid nextGrid = Grids[innerIndex];
        if (nextGrid.AnyPlaceable)
        {
            for (int i = 0; i < 9; i++)
            {
                Placeable[i] = false;
            }
            Placeable[innerIndex] = true;
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                Placeable[i] = Grids[i].AnyPlaceable;
            }
        }
        Winner = UpdateWinner(token, move);
        AnyPlaceable = Winner == null && Grids.Any((x) => x.AnyPlaceable);
        Moves = [..
            from i in Enumerable.Range(0, 9)
            where Placeable[i]
            from j in Grids[i].Moves
            select (i, j)
        ];

        WinningPlayerCell = original.WinningPlayerCell.Place(Winner);
    }
    public LargeGrid<TGrid, TCell> Place(Player.Token player, (int, int) move)
    {
        return new LargeGrid<TGrid, TCell>(this, player, move);
    }
    protected Player.Token? UpdateWinner(Player.Token token, (int, int) move)
    {
        int[][] winLines = move.Item1 switch
        {
            0 => [[1, 2], [3, 6], [4, 8]],
            1 => [[0, 2], [4, 7]],
            2 => [[0, 1], [4, 6], [5, 8]],
            3 => [[0, 6], [4, 5]],
            4 => [[0, 8], [1, 7], [2, 6], [3, 5]],
            5 => [[2, 8], [3, 4]],
            6 => [[0, 3], [2, 4], [7, 8]],
            7 => [[1, 4], [6, 8]],
            8 => [[0, 4], [2, 5], [6, 7]],
            _ => throw new ArgumentOutOfRangeException(nameof(move)),
        };
        return winLines.Any(line => this[line].All(grid => grid.Winner == token)) ? token : null;
    }
}