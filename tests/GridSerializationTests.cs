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
        options.Converters.Add(new TileConverter());
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
    [Fact]
    public void CanSerializeGrid()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new TileConverter());
        options.Converters.Add(new ColorConverter());
        var symbol = Player.Symbol.O;
        var player = new Human(symbol, Color.BLUE, 0);
        var token = player.GetToken();

        var tile = new Tile(token);
        var json = JsonSerializer.Serialize(tile, options);
        var newTile = JsonSerializer.Deserialize<Tile>(json, options);
        Assert.Equal(tile.Owner, newTile?.Owner);
        Assert.Equal(tile.Placeable, newTile?.Placeable);
        Assert.Equal(tile.ToString(), newTile?.ToString());
    }
}