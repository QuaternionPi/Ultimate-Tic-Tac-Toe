using System.Text.Json.Serialization;
using Raylib_cs;
using UltimateTicTacToe.UI;

namespace UltimateTicTacToe.Game;

[JsonDerivedType(typeof(Human), "Human")]
[JsonDerivedType(typeof(Bot), "Bot")]
public abstract partial class Player(Player.Symbol shape, Color color, int score)
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
    public int Score { get; set; } = score;
    public Symbol Shape { get; set; } = shape;
    public Color Color { get; set; } = color;
    public event Action<Player, LargeGrid<Grid<Tile>, Tile>, (int, int)>? PlayTurn;
    protected void InvokePlayTurn(Player player, LargeGrid<Grid<Tile>, Tile> board, (int, int) move) => PlayTurn?.Invoke(player, board, move);
    public abstract void BeginTurn(LargeGrid<Grid<Tile>, Tile> board, LargeBoard<Grid<Tile>, Tile> largeBoard, Player opponent);
    public abstract void EndTurn();
    public abstract void Update();
}