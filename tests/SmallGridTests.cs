using UltimateTicTacToe;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToeTests;

public class SmallGridTests
{
    private readonly Grid<Tile> _grid;
    public SmallGridTests()
    {
        _grid = new();
    }
    [Fact]
    public void EmptyGridHasNineEmptyCells()
    {
        foreach(var row in _grid.Cells)
            foreach (var cell in row)
                Assert.Null(cell.Player);
    }
}
