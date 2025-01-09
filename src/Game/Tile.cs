using System.Numerics;
using System.Text.Json.Serialization;
using Raylib_cs;

namespace UltimateTicTacToe.Game;
public class Tile : IDrawable, IUpdatable, ITransitional, ICell
{
    public Tile()
    {
        Player = null;
        Transform = new Transform2D(Vector2.Zero, 0, 0);
        Placeable = false;
    }
    public Tile(Player? player, Transform2D transform, bool placeable, float transitionValue)
    {
        Player = player;
        Transform = transform;
        Placeable = placeable && Player == null;
        TransitionValue = transitionValue;
        if (Player == null)
        {
            TransitionValue = 0;
        }
    }
    [JsonInclude]
    public Player? Player { get; }
    [JsonInclude]
    public bool Placeable { get; }
    [JsonInclude]
    public Transform2D Transform { get; }
    public event Action<ICell>? Clicked;
    public bool InTransition { get { return TransitionValue != 0; } }
    public float TransitionValue { get; protected set; }
    public void Draw()
    {
        if (Placeable)
        {
            int width = 20;
            Graphics.Draw.Rectangle((int)Transform.Position.X - width / 2, (int)Transform.Position.Y - width / 2, width, width, Color.LIGHTGRAY);
        }
        if (Player == null)
        {
            return;
        }
        Player.DrawSymbol(Transform, TransitionValue);
    }
    public void Update()
    {
        bool leftMouse = Mouse.IsMouseButtonReleased(0);
        Vector2 mousePosition = Mouse.GetMousePosition();
        Rectangle rectangle = new Rectangle(Transform.Position.X - 25, Transform.Position.Y - 25, 50, 50);
        bool collision = CheckCollision.PointRec(mousePosition, rectangle);
        if (leftMouse && collision)
        {
            Clicked?.Invoke(this);
        }
        TransitionValue = Math.Max(0, TransitionValue - 0.07f / MathF.Sqrt(Transform.Scale));
    }
    public ICell Place(Player player, bool placeable)
    {
        return new Tile(player, Transform, placeable, 1);
    }
    public ICell DeepCopyPlacable(bool placeable)
    {
        return new Tile(Player, Transform, placeable, TransitionValue);
    }
    public List<Address> PathTo(ICell cell) => new List<Address>();
}