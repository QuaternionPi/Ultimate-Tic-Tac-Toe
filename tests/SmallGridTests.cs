using Raylib_cs;
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
        foreach (var row in grid.Cells)
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
    public void WonByThreeInARow(int x1, int y1, int x2, int y2, int x3, int y3)
    {
        Grid<Tile> grid = new(null, new(), true);
        Bot player = new(Player.Symbol.O, Color.BLUE, 0);

        Tile topLeft = grid.Cells[x1][y1];
        grid = grid.Place([topLeft], player, true);
        Tile topMiddle = grid.Cells[x2][y2];
        grid = grid.Place([topMiddle], player, true);
        Tile topRight = grid.Cells[x3][y3];
        grid = grid.Place([topRight], player, true);

        Player? winner = grid.Winner();

        Assert.Equal(winner, player);
    }
    [Fact]
    public void GameNotWonByConstellationOfSix()
    {
        Grid<Tile> grid = new(null, new(), true);
        Bot player = new(Player.Symbol.O, Color.BLUE, 0);

        Tile topLeft = grid.Cells[0][0];
        grid = grid.Place([topLeft], player, true);
        Tile topMiddle = grid.Cells[0][1];
        grid = grid.Place([topMiddle], player, true);
        Tile leftMiddle = grid.Cells[1][0];
        grid = grid.Place([leftMiddle], player, true);
        Tile bottomRight = grid.Cells[2][2];
        grid = grid.Place([bottomRight], player, true);
        Tile bottomMiddle = grid.Cells[2][1];
        grid = grid.Place([bottomMiddle], player, true);
        Tile rightMiddle = grid.Cells[1][2];
        grid = grid.Place([rightMiddle], player, true);

        Player? winner = grid.Winner();

        Assert.Null(winner);
    }
    [Fact]
    public void GameCanTie()
    {
        Grid<Tile> grid = new(null, new(), true);
        Bot player1 = new(Player.Symbol.O, Color.BLUE, 0);
        Bot player2 = new(Player.Symbol.X, Color.RED, 0);

        Tile topLeft = grid.Cells[0][0];
        grid = grid.Place([topLeft], player1, true);
        Tile topMiddle = grid.Cells[0][1];
        grid = grid.Place([topMiddle], player1, true);
        Tile topRight = grid.Cells[0][2];
        grid = grid.Place([topRight], player2, true);
        Tile leftMiddle = grid.Cells[1][0];
        grid = grid.Place([leftMiddle], player2, true);
        Tile trueMiddle = grid.Cells[1][1];
        grid = grid.Place([trueMiddle], player1, true);
        Tile rightMiddle = grid.Cells[1][2];
        grid = grid.Place([rightMiddle], player1, true);
        Tile bottomLeft = grid.Cells[2][0];
        grid = grid.Place([bottomLeft], player1, true);
        Tile bottomMiddle = grid.Cells[2][1];
        grid = grid.Place([bottomMiddle], player2, true);
        Tile bottomRight = grid.Cells[2][2];
        grid = grid.Place([bottomRight], player2, true);

        Player? winner = grid.Winner();

        Assert.Null(winner);
    }

}
