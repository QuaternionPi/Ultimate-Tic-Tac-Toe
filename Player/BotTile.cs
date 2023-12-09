using System.Numerics;
using Raylib_cs;

namespace UltimateTicTacToe
{
    public partial class Bot
    {
        public class Tile : IDrawable, IUpdateable, ITransitionable, IClickableCell
        {
            public Tile()
            {
                Player = null;
                Transform = new LinearTransform(Vector2.Zero, 0, 0);
                Placeable = false;
            }
            public Tile(Player? player, LinearTransform transform, bool placeable, float transitionValue)
            {
                Player = player;
                Transform = transform;
                Placeable = placeable && Player == null;
                TransitionValue = transitionValue;
                if (Player == null)
                {
                    transitionValue = 0;
                }
            }
            public Player? Player { get; }
            public bool Placeable { get; }
            public LinearTransform Transform { get; }
            public event IClickableCell.ClickHandler? Clicked;
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
                    var cells = new List<ICell>() { this };
                    Clicked?.Invoke(cells);
                }
                TransitionValue = Math.Max(0, TransitionValue - 0.07f);
            }
            public ICell Create(Player? player, LinearTransform transform, bool placeable)
            {
                return new Tile(player, transform, placeable, 0);
            }
            public ICell Place(IEnumerable<ICell> cells, Player player, bool placeable)
            {
                return new Tile(player, Transform, placeable, 1);
            }
            public ICell DeepCopyPlacable(bool placeable)
            {
                return new Tile(Player, Transform, placeable, TransitionValue);
            }
            public List<Address> PathTo(ICell cell) => new List<Address>();
            public bool Contains(ICell cell) => cell.Equals(this);
        }
    }
}