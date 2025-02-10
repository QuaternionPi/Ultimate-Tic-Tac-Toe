using Raylib_cs;
using System.Text.Json;
using UltimateTicTacToe.Game;
using UltimateTicTacToe.Serialization.Json;

namespace UltimateTicTacToeTests;

public class GridSerializationTests
{
    public GridSerializationTests()
    {

    }
    [Fact]
    public void CanSerializeEmptyGrid()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new GridOfTConverter());
        var tiles = Enumerable.Range(0, 9).Select(x => new Tile(null));
        var winningPlayerTile = new Tile(null);
        var grid = new Grid<Tile>(tiles, winningPlayerTile);
        var json = JsonSerializer.Serialize(grid, options);
        var newGrid = JsonSerializer.Deserialize<Grid<Tile>>(json, options);
        Assert.Equal(grid.Winner, newGrid?.Winner);
        Assert.Equal(grid.AnyPlaceable, newGrid?.AnyPlaceable);
        Assert.Equal(grid.ToString(), newGrid?.ToString());
    }
}