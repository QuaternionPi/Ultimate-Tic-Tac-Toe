using Raylib_cs;
using System.Text.Json;
using UltimateTicTacToe.Game;
using UltimateTicTacToe.Serialization.Json;

namespace UltimateTicTacToeTests;


public class GameSerializationTests
{
    public GameSerializationTests()
    {

    }
    [Fact]
    public void CanSerializeEmptyGame()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ColorConverter());
        options.Converters.Add(new GridOfTConverter());
        options.Converters.Add(new LargeGridOfTConverter());

        var human = new Human(Player.Symbol.X, Color.RED, 0);
        var bot = new Bot(new(), new(0), Player.Symbol.X, Color.RED, 0);
        var board = EmptyLargeGrid();
        var game = new Game(0, human, bot, board, board, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 2));
        var json = JsonSerializer.Serialize(game, options);
        var newGame = JsonSerializer.Deserialize<Game>(json, options);
        Assert.Equal(game.TurnNumber, newGame?.TurnNumber);
        Assert.Equal(game.ToString(), newGame?.ToString());
    }
    [Fact]
    public void CanSerializePartialGame()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ColorConverter());
        options.Converters.Add(new GridOfTConverter());
        options.Converters.Add(new LargeGridOfTConverter());

        var human = new Human(Player.Symbol.X, Color.RED, 0);
        var bot = new Bot(new(), new(0), Player.Symbol.X, Color.RED, 0);
        var board = EmptyLargeGrid();
        var game = new Game(0, human, bot, board, board, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 2));
        var json = JsonSerializer.Serialize(game, options);
        var newGame = JsonSerializer.Deserialize<Game>(json, options);
        Assert.Equal(game.TurnNumber, newGame?.TurnNumber);
        Assert.Equal(game.ToString(), newGame?.ToString());
    }
    protected static LargeGrid<Grid<Tile>, Tile> EmptyLargeGrid()
    {
        var cells = new Grid<Tile>[9];
        for (int i = 0; i < 9; i++)
        {
            cells[i] = EmptyGrid();
        }
        var victoryTile = new Tile(null);
        return new LargeGrid<Grid<Tile>, Tile>(cells, victoryTile);
    }
    protected static Grid<Tile> EmptyGrid()
    {
        var cells = new Tile[9];
        for (int i = 0; i < 9; i++)
        {
            cells[i] = new Tile(null);
        }
        var victoryTile = new Tile(null);
        return new Grid<Tile>(cells, victoryTile);
    }
}