using Raylib_cs;
using UltimateTicTacToe.Game;

namespace UltimateTicTacToeTests;

public class GridTests
{
    private Grid<Tile> EmptyGrid { get; }
    public GridTests()
    {
        List<Tile> cells = [
            new Tile(null),
            new Tile(null),
            new Tile(null),
            new Tile(null),
            new Tile(null),
            new Tile(null),
            new Tile(null),
            new Tile(null),
            new Tile(null)
        ];
        EmptyGrid = new(cells, new Tile(null));
    }
    [Fact]
    public void EmptyGridHasAllPlacableCells()
    {
        foreach (var cell in EmptyGrid.Cells)
        {
            Assert.True(cell.Placeable);
        }
    }
    [Fact]
    public void EmptyGridHasNoWinner()
    {
        Assert.True(EmptyGrid.Winner is null);
    }
    [Theory]
    [InlineData(0, 1, 2)] // Top row
    [InlineData(3, 4, 5)] // Middle row
    [InlineData(6, 7, 8)] // Bottom row
    [InlineData(0, 3, 6)] // Left column
    [InlineData(1, 4, 7)] // Middle column
    [InlineData(2, 5, 8)] // Right column
    [InlineData(0, 4, 8)] // Negative diagonal
    [InlineData(6, 4, 2)] // Positive diagonal
    public void WonByThreeInARow(int a, int b, int c)
    {
        var symbol = Player.Symbol.O;
        var player = new Human(symbol, Color.BLUE, 0);
        var token = player.GetToken();

        var grid = EmptyGrid;
        grid = grid.Place(token, a);
        grid = grid.Place(token, b);
        grid = grid.Place(token, c);

        Player.Token? winner = grid.Winner;

        Assert.Equal(winner, token);
    }
    [Fact]
    public void GameNotWonByConstellationOfSix()
    {
        var symbol = Player.Symbol.O;
        var player = new Human(symbol, Color.BLUE, 0);
        var token = player.GetToken();

        var grid = EmptyGrid;
        grid = grid.Place(token, 0);
        grid = grid.Place(token, 1);
        grid = grid.Place(token, 3);
        grid = grid.Place(token, 5);
        grid = grid.Place(token, 7);
        grid = grid.Place(token, 8);

        Player.Token? winner = grid.Winner;
        Assert.Null(winner);
    }
    [Fact]
    public void GameCanTie()
    {
        var symbol = Player.Symbol.O;
        var player1 = new Human(symbol, Color.BLUE, 0);
        var player2 = new Human(symbol, Color.RED, 0);
        var token1 = player1.GetToken();
        var token2 = player2.GetToken();

        var grid = EmptyGrid;
        grid = grid.Place(token1, 0);
        grid = grid.Place(token1, 1);
        grid = grid.Place(token2, 2);
        grid = grid.Place(token2, 3);
        grid = grid.Place(token2, 4);
        grid = grid.Place(token1, 5);
        grid = grid.Place(token1, 6);
        grid = grid.Place(token1, 7);
        grid = grid.Place(token2, 8);

        Player.Token? winner = grid.Winner;
        Assert.Null(winner);
    }

}
