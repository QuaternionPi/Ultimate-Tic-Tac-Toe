using Raylib_cs;
using UltimateTicTacToe;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToeTests;

public class SmallGridTests
{
    public SmallGridTests()
    {

    }
    [Fact]
    public void EmptyGridHasNineEmptyCells()
    {
        Grid<Tile> grid = new();
        foreach(var row in grid.Cells)
            foreach (var cell in row)
                Assert.Null(cell.Player);
    }
    [Theory]
    [InlineData(0, 0, 0, 1, 0, 2)] // Top row
    [InlineData(1, 0, 1, 1, 1, 2)] // Middle row
    [InlineData(2, 0, 2, 1, 2, 2)] // Bottom row
    [InlineData(0, 0, 1, 0, 2, 0)] // Left column
    [InlineData(0, 1, 1, 1, 2, 1)] // Middle column
    [InlineData(0, 2, 1, 2, 2, 2)] // Right column
    [InlineData(0, 0, 1, 1, 2, 2)] // Negative diagonal
    [InlineData(2, 0, 1, 1, 0, 2)] // Positive diagonal
    public void WonByThreeInARow(int x1, int y1, int x2, int y2, int x3, int y3){
        Grid<Tile> grid = new(null, new(), true);
        Bot player = new(Player.Symbol.O, Color.BLUE, 0);

        Tile topLeft = grid.Cells[x1][y1];
        grid = (Grid<Tile>)grid.Place([topLeft], player, true);
        Tile topMiddle = grid.Cells[x2][y2];
        grid = (Grid<Tile>)grid.Place([topMiddle], player, true);
        Tile topRight = grid.Cells[x3][y3];
        grid = (Grid<Tile>)grid.Place([topRight], player, true);

        Player? winner = grid.Winner();

        Console.WriteLine(winner);
        Assert.Equal(winner, player);
    }
}
