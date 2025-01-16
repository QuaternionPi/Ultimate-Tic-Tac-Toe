using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;
namespace UltimateTicTacToe.UI;

public class Cell
{
    public Player.Player? Player { get; set; }
    private Transform2D Transform { get; }
    public bool InTransition { get { return TransitionValue != 0; } }
    public float TransitionValue { get; protected set; }
    private Rectangle MouseCollider { get; }
    private Rectangle PlacementIndicator { get; }
    public event Action<Cell>? Clicked;
    public Cell(ICell cell, Transform2D transform)
    {
        Player = cell.Player;
        Transform = transform;
        TransitionValue = Player == null ? 0 : 1;
        MouseCollider = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
        int width = 20;
        PlacementIndicator = new Rectangle((int)Transform.Position.X - width / 2, (int)Transform.Position.Y - width / 2, width, width);
    }
    public Cell(Player.Player player, Transform2D transform, float transitionValue = 1)
    {
        Player = player;
        Transform = transform;
        TransitionValue = transitionValue;
        MouseCollider = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
    }
    public void UpdateCell(ICell cell)
    {
        TransitionValue = TransitionValue != 0 ? TransitionValue : Player == cell.Player ? 0 : 1;
        Player = cell.Player;
    }
    public void Update()
    {
        if (Player != null)
            TransitionValue = Math.Max(0, TransitionValue - 0.07f / MathF.Sqrt(Transform.Scale));
        bool leftMouse = Mouse.IsMouseButtonReleased(0);
        Vector2 mousePosition = Mouse.GetMousePosition();
        bool collision = CheckCollision.PointRec(mousePosition, MouseCollider);
        if (leftMouse && collision)
        {
            Clicked?.Invoke(this);
        }
    }
    public void Draw()
    {
        Player?.DrawSymbol(Transform, TransitionValue);
    }
    public void DrawPlaceableIndicator()
    {
        Graphics.Draw.RectangleRec(PlacementIndicator, Color.LIGHTGRAY);
    }
}