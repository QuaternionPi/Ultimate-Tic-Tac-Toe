using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;
using UltimateTicTacToe.UI;

namespace UltimateTicTacToe.Game;
public abstract class Player
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
    }
    [JsonInclude]
    public int Score;
    [JsonInclude]
    public Symbol Shape { get; set; }
    [JsonInclude]
    public Color Color { get; set; }
    public event Action<Player, LargeGrid<Grid<Tile>, Tile>, int, int>? PlayTurn;
    protected void InvokePlayTurn(Player player, LargeGrid<Grid<Tile>, Tile> board, int index, int innerIndex) => PlayTurn?.Invoke(player, board, index, innerIndex);
    public abstract void BeginTurn(LargeGrid<Grid<Tile>, Tile> board, LargeBoard<Grid<Tile>, Tile> largeBoard, Player opponent);
    public abstract void EndTurn();
    public abstract void Update();
    public void DrawSymbol(Transform2D transform, float transitionValue)
    {
        transitionValue = Math.Clamp(transitionValue, 0, 1);
        switch (Shape)
        {
            case Symbol.X:
                {
                    Vector2 maxDimensions = new Vector2(40, 5) * transform.Scale;
                    float leftLength = maxDimensions.X * Math.Clamp(1 - transitionValue, 0, 0.5f) * 2;
                    float rightLength = maxDimensions.X * (Math.Clamp(1 - transitionValue, 0.5f, 1) - 0.5f) * 2;
                    Rectangle leftRectangle = new Rectangle(
                        transform.Position.X,
                        transform.Position.Y,
                        leftLength,
                        maxDimensions.Y);
                    Rectangle rightRectangle = new Rectangle(
                        transform.Position.X,
                        transform.Position.Y,
                        rightLength,
                        maxDimensions.Y);

                    Graphics.Draw.RectanglePro(leftRectangle, maxDimensions / 2, 45, Color);
                    Graphics.Draw.RectanglePro(rightRectangle, maxDimensions / 2, -45, Color);
                    return;
                }
            case Symbol.O:
                {
                    int width = 4 * (int)transform.Scale;
                    int innerRadius = 11 * (int)transform.Scale;
                    int outerRadius = innerRadius + width;
                    float angle = 360f * (1 - transitionValue) + 180;

                    Graphics.Draw.Ring(transform.Position, innerRadius, outerRadius, 180, angle, 50, Color);
                    return;
                }
        }
    }
}