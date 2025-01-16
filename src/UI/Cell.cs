using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;
namespace UltimateTicTacToe.UI;

public class Cell
{
    public Player.Player? Player { get; set; }
    public Transform2D Transform { get; }
    public bool InTransition { get { return TransitionValue != 0; } }
    public float TransitionValue { get; protected set; }
    public event Action<Cell>? Clicked;
    public Cell(Game.ICell cell, Transform2D transform, float transitionValue = 1)
    {
        Player = cell.Player;
        Transform = transform;
        TransitionValue = transitionValue;
    }
    public Cell(Player.Player player, Transform2D transform, float transitionValue = 1)
    {
        Player = player;
        Transform = transform;
        TransitionValue = transitionValue;
    }
    public void UpdateCell(ICell cell)
    {
        Player = cell.Player;
    }
    public void Update()
    {
        if (Player != null)
            TransitionValue = Math.Max(0, TransitionValue - 0.07f / MathF.Sqrt(Transform.Scale));
        bool leftMouse = Mouse.IsMouseButtonReleased(0);
        Vector2 mousePosition = Mouse.GetMousePosition();
        Rectangle rectangle = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
        bool collision = CheckCollision.PointRec(mousePosition, rectangle);
        if (leftMouse && collision)
        {
            Clicked?.Invoke(this);
        }
    }
    public void Draw()
    {
        if (Player == null)
        {
            return;
        }
        Player.DrawSymbol(Transform, TransitionValue);
    }
    public void DrawPlaceableIndicator()
    {
        int width = 20;
        Graphics.Draw.Rectangle((int)Transform.Position.X - width / 2, (int)Transform.Position.Y - width / 2, width, width, Color.LIGHTGRAY);
    }
}