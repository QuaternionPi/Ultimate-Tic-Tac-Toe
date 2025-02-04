using System.Numerics;
using Raylib_cs;
using UltimateTicTacToe.Game;
namespace UltimateTicTacToe.UI;

public class Cell
{
    private Player.Token? Owner { get; set; }
    private Transform2D Transform { get; }
    public bool InTransition { get { return TransitionValue != 0; } }
    public float TransitionValue { get; protected set; }
    public TimeSpan TransitionTime { get; protected set; }
    private Rectangle MouseCollider { get; }
    private Rectangle PlacementIndicator { get; }
    public event Action<Cell>? Clicked;
    public Cell(ICell cell, Transform2D transform, TimeSpan transitionTime)
    {
        Owner = cell.Owner;
        Transform = transform;
        TransitionValue = Owner == null ? 0 : 1;
        MouseCollider = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
        int width = 20;
        TransitionTime = transitionTime;
        PlacementIndicator = new Rectangle((int)Transform.Position.X - width / 2, (int)Transform.Position.Y - width / 2, width, width);
    }
    public Cell(Player.Token winner, Transform2D transform, float transitionValue = 1)
    {
        Owner = winner;
        Transform = transform;
        TransitionValue = transitionValue;
        TransitionTime = new TimeSpan(0);
        MouseCollider = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
    }
    public void UpdateCell(ICell cell)
    {
        TransitionValue = TransitionValue != 0 ? TransitionValue : Owner == cell.Owner ? 0 : 1;
        Owner = cell.Owner;
    }
    public void Update()
    {
        var deltaTime = 1.0 / 60.0;
        var transitionTimeElapsed = TransitionTime.TotalSeconds == 0 ?
            1
            : deltaTime / TransitionTime.TotalSeconds / Math.Sqrt(Transform.Scale);
        if (Owner != null)
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
        DrawSymbol(Transform, TransitionValue);
    }
    public void DrawPlaceableIndicator()
    {
        if (Owner == null)
            Graphics.Draw.RectangleRec(PlacementIndicator, Color.LIGHTGRAY);
    }
    public void DrawSymbol(Transform2D transform, float transitionValue)
    {
        transitionValue = Math.Clamp(transitionValue, 0, 1);
        switch (Owner?.Symbol)
        {
            case Player.Symbol.X:
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

                    Graphics.Draw.RectanglePro(leftRectangle, maxDimensions / 2, 45, Owner.Color);
                    Graphics.Draw.RectanglePro(rightRectangle, maxDimensions / 2, -45, Owner.Color);
                    return;
                }
            case Player.Symbol.O:
                {
                    int width = 4 * (int)transform.Scale;
                    int innerRadius = 11 * (int)transform.Scale;
                    int outerRadius = innerRadius + width;
                    float angle = 360f * (1 - transitionValue) + 180;

                    Graphics.Draw.Ring(transform.Position, innerRadius, outerRadius, 180, angle, 50, Owner.Color);
                    return;
                }
        }
    }
}