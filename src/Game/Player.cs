using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;
using UltimateTicTacToe.UI;

namespace UltimateTicTacToe.Game;
public abstract partial class Player
{
    public static readonly Color[] AllowedColors = [
        Color.RED,
        Color.BLUE,
        Color.GREEN,
        Color.MAROON,
        Color.DARKBLUE,
        Color.DARKGREEN,
    ];
    public enum Symbol { X, O };
    public Player(Symbol symbol, Color color, int score)
    {
        Shape = symbol;
        Color = color;
        Score = score;
        _token = new Token(symbol, color);
    }
    [JsonInclude]
    public int Score;
    [JsonInclude]
    public Symbol Shape { get; set; }
    [JsonInclude]
    public Color Color { get; set; }
    public event Action<Player, LargeGrid<Grid<Tile>, Tile>, (int, int)>? PlayTurn;
    protected void InvokePlayTurn(Player player, LargeGrid<Grid<Tile>, Tile> board, (int, int) move) => PlayTurn?.Invoke(player, board, move);
    public abstract void BeginTurn(LargeGrid<Grid<Tile>, Tile> board, LargeBoard<Grid<Tile>, Tile> largeBoard, Player opponent);
    public abstract void EndTurn();
    public abstract void Update();
}