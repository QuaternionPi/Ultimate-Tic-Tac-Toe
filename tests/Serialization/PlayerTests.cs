using Raylib_cs;
using System.Text.Json;
using UltimateTicTacToe.Game;
using UltimateTicTacToe.Serialization.Json;

namespace UltimateTicTacToeTests;


public class PlayerSerializationTests
{
    public PlayerSerializationTests()
    {

    }
    [Fact]
    public void CanSerializeHumanPlayer()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ColorConverter());

        var symbol = Player.Symbol.O;
        Player player = new Human(symbol, Color.BLUE, 0);
        var json = JsonSerializer.Serialize(player, options);
        var newPlayer = JsonSerializer.Deserialize<Player>(json, options);
        Assert.Equal(player.Score, newPlayer?.Score);
        Assert.Equal(player.Shape, newPlayer?.Shape);
        Assert.Equal(player.Color, newPlayer?.Color);
        Assert.Equal(player.GetToken(), newPlayer?.GetToken());
    }
    [Fact]
    public void CanSerializeBotPlayer()
    {
        var options = new JsonSerializerOptions();

        var evaluator = new LargeBoardEvaluator();
        var symbol = Player.Symbol.O;
        var player = new Bot(evaluator, symbol, Color.BLUE, 0);
        var json = JsonSerializer.Serialize(player, options);
        var newPlayer = JsonSerializer.Deserialize<Player>(json, options);
        Assert.Equal(player.Score, newPlayer?.Score);
        Assert.Equal(player.Shape, newPlayer?.Shape);
        Assert.Equal(player.Color, newPlayer?.Color);
        Assert.Equal(player.GetToken(), newPlayer?.GetToken());
    }
}