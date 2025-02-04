using Raylib_cs;
using System.Text.Json;
using UltimateTicTacToe.Game;
using UltimateTicTacToe.Serialization.Json;

namespace UltimateTicTacToeTests;

public class TileSerializationTests
{
    public TileSerializationTests()
    {

    }
    [Fact]
    public void CanSerializeEmptyTile()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new TileConverter());
        var tile = new Tile(null);
        var json = JsonSerializer.Serialize(tile, options);
        var newTile = JsonSerializer.Deserialize<Tile>(json, options);
        Assert.Equal(tile.Owner, newTile?.Owner);
        Assert.Equal(tile.Placeable, newTile?.Placeable);
        Assert.Equal(tile.ToString(), newTile?.ToString());
    }
    [Fact]
    public void CanSerializeTile()
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