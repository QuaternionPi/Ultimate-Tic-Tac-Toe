using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;
namespace UltimateTicTacToe.UI;

public class Cell
{
    private Player? Player { get; set; }
    private Transform2D Transform { get; }
    public bool InTransition { get { return TransitionValue != 0; } }
    public float TransitionValue { get; protected set; }
    public TimeSpan TransitionTime { get; protected set; }
    private Rectangle MouseCollider { get; }
    private Rectangle PlacementIndicator { get; }
    public event Action<Cell>? Clicked;
    public Cell(ICell cell, Transform2D transform, TimeSpan transitionTime)
    {
        Player = cell.Player;
        Transform = transform;
        TransitionValue = Player == null ? 0 : 1;
        MouseCollider = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
        int width = 20;
        TransitionTime = transitionTime;
        PlacementIndicator = new Rectangle((int)Transform.Position.X - width / 2, (int)Transform.Position.Y - width / 2, width, width);
    }
    public Cell(Player player, Transform2D transform, float transitionValue = 1)
    {
        Player = player;
        Transform = transform;
        TransitionValue = transitionValue;
        TransitionTime = new TimeSpan(0);
        MouseCollider = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
    }
    public void UpdateCell(ICell cell)
    {
        TransitionValue = TransitionValue != 0 ? TransitionValue : Player == cell.Player ? 0 : 1;
        Player = cell.Player;
    }
    public void Update()
    {
        var deltaTime = 1.0 / 60.0;
        var transitionTimeElapsed = TransitionTime.TotalSeconds == 0 ?
            1
            : deltaTime / TransitionTime.TotalSeconds / Math.Sqrt(Transform.Scale);
        if (Player != null)
        {
            var transitionDelta = transitionTimeElapsed;
            TransitionValue = (float)Math.Max(0, TransitionValue - transitionDelta);
        }
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
        if (Player == null)
            Graphics.Draw.RectangleRec(PlacementIndicator, Color.LIGHTGRAY);
    }
}